using CarRenting.API.Models;
using CarRenting.BusinessObjects.Models;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Routing;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

DotNetEnv.Env.TraversePath().Load();
var builder = WebApplication.CreateBuilder(args);

// Get key
var secretKey = builder.Configuration["AppSettings:SecretKey"];
var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

// Add services to the container.

// Default 
builder.Services
    .AddControllers().AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        })
    .AddOData(o => o.Select().Filter()
            .Count().OrderBy().Expand().SetMaxTop(100)
            .AddRouteComponents("odata", GetEdmModel())); ; 
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập token JWT (không cần thêm 'Bearer ' phía trước).",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
             {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
             },
             new string[] {}
        }
    });
});
builder.Services.AddSwaggerGen();
// AUTHEN
builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<AdminAccount>(builder.Configuration.GetSection("AdminAccount"));
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(opt =>
    {
        opt.SaveToken = true;
        opt.RequireHttpsMetadata = false;
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            // Tự cấp token
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["AppSettings:Audience"],
            ValidIssuer = builder.Configuration["AppSettings:Issuer"],

            // Ký vào token
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes), // thuật toán đối xứng

            ClockSkew = TimeSpan.Zero
        };
        opt.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                var role = context.Principal?.FindFirst(ClaimTypes.Role)?.Value;
                var jti = context.Principal?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

                if (role == UserRoles.Admin && jti != AdminSessionStore.CurrentJti)
                {
                    context.Fail("Phiên đăng nhập admin đã bị thay thế.");
                }
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddCors(option =>
{
    option.AddDefaultPolicy(p =>
            p.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());
});

// USING
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseODataBatching();
app.Use((context, next) =>
{
    var endpoints = context.GetEndpoint();
    if (endpoints == null)
    {
        return next();
    }

    IEnumerable<string> templates;
    IODataRoutingMetadata metadata = endpoints.Metadata.GetMetadata<IODataRoutingMetadata>();

    if (metadata != null)
    {
        templates = metadata.Template.GetTemplates();
    }

    return next();
});
app.UseCors();
app.UseHttpsRedirection();
// Use Authen
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


static IEdmModel GetEdmModel()
{
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<CarInformation>("CarInformation");
    return builder.GetEdmModel();
}