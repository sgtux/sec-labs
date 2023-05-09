using System.Buffers;
using System.Linq;
using System.Net.Mime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using NetCoreWebGoat.Config;
using NetCoreWebGoat.Data;
using NetCoreWebGoat.Extentions;
using NetCoreWebGoat.Formatters;
using NetCoreWebGoat.Repositories;

var builder = WebApplication.CreateBuilder(args);
var config = new AppConfig();

builder.Services.AddControllers(options =>
{
    options.InputFormatters.Add(new TextPlainInputFormatter());
});

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson()
    .AddRazorRuntimeCompilation();

builder.Services.AddOptions<MvcOptions>()
        .PostConfigure<IOptions<JsonOptions>, IOptions<MvcNewtonsoftJsonOptions>, ArrayPool<char>, ObjectPoolProvider, ILoggerFactory>(
    (mvcOptions, jsonOpts, newtonJsonOpts, charPool, objectPoolProvider, loggerFactory) =>
    {
        var formatter = mvcOptions.InputFormatters.OfType<NewtonsoftJsonInputFormatter>().First(i => i.SupportedMediaTypes.Contains("application/json"));
        formatter.SupportedMediaTypes.Add(Microsoft.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/csp-report"));
        mvcOptions.InputFormatters.RemoveType<NewtonsoftJsonInputFormatter>();
        mvcOptions.InputFormatters.Add(formatter);
    });

builder.Services.ConfigureAuth(config);

builder.Services.AddLogging(options => options.AddSimpleConsole(c => c.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] "));

builder.Services.AddSingleton(config);

new Database(config).Initialize();

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<PostRepository>();
builder.Services.AddScoped<CommentRepository>();
builder.Services.AddScoped<CspRepository>();

var app = builder.Build();

app.Use(async (context, next) =>
{
    var contentType = context.Request.Headers["Content-Type"];
    if(contentType == MediaTypeNames.Application.Xml)
    {
        context.Request.Headers.Remove("Content-Type");
        context.Request.Headers.Add("Content-Type", MediaTypeNames.Text.Plain);
    }
    await next.Invoke();
});

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (ctx, next) =>
    {
        if (!string.IsNullOrEmpty(config.CspHttpHeader))
            ctx.Response.Headers.Add("Content-Security-Policy", config.CspHttpHeader);
        await next();
    });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();