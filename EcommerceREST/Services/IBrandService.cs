using EcommerceREST.DTOs;
using System.ComponentModel.DataAnnotations;

namespace EcommerceREST.Services
{
    public interface IBrandService
    {
        /// <summary>
        /// Obtiene una lista de todas las marcas.
        /// </summary>
        /// <returns></returns>
        Task<List<BrandDTO>> GetAllAsync();

        /// <summary>
        /// Obtiene una marca por su ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BrandDTO> GetByIdAsync(int id);

        /// <summary>
        /// Crea una nueva marca.
        /// </summary>
        /// <param name="brandCreateDTO"></param>
        /// <returns></returns>
        Task AddAsync(BrandCreateDTO brandCreateDTO);
    }
}
