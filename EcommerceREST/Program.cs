using EcommerceMVC.Services;
using EcommerceREST.Data;
using EcommerceREST.Services;
using EcommerceREST.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

//Agregar DbContext para la aplicaic�n principal
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql_connection")));

//Agregar servicios de la aplicaci�n

//Settings para la carga de archivos
builder.Services.Configure<UploadSettings>(builder.Configuration.GetSection("UploadSettings"));

//Servicio de marcas
builder.Services.AddScoped<IBrandService, BrandService>(); //Registra el servicio de marcas con su implementaci�n

// Servicio de categor�as
builder.Services.AddScoped<ICategoryService, CategoryService>(); //Registra el servicio de categor�as con su implementaci�n

// Servicio de productos
builder.Services.AddScoped<IProductService, ProductService>(); //Registra el servicio de productos con su implementaci�n






// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Servir archivos est�ticos (como im�genes, CSS, JS, etc.)
//app.UseStaticFiles(); //Permite servir archivos est�ticos desde la carpeta wwwroot
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Uploads")
        ),
    RequestPath = "/Uploads"
}); //Sirve para que los archivos subidos se puedan acceder desde la ruta /Uploads (Proporciona archivos al front   )

app.UseAuthorization();

app.MapControllers();

app.Run();
