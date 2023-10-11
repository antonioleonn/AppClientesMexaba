using Microsoft.EntityFrameworkCore;
using AppClientesMexaba.Models;

using AppClientesMexaba.Servicios.Contrato;
using AppClientesMexaba.Servicios.Implementacion;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Toolbelt.Extensions.DependencyInjection;
using AppClientesMexaba.Data;
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using System.Net.Http;
using static AppClientesMexaba.Models.ClContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


//INICIA CADENA DE CONEXIÓN SQLSERVER PRINCIPAL PARA AUTENTICACION 
builder.Services.AddDbContext<ClContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaAut"));
});
//FINALIZA CADENA DE CONEXIÓN SQLSERVER PARA AUTENTICACION

//INICIA CADENA DE CONEXIÓN SQLSERVER PARA SERVIDOR ACA
builder.Services.AddDbContext<ServidorACADbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("ServidorACA"));
});
//FINALIZA CADENA DE CONEXIÓN SQLSERVER PARA SERVIDOR ACA

//INICIA CADENA DE CONEXIÓN SQLSERVER PARA SERVIDOR ALV
builder.Services.AddDbContext<ServidorALVDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("ServidorALV"));
});
//FINALIZA CADENA DE CONEXIÓN SQLSERVER PARA SERVIDOR ALV

//INICIA LA UTILIZACIÓN DE CLASES DE SERVICIOS
builder.Configuration.AddJsonFile("appsettings.json");

builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<D_Cxccli>();

builder.Services.AddHttpClient();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

//FINALIZA LA UTILIZACIÓN DE CLASES DE SERVICIOS



// INICIA EL AÑADIR LA COOKIE DE LOGIN
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Inicio/IniciarSesion";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });
// FINALIZA EL AÑADIR LA COOKIE DE LOGUE

//CONFIGURACION DE CACHE EN DONDE SE BORRA PARA LA SESIÓN QUE YA SE CERRO
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

//Se añade
app.UseAuthentication();

app.UseAuthorization();

app.UseCssLiveReload();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Inicio}/{action=IniciarSesion}/{id?}");

});



app.Run();