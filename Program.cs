using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using Pickle;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();

var contentPath = Path.Combine(builder.Environment.ContentRootPath, "content");
if (!Directory.Exists(contentPath))
    Directory.CreateDirectory(contentPath);
var files = new Files(contentPath);
builder.Services.AddSingleton(files);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
    app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

var denyRequest = (HttpContext context, HttpStatusCode httpStatusCode) =>
{
    context.Response.Clear();
    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
    return Task.CompletedTask;
};

app.Use((context, next) =>
{
    if (!context.Request.Path.StartsWithSegments(new PathString("/_content"), out PathString remaining))
        return next(context);

    var regex = Regex.Match(remaining.Value ?? string.Empty, "([0-9]{4}/[0-9]{2}/[0-9]{2})\\.([a-z]+)");
    if (!regex.Success || regex.Captures.Count != 1 || regex.Groups.Count != 3)
        return denyRequest(context, HttpStatusCode.BadRequest);

    var dateStr = regex.Groups[1].Value.Replace('/', '-');
    if (!DateOnly.TryParse(dateStr, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
        return denyRequest(context, HttpStatusCode.BadRequest);

    if (date.ToDateTime(TimeOnly.MinValue) > DateTime.UtcNow.Date)
        return denyRequest(context, HttpStatusCode.Unauthorized);

    var fileType = regex.Groups[2].Value;
    context.Response.Headers.ContentDisposition = new StringValues($"filename=\"pickle-{dateStr}.{fileType}\";");
    return next(context);
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(files.ContentPath),
    RequestPath = "/_content"
});

app.Run();
