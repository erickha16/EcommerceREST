using EcommerceREST.Data;
using EcommerceREST.DTOs;
using EcommerceREST.Models;
using EcommerceREST.Constants;
using Microsoft.EntityFrameworkCore;

namespace EcommerceREST.Services
{
    public class ProductService : IProductService
    {
        // ------ Inyección de dependencias del contexto de la base de datos -------
        //La inyección de dependencias permite que el servicio pueda acceder a la base de datos sin necesidad de crear una
        //instancia del contexto directamente.
        private readonly ApplicationDbContext _context; //Contexto de la base de datos
        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }
        //------------------------------------------------------------------------

        //---------------- Implementación de sercicios (Métodos dede IProductService) ---------------- \\
        // Método para obtener la lista de productos --------------------------------------------------------------------------------------------
        public async Task<List<ProductDTO>> GetAllAsync()
        {
            var products = await _context.Products //Obtiene todos los productos de la base de datos (Se requeire el contesxto)
                .Where(p => !p.IsDeleted) //Filtra los productos que no estén eliminados (IsDeleted es una propiedad del modelo Product)
                .Select(p => new ProductDTO //Proyección a ProductDTO (es lo que pide la función) para devolver solo los datos necesarios
                {
                    Id = p.Id, //Id del producto
                    Name = p.Name, //Nombre del producto
                    Price = p.Price, //Precio del producto
                    Description = p.Description, //Descripción del producto
                    CategoryId = p.CategoryId, //Id de la categoría del producto
                    Category = p.Category.Name, //Trae el nombre de la categoria
                    BrandId = p.BrandId, //Id de la marca del producto
                    Brand = p.Brand.Name, // Trae ek nombre de la marca
                    HighSystem = p.HighSystem, //Indica si el producto es de alto sistema
                    Active = p.Active, //Estado activo del producto
                    IsDeleted = p.IsDeleted, //Indica si el producto está eliminado

                })
                .ToListAsync(); //Convierte el resultado a una lista asíncrona
            return products; //Devuelve la lista de productos
        }

        // Método para obtener un producto por su Id--------------------------------------------------------------------------------------------
        public async Task<ProductDTO> GetByIdAsync(int id)
        {
            var product = await _context.Products //Obtiene el producto por Id de la base de datos (Se requeire el contesxto)                
                .Select(p => new ProductDTO //Proyección a ProductDTO (es lo que pide la función) para devolver solo los datos necesarios
                {
                    Id = p.Id, //Id del producto
                    Name = p.Name, //Nombre del producto
                    Price = p.Price, //Precio del producto
                    Description = p.Description, //Descripción del producto
                    CategoryId = p.CategoryId, //Id de la categoría del producto
                    BrandId = p.BrandId, //Id de la marca del producto
                    HighSystem = p.HighSystem, //Indica si el producto es de alto sistema
                    Active = p.Active, //Estado activo del producto
                    IsDeleted = p.IsDeleted, //Indica si el producto está eliminado
                    // Aquí se pueden agregar más propiedades del DTO según sea necesario
                    Category = p.Category.Name, //Nombre de la categoría del producto (asumiendo que Category es una propiedad de navegación)
                    Brand = p.Brand.Name //Nombre de la marca del producto (asumiendo que Brand es una propiedad de navegación)

                })
                .FirstOrDefaultAsync(p => p.Id == id); //Obtiene el primer resultado que cumple el la condición o null si no existe

            if (product == null)
            {
                throw new ApplicationException(string.Format(Messages.Error.ProductNotFoundWithId, id));
            }
            return product; //Devuelve el producto encontrado o null si no existe
        }

        // Método para agregar un nuevo producto--------------------------------------------------------------------------------------------
        public async Task AddAsync(ProductCreateDTO productDTO)
        {
            var product = new Product //Crea una nueva instancia de Product (modelo de la base de datos)
            {
                Name = productDTO.Name, //Asigna el nombre del producto desde el DTO
                Price = productDTO.Price, //Asigna el precio del producto desde el DTO
                Description = productDTO.Description, //Asigna la descripción del producto desde el DTO
                CategoryId = productDTO.CategoryId, //Asigna el Id de la categoría del producto desde el DTO
                BrandId = productDTO.BrandId, //Asigna el Id de la marca del producto desde el DTO
            };

            await _context.Products.AddAsync(product); //Agrega el nuevo producto al contexto de la base de datos (Se prepara)
            await _context.SaveChangesAsync(); //Guarda los cambios en la base de datos (Se ejecuta la inserción)
            //Una vezue el producto se ha guardado, podemos asignar el Id generado por la base de datos al DTO

            productDTO.Id = product.Id; //Asigna el Id del producto creado al DTO para que se pueda usar posteriormente
        }

