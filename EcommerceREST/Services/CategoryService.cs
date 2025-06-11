using EcommerceREST.Data;
using EcommerceREST.DTOs;
using EcommerceREST.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceMVC.Services
{
    //Implementación del servicio de categorías
    public class CategoryService : ICategoryService //CategoryService implementa ICategoryService
    {
        // ------ Inyección de dependencias del contexto de la base de datos -------
        //La inyección de dependencias permite que el servicio pueda acceder a la base de datos sin necesidad de crear una
        //instancia del contexto directamente.
        private readonly ApplicationDbContext _context; //Contexto de la base de datos

        public CategoryService(ApplicationDbContext context) //Constructor que recibe el contexto de la base de datos
        {
            _context = context;
        }
        //------------------------------------------------------------------------

        //---------------- Implementación de sercicios (Métodos dede ICategoryService) ---------------- \\
        //---------Obtener todas las categorías de la base de datos (GetAllAsync) ---------\\
        public async Task<List<CategoryDTO>> GetAllAsync()
        {
            var categories = await _context.Categories //Obtiene todas las categorías de la base de datos (Se requeire el contesxto)
                .Where(c => !c.IsDeleted) //Filtra las categorías que no estén eliminadas (IsDeleted es una propiedad del modelo Category)
                .Select(c => new CategoryDTO //Proyección a CategoryDTO (es lo que pide la función) para devolver solo los datos necesarios
                {
                    Id = c.Id, //Id de la categoría
                    Name = c.Name, //Nombre de la categoría
                    Active = c.Active, //Estado activo de la categoría
                    HighSystem = c.HighSystem, //Indica si la categoría es de alto sistema
                })
                .ToListAsync(); //Convierte el resultado a una lista asíncrona

            return categories; //Devuelve la lista de categorías
        }

        //----------------- Obtener una categoría por su ID (GetByIdAsync) ---------------- \\
        public async Task<CategoryDTO> GetByIdAsync(int id)
        {
            var category = await _context.Categories //Obtiene la categoría por su ID desde la base de datos (Se requeire el contesxto)
                .Where(c => c.Id == id && !c.IsDeleted) //Filtra por ID y que no esté eliminada
                .Select(c => new CategoryDTO //Proyección a CategoryDTO para devolver solo los datos necesarios
                {
                    Id = c.Id, 
                    Name = c.Name, 
                    Active = c.Active, 
                    HighSystem = c.HighSystem, 
                })
                .FirstOrDefaultAsync(); //Devuelve el primer resultado o null si no se encuentra


            //No lo validamos, ya que la validación será en el controlador, para generar un código de estado 404 si no se encuentra la categoría.
            return category; 
        }


        //---------------- Añadir una nueva categoría ---------------- \\
        public async Task AddAsync(CategoryDTO categoryDTO)
        {
            var category = new Category //Crea una nueva instancia de Category (modelo de la base de datos)
            {
                Name = categoryDTO.Name, //Asigna el nombre de la categoría desde el DTO
            };

            await _context.Categories.AddAsync(category); //Agrega la nueva categoría al contexto de la base de datos (Se prepara)
            await _context.SaveChangesAsync(); //Guarda los cambios en la base de datos (Se ejecuta la inserción)
        }

        //-----------------------------------------------------------------------------------------------\\
        //Ejercicio extra:
        //4. Obtener la cantidad de productos no eliminados por categoría
        // Utilizar Count y public int ProductCount {get; set;}
        public async Task<List<CategoryDTO>> GetProductCountByCategoryAsync()
        {
            var categories = await _context.Categories //Obtiene todas las categorías de la base de datos (Se requeire el contesxto)
                .Where(c => c.Products.Any(p => !p.IsDeleted)) //Filtra las categorías que tienen al menos un producto no eliminado
                .Select(c => new CategoryDTO //Proyección a CategoryDTO para devolver solo los datos necesarios
                {
                    Id = c.Id, //Id de la categoría
                    Name = c.Name, //Nombre de la categoría
                    Active = c.Active, //Estado activo de la categoría
                    HighSystem = c.HighSystem, //Indica si la categoría es de alto sistema
                    ProductCount = c.Products.Count(p => !p.IsDeleted) //Cuenta los productos no eliminados en cada categoría
                })
                .ToListAsync(); //Convierte el resultado a una lista asíncrona
            return categories; //Devuelve la lista de categorías con su conteo de productos  
        }


    }
}

