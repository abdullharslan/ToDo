using Business.Abstract;
using Business.Concrete;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Veritabanı bağlantısı
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Bağımlılıklar
builder.Services.AddControllers();
builder.Services.AddScoped<IToDoItemService, ToDoItemService>();
builder.Services.AddScoped<IToDoItemRepository, ToDoItemRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

// JWT ayarları
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]!);
builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = true; // Geliştirme aşamasında false bırakabilirsiniz
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// CORS ayarlarını ekle
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin() // Herhangi bir kaynaktan gelen isteklere izin ver
                   .AllowAnyMethod() // Tüm HTTP yöntemlerine izin ver
                   .AllowAnyHeader(); // Tüm başlıklara izin ver
        });
});

var app = builder.Build();

// Middleware'leri ekle
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// CORS middleware'ini burada ekleyin
app.UseCors("AllowAllOrigins"); // CORS middleware'ini ekle
app.UseAuthentication(); // JWT kimlik doğrulama
app.UseAuthorization(); // Yetkilendirme

// Controller'ları kullanıma aç
app.MapControllers();

app.Run();
