using Crefinso.Components;
using Crefinso.Services;
using Crefinso.Services.Clientes;
using Crefinso.Services.Solicitudes;
using Crefinso.Services.Usuarios;
using Crefinso.Services.Empleos;
using CurrieTechnologies.Razor.SweetAlert2;
using Crefinso.Services.Pagos;
using Crefinso.Services.Prestamos;

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
builder.Services.AddScoped<JobServices>();
builder.Services.AddScoped<PaymentServices>();
builder.Services.AddScoped<LoanServices>();

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
