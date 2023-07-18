using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Passwordless.YourBackend.Auth;
using Passwordless.YourBackend.Database;
using Passwordless.YourBackend.Passwordless;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var cfg = builder.Configuration;
builder.Services.Configure<JwtOptions>(cfg.GetSection(PasswordlessOptions.Root));
builder.Services.AddTransient<IPasswordlessClient, PasswordlessClient>();
builder.Services.AddHttpClient("Passwordless", (sp,c) =>
{
    var root = (IConfigurationRoot)sp.GetRequiredService<IConfiguration>();
    var o = root.GetSection(PasswordlessOptions.Root).Get<PasswordlessOptions>();
    c.BaseAddress = new Uri(o.BaseUrl);
    c.DefaultRequestHeaders.Add("ApiSecret", o.Secret);
});
builder.Services.AddSingleton<IJwtTokenManager, JwtTokenManager>();
// Adding Authentication
builder.Services.Configure<JwtOptions>(cfg.GetSection(JwtOptions.Root));
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })

// Adding Jwt Bearer  
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
        };
    });
builder.Services.AddCors(p => p.AddPolicy("default", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));
builder.Services.AddDbContext<YourBackendContext>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("default");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();