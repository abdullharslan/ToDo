using Business.Abstract;
using Business.Concrete;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Microsoft.EntityFrameworkCore;

/*
* Program.cs, uygulamanın yapılandırmasını ve servis kayıtlarını içerir.
* Bu dosya:
* - Dependency Injection yapılandırması
* - Veritabanı bağlantısı
* - JWT authentication
* - CORS politikaları
* - Middleware pipeline
* ayarlarını yönetir.
*/

var builder = WebApplication.CreateBuilder(args);

/*
* Veritabanı Yapılandırması
* PostgreSQL veritabanı bağlantısı için DbContext kaydı.
* Bağlantı bilgileri appsettings.json'dan alınır.
*/
builder.Services.AddDbContext<AppDbContext>(options =>
{
   var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
   options.UseNpgsql(connectionString);
});

/*
* Dependency Injection Kayıtları
* Service ve Repository'lerin lifetime'ları Scoped olarak ayarlanır.
* Bu sayede her HTTP isteği için yeni bir instance oluşturulur.
*/
builder.Services.AddControllers();
// Repository Registrations
builder.Services.AddScoped<IToDoItemRepository, ToDoItemRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
// Service Registrations
builder.Services.AddScoped<IToDoItemService, ToDoItemService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();

/*
* JWT Authentication Yapılandırması
* Token doğrulama parametreleri ve güvenlik ayarları burada yapılır.
*/
var jwtKey = builder.Configuration["Jwt:Key"] 
   ?? throw new InvalidOperationException("JWT Key is not configured.");
var key = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(x =>
{
   x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
   x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
   x.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
   x.SaveToken = true;
   x.TokenValidationParameters = new TokenValidationParameters
   {
       ValidateIssuerSigningKey = true,
       IssuerSigningKey = new SymmetricSecurityKey(key),
       ValidateIssuer = true,
       ValidateAudience = true,
       ValidIssuer = builder.Configuration["Jwt:Issuer"],
       ValidAudience = builder.Configuration["Jwt:Audience"],
       ValidateLifetime = true,
       ClockSkew = TimeSpan.Zero
   };
});

/*
* CORS (Cross-Origin Resource Sharing) Yapılandırması
* Frontend uygulamasının API'ye erişim izinleri burada yapılandırılır.
*/
builder.Services.AddCors(options =>
{
   options.AddPolicy("AllowedOrigins",
       policyBuilder =>
       {
           policyBuilder
               .WithOrigins(
                   builder.Configuration.GetSection("AppSettings:AllowedOrigins").Get<string[]>()
                   ?? new[] { "http://localhost:4200" }
               )
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
       });
});

var app = builder.Build();

/*
* Middleware Pipeline Yapılandırması
* Middleware'ler belirli bir sırayla eklenir.
*/
if (app.Environment.IsDevelopment())
{
   app.UseDeveloperExceptionPage();
}
else
{
   app.UseExceptionHandler("/api/error");
   app.UseHsts();
}

/*
* HTTPS Yönlendirmesi
* Tüm HTTP isteklerini HTTPS'e yönlendirir
*/
// app.UseHttpsRedirection();

/*
* Routing Middleware
* HTTP isteklerini doğru endpoint'lere yönlendirir
*/
app.UseRouting();

/*
* CORS Middleware
* Cross-Origin isteklere izin verir
* Authentication'dan önce yapılandırılmalıdır
*/
app.UseCors("AllowedOrigins");

/*
* Authentication ve Authorization Middleware'leri
* Kimlik doğrulama ve yetkilendirme işlemlerini yönetir
*/
app.UseAuthentication(); // JWT kimlik doğrulama
app.UseAuthorization(); // Yetkilendirme

/*
* Controller Endpoint'leri
* API rotalarını yapılandırır ve controller'ları kullanıma açar
*/
app.MapControllers();

// Uygulamayı başlat
app.Run();