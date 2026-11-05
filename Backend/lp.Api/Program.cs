using System.Text;
using System.Threading.RateLimiting;
using lp.Application.Interface;
using lp.Application.Mappings;
using lp.Application.Options;
using lp.Application.Service;
using lp.Infrastructure;
using lp.Infrastructure.Data;
using lp.Infrastructure.PaymentGateway;
using lp.Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder=WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<LpDbConext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    );
});

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("LoginPolicy", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString()
                          ?? "unknown",

            factory: partition => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0,
                AutoReplenishment = true
            }
        )
    );

    options.OnRejected = async (context, cancellationToken) =>
    {
        if (context.Lease.TryGetMetadata(
            MetadataName.RetryAfter, out var retryAfter))
        {
            context.HttpContext.Response.Headers.RetryAfter =
                Math.Ceiling(retryAfter.TotalSeconds).ToString();
        }

        await context.HttpContext.Response.WriteAsync(
            "Too many requests. Please try again later.",
            cancellationToken
        );
    };
});

builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<IAdminService,AdminService>();
builder.Services.AddScoped<ICourseRepository,CourseRepository>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPaymenetService, PaymentService>();
builder.Services.AddScoped<ILessonRepository,LessonRepository>();
builder.Services.AddScoped<ILessonService,LessonService>();


builder.Services.Configure<KhaltiOptions>(builder.Configuration.GetSection("Khalti"));

builder.Services.AddHttpClient<IPaymentGateway, KhaltiPaymentGateway>();


builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer",new OpenApiSecurityScheme
    {
        Name="Authorization",
        Type=SecuritySchemeType.Http,
        Scheme="Bearer",
        BearerFormat="JWT",
        In=ParameterLocation.Header,
        Description="Enter your token here"

    });

    options.AddSecurityRequirement(document=>new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer",document),
            new List<string>()
        }
    });
});

var key=builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Cant find the token");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters=new TokenValidationParameters
    {
        ValidateIssuer=true,
        ValidateAudience=true,
        ValidateIssuerSigningKey=true,
        ValidateLifetime=true,

        ValidAudience=builder.Configuration["Jwt:Audience"],
        ValidIssuer=builder.Configuration["Jwt:Issuer"],

        IssuerSigningKey=new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(key)
        ),

        ClockSkew=TimeSpan.Zero,

    };
});

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<MappingProfile>();
});

var app=builder.Build();

using(var scope = app.Services.CreateScope())
{
    var context=scope.ServiceProvider.GetRequiredService<LpDbConext>();
    var configuration=scope.ServiceProvider.GetRequiredService<IConfiguration>();

    await AdminSeed.SeedAdminAsync(context,configuration);
};

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
    app.UseSwagger();
}

app.UseRouting();
app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();
