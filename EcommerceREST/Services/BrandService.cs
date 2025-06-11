using EcommerceREST.Data;
using EcommerceREST.DTOs;
using EcommerceREST.Models;
using EcommerceREST.Settings;
using EcommerceREST.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Http.Logging;
using Microsoft.Extensions.Options;

namespace EcommerceREST.Services
{
    public class BrandService : IBrandService //Implementa la interfaz IBrandService para definir los servicios de marcas
    {
        //Implemnetacion del servicio de marcas

        //// ------ Inyección de dependencias del contexto de la base de datos -------
        //La inyección de dependencias permite que el servicio pueda acceder a la base de datos sin necesidad de crear una
        //instancia del contexto directamente.

        private readonly ApplicationDbContext _context; //Contexto de la base de datos
        private readonly UploadSettings _uploadSettings; //Configuración de carga de archivos
        private readonly IWebHostEnvironment _env; //Entorno web para acceder a la ruta del servidor

        public BrandService(ApplicationDbContext context, IOptions<UploadSettings> uploadSettings, IWebHostEnvironment env) //Constructor que recibe el contexto de la base de datos
        {
            _context = context;
            _uploadSettings = uploadSettings.Value; //Obtiene la configuración de carga de archivos desde las opciones inyectadas
            _env = env;
        }
        //------------------------------------------------------------------------



        //---------------- Implementación de sercicios (Métodos dede ICategoryService) ---------------- \\

        // Método para obtener todas las marcas de la base de datos ------------------------------------------------------------------------------------
        public async Task<List<BrandDTO>> GetAllAsync()
        {
           var brands = await _context.Brands //Obtiene todas las marcas de la base de datos (Se requeire el contesxto)
                .Where(b => !b.IsDeleted) //Filtra las marcas que no estén eliminadas (IsDeleted es una propiedad del modelo Brand)
                .Select(b => new BrandDTO //Proyección a BrandDTO (es lo que pide la función) para devolver solo los datos necesarios
                {
                    Id = b.Id, //Id de la marca
                    Name = b.Name, //Nombre de la marca
                    LogoUrl = b.LogoUrl, //URL del logo de la marca
                    Active = b.Active, //Estado activo de la marca
                    HighSystem = b.HighSystem //Indica si la marca es de alto sistema
                })
                .ToListAsync(); //Convierte el resultado a una lista asíncrona

            return brands; //Devuelve la lista de marcas
        }

        // Método para obtener una marca por su ID ------------------------------------------------------------------------------------
        public async Task<BrandDTO> GetByIdAsync(int id)
        {
            //Busca la marca por su ID y verifica que no esté eliminada
            var brand = await _context.Brands
                .Select(b => new BrandDTO //Proyección a BrandDTO para devolver solo los datos necesarios
                {
                    Id = b.Id, 
                    Name = b.Name, 
                    LogoUrl = b.LogoUrl,
                    Active = b.Active, 
                    HighSystem = b.HighSystem 
                })
                .FirstOrDefaultAsync(); //Obtiene el primer resultado o null si no se encuentra

            //No validamos si la marca es null, ya que la valdación se realiza en el controlador
            return brand;
        }

        //Método para añadir una nueva marca a la base de datos ------------------------------------------------------------------------------------
        public async Task AddAsync(BrandCreateDTO brandCreateDTO)
        {
            //Cargar la imagen en el servidor y obtener la URL
            var urlImagen = await UploadImage(brandCreateDTO.File); //Llama al método UploadImage para cargar la imagen y obtener la URL

            //Crear una nueva instancia de Brand con los datos del DTO
            var brand = new Brand
            {
                Name = brandCreateDTO.Name, //Nombre de la marca
                LogoUrl = urlImagen, //URL del logo de la marca
            };

            await _context.Brands.AddAsync(brand); //Agrega la nueva marca al contexto de la base de datos (Se prepara)
            await _context.SaveChangesAsync();

            brandCreateDTO.Id = brand.Id; //Asigna el ID de la marca creada al DTO para su uso posterior 

        }


        // ------Método para cargar la imagen ---------------------------------------------------------------------------------------------------
        private async Task<string> UploadImage(IFormFile file)
        {
            ValidateFile(file);

            string _customPath = Path.Combine(Directory.GetCurrentDirectory(), _uploadSettings.UploadDirectory); //Define la ruta personalizada para guardar las imágenes
            //string _customPath = Path.Combine(_env.WebRootPath, _uploadSettings.UploadDirectory); //Automaticamente creaba un directorio en wwwroot/uploads

            if (!Directory.Exists(_customPath))   // Crear el directorio si no existe
            {
                Directory.CreateDirectory(_customPath);
            }

            // Generar el nombre único del archivo
            var fileName = Path.GetFileNameWithoutExtension(file.FileName)
                            + Guid.NewGuid().ToString()
                            + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(_customPath, fileName);

            // Guardar el archivo
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Retornar la ruta relativa o completa, según sea necesario
            return $"/{_uploadSettings.UploadDirectory}/{fileName}";
        }

        //Validar el archivo de imagen 
        private void ValidateFile(IFormFile file)
        {
            var permittedExtensions = _uploadSettings.AllowedExtensions.Split(',');
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!permittedExtensions.Contains(extension))
            {
                //throw new NotSupportedException("El tipo de archivo no es soportado.");
                //throw new NotSupportedException(Messages.Validation.UnSupportedFileType);
                throw new NotSupportedException(Messages.Validation.UnSupportedFileType);
            }
        }
    }
}
