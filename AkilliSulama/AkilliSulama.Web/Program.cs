using AkilliSulama.Business;
using Microsoft.AspNetCore.Authentication.Cookies;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddIsKatmani(builder.Configuration.GetConnectionString("AkilliSulamaDb")!);

// Add services to the container.
builder.Services.AddControllersWithViews();
// OTURUM YÖNETİMİ: Cookie tabanlı kimlik doğrulama
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
    {
        o.LoginPath = "/Hesap/Giris";                 // Giriş yapmamış kişi buraya yönlendirilir
        o.AccessDeniedPath = "/Hesap/ErisimEngellendi"; // Yetkisi olmayan kişi buraya
        o.ExpireTimeSpan = TimeSpan.FromMinutes(30);    // 30 dk işlem yoksa oturum biter
        o.SlidingExpiration = true;                      // Her işlemde süre yenilenir
        o.Cookie.HttpOnly = true;                        // JavaScript cookie'yi okuyamaz (XSS koruması)
        o.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Sadece HTTPS üzerinden gönderilir
        o.Cookie.SameSite = SameSiteMode.Strict;         // Başka sitelerden gelen isteklerde gönderilmez
    });
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
