using PoemPass.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();
builder.Services.AddSingleton<PoemPassGenerator>();

var app = builder.Build();
app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapControllerRoute("default", "{action=Index}", new { Controller = "Home" });

app.Run();