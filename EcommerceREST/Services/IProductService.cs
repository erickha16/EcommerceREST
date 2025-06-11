using EcommerceREST.DTOs;

namespace EcommerceREST.Services
{
    public interface IProductService
    {
        /// <summary>
        /// Obtiene todos los productos no eliminados.
        /// </summary>
        /// <returns></returns>
        Task<List<ProductDTO>> GetAllAsync();

        /// <summary>
        /// Obtiene un producto por su ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProductDTO> GetByIdAsync(int id);

        /// <summary>
        /// Crea un nuevo producto.
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        Task AddAsync(ProductCreateDTO productDto);

        /// <summary>
        /// Edita un producto existente.
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        Task UpdateAsync(ProductCreateDTO productDto);

        /// <summary>
        /// Elimina un producto por su ID (marca como eliminado).
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(int id);

        //Extras:
        // 1. Obtener productos activos y no eliminados, ordenados por precio (ascendente)
        Task<List<ProductDTO>> GetActiveProductsOrderedByPriceAsync();

        //2. Buscar productos no eliminados que contengan cierto texto en el nombre o descripción
        Task<List<ProductDTO>> SearchProductsByTextAsync(string searchText);

        // 3. Obtener los N productos más caros no eliminados
        Task<List<ProductDTO>> GetTopExpensiveProductsAsync(int count = 3);

        //4. Obtener la cantidad de productos no eliminados por categoría
        //este se encuentra en ICategoryService

        //5. Obtener productos no eliminados con precios por encima del promedio
        Task<List<ProductDTO>> GetProductsAboveAveragePriceAsync();
    }
}
