using GameServer.Models;
using GameServer.Services;
using Microsoft.EntityFrameworkCore;
using dotenv.net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Add OAuth Service
builder.Services.AddHttpClient();
builder.Services.AddScoped<OAuthService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var serverVersion = new MySqlServerVersion(new Version(8, 0, 39));
builder.Services.AddDbContext<GameContext>(opt => opt.UseMySql(connectionString, serverVersion));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // 세션 유효 시간
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


// .env 파일 로드
//DotEnv.Load();
DotEnv.Load(options: new DotEnvOptions(ignoreExceptions: false, envFilePaths: new[] { "../.env" }));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSession();

app.UseRouting();

app.MapControllers();

app.Run();