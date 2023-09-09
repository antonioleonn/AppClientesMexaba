using Microsoft.EntityFrameworkCore;
using AppClientesMexaba.Models;

using AppClientesMexaba.Servicios.Contrato;
using AppClientesMexaba.Servicios.Implementacion;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


//INICIA CADENA DE CONEXI�N SQLSERVER
builder.Services.AddDbContext<ClContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSQL"));
    });
//FINALIZA CADENA DE CONEXI�N SQLSERVER

//INICIA LA UTILIZACI�N DE CLASES DE SERVICIOS
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
//FINALIZA LA UTILIZACI�N DE CLASES DE SERVICIOS

// INICIA EL A�ADIR LA COOKIE DE LOGIN
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Inicio/IniciarSesion";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });
// FINALIZA EL A�ADIR LA COOKIE DE LOGUE

//CONFIGURACION DE CACHE EN DONDE SE BORRA PARA LA SESI�N QUE YA SE CERRO
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(
        new ResponseCacheAttribute
        {
            NoStore = true,
            Location = ResponseCacheLocation.None,
        }
        );
});
//FINALIZA CONFIGURACION DE CACHE

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//Se a�ade
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Inicio}/{action=IniciarSesion}/{id?}");

app.Run();