        // Método para actualizar un producto existente--------------------------------------------------------------------------------------------
        public async Task UpdateAsync(ProductCreateDTO productDTO)
        {
            var product = await _context.Products.FindAsync(productDTO.Id); //Busca el producto por Id en la base de datos(mejor rendimiento)
            if (product == null)
            {
                throw new ApplicationException(string.Format(Messages.Error.ProductNotFoundWithId, productDTO.Id));
            }
            //Actualiza las propiedades del producto con los valores del DTO (Los campos que reremos reemplazar)
            product.Name = productDTO.Name;
            product.Price = productDTO.Price;
            product.Description = productDTO.Description;
            product.CategoryId = productDTO.CategoryId;
            product.BrandId = productDTO.BrandId;
            product.Active = productDTO.Active;

            _context.Products.Update(product); //Marca el producto como modificado en el contexto (.Update es de Entity Framework core)
            await _context.SaveChangesAsync(); //Guarda los cambios en la base de datos

        }

        //Método para eliminar un producto existente--------------------------------------------------------------------------------------------
        public async Task DeleteAsync(int id) {
            var product = await _context.Products.FindAsync(id); //Busca el producto por Id en la base de datos(mejor rendimiento)

            if (product == null || product.IsDeleted) //Verificamos si el producto existe
            {
                throw new ApplicationException(string.Format(Messages.Error.ProductNotFoundWithId, id));
            }

            //Cambiamos el estado del producto a eliminado (en lugar de eliminarlo físicamente)
            product.IsDeleted = true; //Marcamos el producto como eliminado

            _context.Products.Update(product); //Marca el producto como modificado en el contexto
            await _context.SaveChangesAsync();

            //Si se desea eliminar físicamente el producto, se puede usar el siguiente código en su lugar:
            // _context.Products.Remove(product); //Elimina el producto del contexto (esto lo elimina físicamente de la base de datos)

        }


        //----------------------------------------------- Ejercicios extra: ----------------------------------------------- \\
        // 1. Obtener productos activos y no eliminados, ordenados por precio (ascendente)
        // OrderBy
        public async Task<List<ProductDTO>> GetActiveProductsOrderedByPriceAsync()
        {
            var products = await _context.Products //Obtiene todos los productos de la base de datos (Se requeire el contesxto)
                 .Where(p => p.Active && !p.IsDeleted) //Filtra los productos que no estén eliminados (IsDeleted es una propiedad del modelo Product)
                 .OrderBy(p => p.Price)
                 .Select(p => new ProductDTO //Proyección a ProductDTO (es lo que pide la función) para devolver solo los datos necesarios
                 {
                     Id = p.Id, //Id del producto
                     Name = p.Name, //Nombre del producto
                     Price = p.Price, //Precio del producto
                     Description = p.Description, //Descripción del producto
                     CategoryId = p.CategoryId, //Id de la categoría del producto
                     Category = p.Category.Name, //Trae el nombre de la categoria
                     BrandId = p.BrandId, //Id de la marca del producto
                     Brand = p.Brand.Name, // Trae ek nombre de la marca
                     HighSystem = p.HighSystem, //Indica si el producto es de alto sistema
                     Active = p.Active, //Estado activo del producto
                     IsDeleted = p.IsDeleted, //Indica si el producto está eliminado

                 })
                 .ToListAsync(); 
            return products; //Devuelve la lista de productos
        }

