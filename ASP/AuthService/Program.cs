using MongoDB.Driver;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AuthService.Data;

var builder = WebApplication.CreateBuilder(args);

// Проверка и получение конфигурации
var username = Environment.GetEnvironmentVariable("MongoDB_Username");
var password = Environment.GetEnvironmentVariable("MongoDB_Password");
var connectionString = builder.Configuration["MongoDB:ConnectionString"]
    .Replace("{username}", username)
    .Replace("{password}", password);

var mongoClient = new MongoClient(connectionString);
var database = mongoClient.GetDatabase(builder.Configuration["MongoDB:DatabaseName"]);


// Регистрация базы данных как сервиса
builder.Services.AddSingleton<IMongoDatabase>(database);
builder.Services.AddSingleton<MongoDbContext>();

// Настройка JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
