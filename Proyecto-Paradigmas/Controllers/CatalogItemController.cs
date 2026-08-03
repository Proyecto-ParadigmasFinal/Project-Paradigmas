using Microsoft.AspNetCore.Mvc;
using Proyecto_Paradigmas.Dtos.CatalogItems;
using ProyectoParadigmas.Dtos.CatalogItems;
using ProyectoParadigmas.Services.CatalogItems;
using Microsoft.AspNetCore.Authorization;

namespace ProyectoParadigmas.Controllers
{
    [Route("api/catalog")]
    [ApiController]
    public class CatalogItemController : ControllerBase
    {
        private readonly ICatalogItemService _catalogItemService;

        public CatalogItemController(ICatalogItemService catalogItemService)
        {
            _catalogItemService = catalogItemService;
        }

        [HttpGet]
        public async Task<ActionResult> GetPage(string searchTerm = "", int page = 1, int pageSize = 10)
        {
            var response = await _catalogItemService.GetPageAsync(searchTerm, page, pageSize);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetOne(string id)
        {
            var result = await _catalogItemService.GetOneByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<ActionResult> Create(CatalogItemCreateDto dto)
        {
            var result = await _catalogItemService.CreateAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, CatalogItemEditDto dto)
        {
            var result = await _catalogItemService.EditAsync(id, dto);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "Administrador")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var result = await _catalogItemService.DeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}