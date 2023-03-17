using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.OpenApi.Models;
using System.Reflection;
using tfemshoes.API;
using tfemshoes.API.Authorization;
using tfemshoes.Domain.Context;
using tfemshoes.Domain.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddLogging(options =>
{
    options.AddConsole();
    options.SetMinimumLevel(LogLevel.Information);

    options.AddFilter<ApplicationInsightsLoggerProvider>("tfemshoes", LogLevel.Information);
    options.AddApplicationInsights(builder.Configuration["ApplicationInsights:InstrumentationKey"]);
});

if (!builder.Environment.IsDevelopment())
{
    // In a deployed environment, connect to the specified Azure Key Vault
    // This uses the "default" Identity, either a local user, or an Azure Managed Identity
    builder.Configuration.AddAzureKeyVault($"https://{builder.Configuration["KeyVaultName"]}.vault.azure.net/");
}

// Add Database Context awareness
builder.Services.AddDbContext<TFemShoesContext>(options =>
{
    var connection = "Server={0};Database={1};User Id={2};Password={3};";
    connection = String.Format(connection,
        builder.Configuration["DatabaseServerName"],
        builder.Configuration["DatabaseName"],
        builder.Configuration["DatabaseUserId"],
        builder.Configuration["DatabasePassword"]);
    options.UseSqlServer(connection, x => x.UseNetTopologySuite());
});

// Add services to the container.
builder.Services.AddScoped<IStoreEntityService, StoreEntityService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAzureMapsQueryService, AzureMapsQueryService>();

// Add the controllers to the context
builder.Services.AddControllers();

// Setup API Versioning
// Support reading version from the URL, Header, or Query String params
builder.Services.AddApiVersioning(setup =>
{
    setup.DefaultApiVersion = new ApiVersion(1, 0);
    setup.AssumeDefaultVersionWhenUnspecified = true;
    setup.ReportApiVersions = true;
    setup.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("api-version"),
        new QueryStringApiVersionReader("api-version")
    );
});

// Configure versioning support metadata for Swagger
builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

// Add Swagger Gen support
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "TFem Shoes Project API",
        Description = "An API for the TFem Shoes Project. Stores that are supportive of Trans-feminine people looking for comfortable shoes.",
        TermsOfService = new Uri("https://tfemshoes.azurewebsites.net/terms"),
        Contact = new OpenApiContact
        {
            Name = "TFem Shoes Dev Team",
            Url = new Uri("https://tfemshoes.azurewebsites.net/contact")
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsProduction())
{
    app.UseSwagger();

    // configure the Swagger middleware
    app.UseSwaggerUI(options =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseHttpsRedirection();

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);

app.UseMiddleware<BasicAuthMiddleware>();

app.MapControllers();

app.Run();
