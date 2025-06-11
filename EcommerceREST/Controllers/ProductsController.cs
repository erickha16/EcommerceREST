using EcommerceREST.DTOs;
using EcommerceREST.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceREST.Controllers
{
    [Route("api/v1/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Obtiene todos los productos.
        /// </summary>
        /// <returns>Lista de productos.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();

            return Ok(products);        // Retorna 200 OK con la lista de productos.
        }

        /// <summary>
        /// Obtiene un producto por su ID.
        /// </summary>
        /// <param name="id">ID del producto.</param>
        /// <returns>Producto encontrado.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(new { message = "El producto no existe" });     // Respuesta HTTP 404 Not Found con un mensaje
            }


            return Ok(product);                                                 // Retorna 200 OK con el producto encontrado.
        }

        /// <summary>
        /// Crea un nuevo producto.
        /// </summary>
        /// <param name="productDTO">Datos del producto a crear.</param>
        /// <returns>Producto creado.</returns>
        /// <response code="201">Devuelve el nuevo producto creado.</response>
        /// <response code="400">Si los datos proporcionados no son válidos.</response>
        [HttpPost]
        public async Task<ActionResult<ProductDTO>> Create([FromBody] ProductCreateDTO productDTO)
        {
            // Con [ApiController] se hace la validación de modelo de manera automática.
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);  // Retorna 400

            try
            {
                await _productService.AddAsync(productDTO);

                return CreatedAtAction(nameof(GetById), new { id = productDTO.Id }, productDTO);    // Retorna 201 Created con la información del nuevo producto.
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Hubo un error al crear el producto: {ex.Message}" });   // Retorna 400 Bad Request con un mensaje de error.
            }
        }

        /// <summary>
        /// Actualiza un producto existente.
        /// </summary>
        /// <param name="id">ID del producto a actualizar.</param>
        /// <param name="productDTO">Datos actualizados del producto.</param>
        /// <returns>NoContent si la actualización es exitosa.</returns>
        /// <response code="204">Si la actualización es exitosa.</response>
        /// <response code="400">Si los datos proporcionados no son válidos o los ID no coinciden.</response>
        /// <response code="404">Si el producto no se encuentra.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductCreateDTO productDTO)
        {
            if (id != productDTO.Id)
            {
                return BadRequest(new { message = "El ID de la ruta no coincide con el ID del producto" }); // Respuesta HTTP 400 Bad Request con un mensaje.
            }

            // Con [ApiController] se hace la validación de modelo de manera automática.
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);  // Retorna 400
            //}

            var existingProduct = await _productService.GetByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound(new { message = "Producto no encontrado" });    // Respuesta HTTP 404 Not Found con un mensaje.
            }

            try
            {
                await _productService.UpdateAsync(productDTO);

                return NoContent();                                             // Retorna 204 No Content para indicar que la operación fue exitosa.
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Hubo un error al actualizar el producto: {ex.Message}" });  // Retorna 400 Bad Request con un mensaje de error.
            }
        }

        /// <summary>
        /// Elimina un producto por su ID.
        /// </summary>
        /// <param name="id">ID del producto a eliminar.</param>
        /// <returns>NoContent si la eliminación es exitosa.</returns>
        /// <response code="204">Si la eliminación es exitosa.</response>
        /// <response code="404">Si el producto no se encuentra.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existingProduct = await _productService.GetByIdAsync(id);
                if (existingProduct == null)
                {
                    return NotFound(new { message = "Producto no encontrado" });    // Respuesta HTTP 404 Not Found con un mensaje.
                }

                await _productService.DeleteAsync(id);

                return NoContent();                                                 // Retorna 204 No Content para indicar que la operación fue exitosa.
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        message = $"Error al eliminar el producto: {ex.Message}"
                    });   // Retorna 500 Internal Server Error con un mensaje de error.
            }
        }

    }
}
