using EcommerceREST.DTOs;
using EcommerceREST.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceREST.Controllers
{
    [Route("api/v1/brands")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }


        // GET: api/<BrandController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var brands = await _brandService.GetAllAsync();
            return Ok(brands);
        }

        // GET api/<BrandController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var brand = await _brandService.GetByIdAsync(id);

            if (brand == null)
            {
                return NotFound(new { message = "La marca no existe" });// Respuesta HTTP 404 Not Found con un mensaje
            }

            return Ok(brand);    // Retorna 200 OK con el producto encontrado.
        }

        // POST api/<BrandController>
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] BrandCreateDTO brandDTO)
        {
            try
            {
                await _brandService.AddAsync(brandDTO);
                return CreatedAtAction(nameof(GetById), new { id = brandDTO.Id }, brandDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Hubo un error al crear la marca: {ex.Message}" });
            }
        }

    }
}
