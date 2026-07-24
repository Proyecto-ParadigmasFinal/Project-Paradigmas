using Proyecto_Paradigmas.Dtos.CatalogItems;
using ProyectoParadigmas.Dtos.CatalogItems;
using ProyectoParadigmas.Entities;

namespace ProyectoParadigmas.Mappers
{
    public static class CatalogItemMapper
    {
        public static CatalogItemEntity CreateDtoToEntity(CatalogItemCreateDto dto)
        {
            return new CatalogItemEntity
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Precio = dto.Precio,
                ImagenUrl = dto.ImagenUrl,
                Tipo = dto.Tipo,
                CapacidadPersonas = dto.CapacidadPersonas,
                NumeroHabitacion = dto.NumeroHabitacion,
                Estado = dto.Estado
            };
        }

        public static CatalogItemEntity EditDtoToEntity(CatalogItemEntity entity, CatalogItemEditDto dto)
        {
            entity.Nombre = dto.Nombre;
            entity.Descripcion = dto.Descripcion;
            entity.Precio = dto.Precio;
            entity.ImagenUrl = dto.ImagenUrl;
            entity.Tipo = dto.Tipo;
            entity.CapacidadPersonas = dto.CapacidadPersonas;
            entity.NumeroHabitacion = dto.NumeroHabitacion;
            entity.Estado = dto.Estado;

            return entity;
        }

        public static List<CatalogItemDto> ListEntityToListDto(List<CatalogItemEntity> entities)
        {
            return entities.Select(e => new CatalogItemDto
            {
                Id = e.Id,
                Nombre = e.Nombre,
                Descripcion = e.Descripcion,
                Precio = e.Precio,
                ImagenUrl = e.ImagenUrl,
                Tipo = e.Tipo,
                CapacidadPersonas = e.CapacidadPersonas,
                NumeroHabitacion = e.NumeroHabitacion,
                Estado = e.Estado
            }).ToList();
        }
    }
}