using EcommerceMVC.Services;
using EcommerceREST.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceREST.Controllers
{
    [Route("api/v1/categories")]
    [ApiController] //Ayuda a las validaciones de los modelos y a la generación de documentación de la API
    public class CategoriesController : ControllerBase
    {
        /*    ------------------------------------    Inyección de dependencias ------------------------------------                                   */
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /*    ------------------------------------    Endpoints de la API RESTful para categorías ------------------------------------                  */

        //Obetner todas las categorías
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories); // Devuelve un código 200 OK con la lista de categorías
        }

        //obtener una categoría por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound(new {message = "No se encontró la categoría" }); // Devuelve un código 404 Not Found si no se encuentra la categoría
            }
            return Ok(category); // Devuelve un código 200 OK con la categoría encontrada
        }

        //Insertar una nueva categoría
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryDTO categoryDTO)
        {
            try
            {
                await _categoryService.AddAsync(categoryDTO);
                return CreatedAtAction(nameof(GetById), new
                {  //EL nameof(GetById) hace referencia al método GetById de arriba
                    id = categoryDTO.Id 
                }, categoryDTO); // Devuelve un código 201 Created con la categoría creada (Retorna el objeto recién creado)
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = $"Error al crear la categoría: {ex.Message}"}); // Devuelve un código 400 Bad Request si hay un error al crear la categoría
            }
        }


    }
}
