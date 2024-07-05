using DocumentsService.API.Storage.Abstractions;
using WebApiContrib.Core.Formatter.MessagePack;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using DocumentsService.Storage.Implementations.InMemoryDb;
using Microsoft.EntityFrameworkCore;
using DocumentsService.Storage.Implementations.HDD;
using DocumentsService.API.OutputFormatters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.RespectBrowserAcceptHeader = true;
    options.OutputFormatters.Insert(0, new CustomXmlOutputFormatter());
    options.OutputFormatters.Insert(0, new MessagePackOutputFormatter(new MessagePackFormatterOptions()));
})
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    }
   )
    .ConfigureApiBehaviorOptions(options =>
    {
        var builtInFactory = options.InvalidModelStateResponseFactory;

        options.InvalidModelStateResponseFactory = context =>
        {
            var logger = context.HttpContext.RequestServices
                                .GetRequiredService<ILogger<Program>>();

            logger.LogError("Invalid model state: {0}", context.ModelState);

            return builtInFactory(context);
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DocumentsDbContext>(options =>
{
    options.UseInMemoryDatabase("MyTestDb");
});

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

builder.Services.AddMemoryCache();
builder.Services.AddScoped<IDocumentsRepository, HddDocumentsRepository>();
//builder.Services.AddScoped<IDocumentsRepository, DbDocumentsRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
