
using BookInformationService;
using BookInformationService.BusinessLayer;
using BookInformationService.DataAccessLayer;
using BookInformationService.DatabaseContext;
using BookInformationService.Services;
using BookInformationService.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

var configuration = new ConfigurationBuilder()
             .AddJsonFile("appsettings.json")
             .Build();

var defaultConnectionString = configuration.GetConnectionString("DefaultConnection");


AppSettings? appSettings = configuration.GetRequiredSection("AppSettings").Get<AppSettings>();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .WriteTo.File(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Serilog\\log_.txt"), rollOnFileSizeLimit: true, fileSizeLimitBytes: 1000000, rollingInterval: RollingInterval.Month, retainedFileCountLimit: 24, flushToDiskInterval: TimeSpan.FromSeconds(1))
    //.WriteTo.Email(emailInfo)                           
    .CreateLogger();


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc()
    .AddJsonTranscoding(); // Add JSON transcoding support

builder.Services.AddGrpcSwagger();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Version = "v1",
            Title = "Build REST gRPC Service in .NET with C#",
            Description = "This services for the book information.",
            TermsOfService = new Uri("https://example.com/terms"),
            Contact = new OpenApiContact
            {
                Name = "Jane Doe",
                Email = "jane.doe@example.com",
                Url = new Uri("https://example.com/contact")
            },
            License = new OpenApiLicense
            {
                Name = "Apache 2.0",
                Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0")
            }
        });

    //// Adding server options
    //c.AddServer(new OpenApiServer
    //{
    //    Url = "https://api.example.com/v1",
    //    Description = "Production server"
    //});
    //c.AddServer(new OpenApiServer
    //{
    //    Url = "https://staging.example.com/v1",
    //    Description = "Staging server"
    //});
    //c.AddServer(new OpenApiServer
    //{
    //    Url = "http://localhost:5000/v1",
    //    Description = "Local development server"
    //});

    var mainAssembly = Assembly.GetEntryAssembly();
    //var referencedAssembly = Assembly.LoadFrom(Path.Combine(AppContext.BaseDirectory, "BookInfoReservationModel.dll")); // Adjust the DLL name as needed

    var mainXmlFile = $"{mainAssembly.GetName().Name}.xml";
    //var referencedXmlFile = $"{referencedAssembly.GetName().Name}.xml";

    var mainXmlPath = Path.Combine(AppContext.BaseDirectory, mainXmlFile);
    //var referencedXmlPath = Path.Combine(AppContext.BaseDirectory, referencedXmlFile);

    c.IncludeXmlComments(mainXmlPath);
    //c.IncludeXmlComments(referencedXmlPath);
    c.IncludeGrpcXmlComments(mainXmlPath, includeControllerXmlComments: true);

    /* To access 
     http://localhost:3101/swagger/index.html
    */

    /* An error occurs if XML documentation generation is not enabled.
     Ensure that XML documentation generation is enabled for your project.
        1. Right-click on your project in Visual Studio.
        2. Select Properties.
        3. Go to the Build tab.
        4. Check the documentation file checkbox.
        5. Verify that the path specified matches what you expect (bin\Debug\net8.0\BookInformationService.xml)
     */
});


// EF
builder.Services.AddDbContext<SystemDbContext>(options =>
            options.UseSqlite(defaultConnectionString));

// Models
builder.Services.AddScoped<IBookInformationDL, BookInformationDL>();
builder.Services.AddScoped<IBookInformationBL, BookInformationBL>();

// Register FluentValidation validators
builder.Services.AddScoped<IValidator<GetBookInformationRequest>, GetBookInformationRequestValidator>();
builder.Services.AddScoped<IValidator<CreateBookInformationRequest>, CreateBookInformationRequestValidator>();
builder.Services.AddScoped<IValidator<UpdateBookInformationRequest>, UpdateBookInformationRequestValidator>();
builder.Services.AddScoped<IValidator<DeleteBookInformationRequest>, DeleteBookInformationRequestValidator>();

var app = builder.Build();

// SwaggerUI
app.UseSwagger();
if (app.Environment.IsDevelopment())
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookInformationService V1");
    });

// Configure the HTTP request pipeline.
app.MapGrpcService<BookInformationGrpcService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
