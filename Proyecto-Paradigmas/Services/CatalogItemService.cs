using Microsoft.EntityFrameworkCore;
using ProyectoParadigmas.Constants;
using ProyectoParadigmas.Database;
using ProyectoParadigmas.Dtos.Common;
using ProyectoParadigmas.Dtos.CatalogItems;
using ProyectoParadigmas.Entities;
using ProyectoParadigmas.Mappers;
using Persons.API.Constants;
using Proyecto_Paradigmas.Dtos.CatalogItems;

namespace ProyectoParadigmas.Services.CatalogItems
{
    public class CatalogItemService : ICatalogItemService
    {
        private readonly HotelDbContext _context;
        private readonly int PAGE_SIZE;
        private readonly int PAGE_SIZE_LIMIT;

        public CatalogItemService(HotelDbContext context, IConfiguration configuration)
        {
            _context = context;
            PAGE_SIZE = configuration.GetValue<int>("PageSize") == 0 ? 10 : configuration.GetValue<int>("PageSize");
            PAGE_SIZE_LIMIT = configuration.GetValue<int>("PageSizeLimit") == 0 ? 100 : configuration.GetValue<int>("PageSizeLimit");
        }

        public async Task<ResponseDto<PageDto<List<CatalogItemDto>>>> GetPageAsync(string searchTerm = "", int page = 1, int pageSize = 10)
        {
            page = Math.Abs(page);
            pageSize = Math.Abs(pageSize);
            pageSize = pageSize <= 0 ? PAGE_SIZE : pageSize;
            pageSize = pageSize > PAGE_SIZE_LIMIT ? PAGE_SIZE_LIMIT : pageSize;

            int startIndex = (page - 1) * pageSize;

            IQueryable<CatalogItemEntity> catalogQuery = _context.CatalogItems;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                catalogQuery = catalogQuery.Where(x => 
                    (x.Nombre + " " + x.Descripcion + " " + x.NumeroHabitacion).Contains(searchTerm));
            }

            int totalRows = await catalogQuery.CountAsync();

            var catalogEntities = await catalogQuery
                .OrderBy(x => x.Nombre)
                .Skip(startIndex)
                .Take(pageSize)
                .ToListAsync();

            return new ResponseDto<PageDto<List<CatalogItemDto>>>
            {
                StatusCode = HttpStatusCode.OK, 
                Status = true,
                Message = HttpMessageResponse.REGISTERS_FOUND,
                Data = new PageDto<List<CatalogItemDto>>
                {
                    CurrentPage = page == 0 ? 1 : page,
                    PageSize = pageSize,
                    TotalItems = totalRows,
                    TotalPages = (int)Math.Ceiling((double)totalRows / pageSize),
                    Items = CatalogItemMapper.ListEntityToListDto(catalogEntities),
                    HasNextPage = startIndex + pageSize < PAGE_SIZE_LIMIT && page < (int)Math.Ceiling((double)totalRows / pageSize),
                    HasPreviousPage = page > 1
                }
            };
        }

        public async Task<ResponseDto<CatalogItemDto>> GetOneByIdAsync(string id)
        {
            var catalogEntity = await _context.CatalogItems.FirstOrDefaultAsync(c => c.Id == id);

            if (catalogEntity is null)
            {
                return new ResponseDto<CatalogItemDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Message = HttpMessageResponse.REGISTER_NOT_FOUND,
                    Status = false
                };
            }

            return new ResponseDto<CatalogItemDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = HttpMessageResponse.REGISTER_FOUND,
                Status = true,
                Data = new CatalogItemDto
                {
                    Id = catalogEntity.Id,
                    Nombre = catalogEntity.Nombre,
                    Descripcion = catalogEntity.Descripcion,
                    Precio = catalogEntity.Precio,
                    ImagenUrl = catalogEntity.ImagenUrl,
                    Tipo = catalogEntity.Tipo,
                    CapacidadPersonas = catalogEntity.CapacidadPersonas,
                    NumeroHabitacion = catalogEntity.NumeroHabitacion,
                    Estado = catalogEntity.Estado
                }
            };
        }

        public async Task<ResponseDto<CatalogItemActionResponseDto>> CreateAsync(CatalogItemCreateDto dto)
        {
            var entity = CatalogItemMapper.CreateDtoToEntity(dto);

            entity.Id = Guid.NewGuid().ToString();
            
            _context.CatalogItems.Add(entity);
            await _context.SaveChangesAsync();

            return new ResponseDto<CatalogItemActionResponseDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = HttpMessageResponse.REGISTER_CREATED,
                Status = true,
                Data = new CatalogItemActionResponseDto { Id = entity.Id }
            };
        }

        public async Task<ResponseDto<CatalogItemActionResponseDto>> EditAsync(string id, CatalogItemEditDto dto)
        {
            var entity = await _context.CatalogItems.FirstOrDefaultAsync(c => c.Id == id);

            if (entity is null)
            {
                return new ResponseDto<CatalogItemActionResponseDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = HttpMessageResponse.REGISTER_NOT_FOUND
                };
            }

            var updatedEntity = CatalogItemMapper.EditDtoToEntity(entity, dto);
            _context.CatalogItems.Update(updatedEntity);
            await _context.SaveChangesAsync();

            return new ResponseDto<CatalogItemActionResponseDto>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = HttpMessageResponse.REGISTER_UPDATED,
                Data = new CatalogItemActionResponseDto { Id = id }
            };
        }

        public async Task<ResponseDto<CatalogItemActionResponseDto>> DeleteAsync(string id)
        {
            var entity = await _context.CatalogItems.FirstOrDefaultAsync(c => c.Id == id);

            if (entity is null)
            {
                return new ResponseDto<CatalogItemActionResponseDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = HttpMessageResponse.REGISTER_NOT_FOUND
                };
            }

            _context.CatalogItems.Remove(entity);
            await _context.SaveChangesAsync();

            return new ResponseDto<CatalogItemActionResponseDto>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = HttpMessageResponse.REGISTER_DELETED,
                Data = new CatalogItemActionResponseDto { Id = id }
            };
        }
    }
}