        // 2. Buscar productos no eliminados que contengan cierto texto en el nombre o descripción
        //Se debe usar Contains 
        public async Task<List<ProductDTO>> SearchProductsByTextAsync(string searchText)
        {
            string searchLower = searchText.ToLower();// Convertir la cadena en minusculas

            var products = await _context.Products //Obtiene todos los productos de la base de datos (Se requeire el contesxto)
                 .Where(p => !p.IsDeleted && ((p.Name.ToLower().Contains(searchLower)) || (p.Description.ToLower().Contains(searchLower)))) //Filtra los productos que no estén eliminados (IsDeleted es una propiedad del modelo Product)
                 .Select(p => new ProductDTO //Proyección a ProductDTO (es lo que pide la función) para devolver solo los datos necesarios
                 {
                     Id = p.Id, //Id del producto
                     Name = p.Name, //Nombre del producto
                     Price = p.Price, //Precio del producto
                     Description = p.Description, //Descripción del producto
                     CategoryId = p.CategoryId, //Id de la categoría del producto
                     Category = p.Category.Name, //Trae el nombre de la categoria
                     BrandId = p.BrandId, //Id de la marca del producto
                     Brand = p.Brand.Name, // Trae ek nombre de la marca
                     HighSystem = p.HighSystem, //Indica si el producto es de alto sistema
                     Active = p.Active, //Estado activo del producto
                     IsDeleted = p.IsDeleted, //Indica si el producto está eliminado

                 })
                 .ToListAsync();
            return products; //Devuelve la lista de productos
        }

        //3. Obtener los N productos más caros no eliminados
        // Utilizando OrderByDescending y Take
        public async Task<List<ProductDTO>> GetTopExpensiveProductsAsync(int count)
        {
            var products = await _context.Products //Obtiene todos los productos de la base de datos (Se requeire el contesxto)
                 .Where(p => !p.IsDeleted) //Filtra los productos que no estén eliminados (IsDeleted es una propiedad del modelo Product)
                 .OrderByDescending(p => p.Price) //Ordena los productos por precio de forma descendente
                 .Take(count) //Toma los primeros N productos
                 .Select(p => new ProductDTO //Proyección a ProductDTO 
                 {
                     Id = p.Id, //Id del producto
                     Name = p.Name, //Nombre del producto
                     Price = p.Price, //Precio del producto
                     Description = p.Description, //Descripción del producto
                     CategoryId = p.CategoryId, //Id de la categoría del producto
                     Category = p.Category.Name, //Trae el nombre de la categoria
                     BrandId = p.BrandId, //Id de la marca del producto
                     Brand = p.Brand.Name, // Trae ek nombre de la marca
                     HighSystem = p.HighSystem, //Indica si el producto es de alto sistema
                     Active = p.Active, //Estado activo del producto
                     IsDeleted = p.IsDeleted, //Indica si el producto está eliminado
                 })
                 .ToListAsync();

            return products; //Devuelve la lista de productos
        }

        //5. Obtener productos no eliminados con precios por encima del promedio. Utilizar AverageAsync
        public async Task<List<ProductDTO>> GetProductsAboveAveragePriceAsync()
        {
            // Primero calculamos el precio promedio de todos los productos
            var averagePrice = await _context.Products
                .Where(p => !p.IsDeleted) // Filtramos los productos que no están eliminados
                .AverageAsync(p => p.Price); // Calculamos el precio promedio

            // Luego obtenemos los productos con precio mayor al promedio
            var products = await _context.Products //Obtiene todos los productos de la base de datos (Se requeire el contesxto)
                .Where(p => p.Price > averagePrice && !p.IsDeleted) //Filtra los productos que no estén eliminados y cuyo precio sea mayor al promedio
                .Select(p => new ProductDTO //Proyección a ProductDTO (es lo que pide la función) para devolver solo los datos necesarios
                {
                    Id = p.Id, //Id del producto
                    Name = p.Name, //Nombre del producto
                    Price = p.Price, //Precio del producto
                    Description = p.Description, //Descripción del producto
                    CategoryId = p.CategoryId, //Id de la categoría del producto
                    Category = p.Category.Name, //Trae el nombre de la categoria
                    BrandId = p.BrandId, //Id de la marca del producto
                    Brand = p.Brand.Name, // Trae ek nombre de la marca
                    HighSystem = p.HighSystem, //Indica si el producto es de alto sistema
                    Active = p.Active, //Estado activo del producto
                    IsDeleted = p.IsDeleted, //Indica si el producto está eliminado
                })
                .ToListAsync(); //Convierte el resultado a una lista asíncrona

            return products;
        }

    }

}
