using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using MyWebApp.Data;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// =======================================
// 1️⃣ CONFIGURACIÓN DE SERVICIOS
// =======================================

// Base de datos (SQL Server)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// Configuración de Identity (autenticación y registro)
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    // Opciones de contraseña y seguridad
    options.SignIn.RequireConfirmedAccount = false; // pon en true si usarás confirmación por correo
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()  // Usa tu ApplicationDbContext
    .AddDefaultTokenProviders();                        // Tokens para recuperación de contraseña, etc.

// Habilitar Razor Pages (Identity las usa)
builder.Services.AddRazorPages();

// =======================================
// 2️⃣ CONSTRUIR LA APLICACIÓN
// =======================================
var app = builder.Build();

// =======================================
// 3️⃣ CONFIGURACIÓN DEL PIPELINE (MIDDLEWARES)
// =======================================

// Manejador de errores y seguridad HTTPS
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 🔒 Orden correcto: primero autenticación, luego autorización
app.UseAuthentication();
app.UseAuthorization();

// Mapeo de páginas Razor (incluye las de Identity)
app.MapRazorPages();

// Si luego agregas controladores MVC, puedes usar:
// app.MapControllers();

app.Run();
