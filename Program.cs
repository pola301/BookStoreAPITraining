using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using BookInfo.API.DbContexts;
using BookInfo.API.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Reflection;



Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/bookInfo.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();
builder.Host.UseSerilog();
#if DEBUG
builder.Services.AddTransient<IMail,Mail>();
#else
builder.Services.AddTransient<IMail,CloudMailService>();
#endif

builder.Services.AddDbContext<BookInfoContext>(dbContextOptions => dbContextOptions.UseSqlite(builder.Configuration["ConnectionStrings:BooksInfoDbConnectionString"]));
// Add services to the container.
builder.Services.AddScoped<IBookInfoRep, BookInfoRep>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson()
.AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
{
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        ValidAudience = builder.Configuration["Authentication:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(builder.Configuration["Authentication:SecretForKey"])),
    }; });
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("must be from antwerp", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("book", "Antwerp");
    }
    );
});

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = ctx =>
    {
        ctx.ProblemDetails.Extensions.Add("additional info", "Additional info example");
        ctx.ProblemDetails.Extensions.Add("Server", Environment.MachineName);
    };
});
builder.Services.AddApiVersioning(setupAction =>
{
    setupAction.ReportApiVersions = true;
    setupAction.AssumeDefaultVersionWhenUnspecified = true;
    setupAction.DefaultApiVersion = new ApiVersion(1, 0);
}).AddMvc()
.AddApiExplorer(setupAction =>
{
    setupAction.SubstituteApiVersionInUrl = true;
});

var apiVersionProvider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

builder.Services.AddSwaggerGen(setupAction =>
{
    foreach(var Description in apiVersionProvider.ApiVersionDescriptions)
    {
        setupAction.SwaggerDoc($"{Description.GroupName}",
            new()
            {
                Title = "Book Info API",
                Version = Description.ApiVersion.ToString(),
                Description = "Through this api you can access books"
            });
    }
    var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

    setupAction.IncludeXmlComments(xmlCommentsFullPath);
});

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BookInfoContext>();
    context.Database.EnsureCreated();
}
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(setupAction =>
    {
        var descriptions = app.DescribeApiVersions();
        foreach (var description in descriptions)
        {
            setupAction.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();
app.UseEndpoints(endpoints =>endpoints.MapControllers());
app.Run();