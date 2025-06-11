using EcommerceREST.DTOs;

namespace EcommerceMVC.Services
{
    //Interfaz para el servicio de categorías
    public interface ICategoryService
    {
        // -------------------- Métodos que debe implementar el servicio de categorías -------------------- \\
        //Task indica que el método es asíncrono y devuelve un resultado en el futuro 
        //Las tres /// notas indican que es un comentario XML, que se utiliza para documentar el código y generar documentación automáticamente.

        /// <summary>
        /// Obtiene una lista de todas las categorías.
        /// </summary>
        /// <returns></returns>
        Task<List<CategoryDTO>> GetAllAsync();



        /// <summary>
        /// Obtiene una categoría por su ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<CategoryDTO> GetByIdAsync(int id);


        /// <summary>
        /// Inserta una catergoría nueva en la base de datos.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        Task AddAsync(CategoryDTO category);

        /// <summary>
        /// Obtiene la cantidad de productos no eliminados por categoría.
        /// </summary>
        /// <returns></returns>
        Task<List<CategoryDTO>> GetProductCountByCategoryAsync();

    }
}
