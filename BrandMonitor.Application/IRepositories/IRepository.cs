using BrandMonitor.Application.EntityDtoModels.CreateDtoModels;
using BrandMonitor.Application.EntityDtoModels.EntityDtoModels;
using BrandMonitor.Application.EntityDtoModels.UpdateDtoModels;

namespace BrandMonitor.Application.IRepositories;

public interface IRepository<TEntityDto, TCreateDto, TUpdateDto>
    where TEntityDto : BaseEntityDto
    where TCreateDto : BaseCreateDto
    where TUpdateDto : BaseUpdateDto
{
    Task<TEntityDto?> GetById(long id);
    Task<List<TEntityDto>> GetAll();

    Task<TEntityDto> Create(TCreateDto entityDto);

    Task<TEntityDto> Update(TUpdateDto entityDto);
}