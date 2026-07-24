using ProyectoParadigmas.Dtos.Common;
using ProyectoParadigmas.Dtos.CatalogItems;
using Proyecto_Paradigmas.Dtos.CatalogItems;

namespace ProyectoParadigmas.Services.CatalogItems
{
    public interface ICatalogItemService
    {
        Task<ResponseDto<PageDto<List<CatalogItemDto>>>> GetPageAsync(string searchTerm = "", int page = 1, int pageSize = 10);
        Task<ResponseDto<CatalogItemDto>> GetOneByIdAsync(string id);
        Task<ResponseDto<CatalogItemActionResponseDto>> CreateAsync(CatalogItemCreateDto dto);
        Task<ResponseDto<CatalogItemActionResponseDto>> EditAsync(string id, CatalogItemEditDto dto);
        Task<ResponseDto<CatalogItemActionResponseDto>> DeleteAsync(string id);
    }
}