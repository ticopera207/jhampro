using Microsoft.EntityFrameworkCore;
using jhampro.Models;
using jhampro.Service;
using Amazon.S3;
using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime; // 👈 Importante


var builder = WebApplication.CreateBuilder(args);

// Configurar la conexión a la base de datos
var connectionString = builder.Configuration.GetConnectionString("PostgreSQLConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

// 🧩 Agregar servicios necesarios
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

// ✅ Configurar credenciales AWS desde appsettings.json
var awsOptions = new AWSOptions
{
    Credentials = new BasicAWSCredentials(
        builder.Configuration["AWS:AccessKey"],
        builder.Configuration["AWS:SecretKey"]
    ),
    Region = RegionEndpoint.GetBySystemName(builder.Configuration["AWS:Region"])
};

builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<IAmazonS3>(); // Ahora sí funcionará correctamente

// Registrar font resolver global
PdfSharpCore.Fonts.GlobalFontSettings.FontResolver = new CustomFontResolver();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();