using DocumentsService.API.Storage.Abstractions;
using DocumentsService.API.Storage.Implementations;
using System.Xml;
using WebApiContrib.Core.Formatter.MessagePack;
using System.Text;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using DocumentsService.Storage.Implementations.InMemoryDb;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using DocumentsService;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.RespectBrowserAcceptHeader = true;
    options.OutputFormatters.Insert(0, new CustomXmlOutputFormatter());
    options.OutputFormatters.Insert(0, new MessagePackOutputFormatter(new MessagePackFormatterOptions()));

})
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.DictionaryKeyPolicy = null;
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

//builder.Services.AddScoped<IDocumentsRepository, HddDocumentsRepository>();
builder.Services.AddScoped<IDocumentsRepository, InMemoryDocumentsRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
