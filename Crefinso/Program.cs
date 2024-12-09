using Crefinso.Components;
using Crefinso.Services;
using Crefinso.Services.Clientes;
using Crefinso.Services.Solicitudes;
using Crefinso.Services.Usuarios;
using CurrieTechnologies.Razor.SweetAlert2;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddScoped(o => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7083/"),
});

builder.Services.AddSweetAlert2();
builder.Services.AddScoped<AuthServices>();
builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<ClientServices>();
builder.Services.AddScoped<RequestServices>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
