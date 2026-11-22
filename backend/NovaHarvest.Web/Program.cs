var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();

var app = builder.Build();

app.MapStaticAssets();

// Configuro la localización española.
app.UseRequestLocalization("es-ES");

app.MapRazorPages();


   
app.Run();
