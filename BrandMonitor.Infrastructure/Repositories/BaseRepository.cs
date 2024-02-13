using AutoMapper;
using BrandMonitor.Application.EntityDtoModels.CreateDtoModels;
using BrandMonitor.Application.EntityDtoModels.EntityDtoModels;
using BrandMonitor.Application.EntityDtoModels.UpdateDtoModels;
using BrandMonitor.Application.IRepositories;
using BrandMonitor.Infrastructure.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace BrandMonitor.Infrastructure.Repositories;

public abstract class BaseRepository<TEntity, TEntityDto, TCreateDto, TUpdateDto> : IRepository<TEntityDto, TCreateDto, TUpdateDto>
    where TEntity : BaseEntity
    where TEntityDto : BaseEntityDto
    where TCreateDto : BaseCreateDto
    where TUpdateDto : BaseUpdateDto
{
    protected readonly BrandMonitorContext Context;
    protected readonly IMapper Mapper;
    
    public BaseRepository(
        BrandMonitorContext context,
        IMapper mapper)
    {
        Context = context;
        Mapper = mapper;
    }
    
    private IQueryable<TEntity> GetQueryable()
    {
        return Context
            .Set<TEntity>()
            .AsNoTracking();
    }
    
    public virtual async Task<TEntityDto?> GetById(long id)
    {
        var query = GetQueryable();
        
        var entity = await query
            .FirstOrDefaultAsync(w => w.Id == id);

        return entity == null
            ? null
            : Mapper.Map<TEntity, TEntityDto>(entity);
    }
    
    public virtual async Task<List<TEntityDto>> GetAll()
    {
        var query = GetQueryable();

        var entities = await query.ToListAsync();

        return Mapper.Map<List<TEntity>, List<TEntityDto>>(entities);
    }
    
    public virtual async Task<TEntityDto> Create(TCreateDto entityDto)
    {
        var entity = Mapper.Map<TCreateDto, TEntity>(entityDto);

        Context.Add((object)entity);

        await Context.SaveChangesAsync();
        
        Context.ChangeTracker.Clear();

        return Mapper.Map<TEntity, TEntityDto>(entity);
    }
    
    public virtual async Task<TEntityDto> Update(TUpdateDto entityDto)
    {
        var entity = Mapper.Map<TUpdateDto, TEntity>(entityDto);

        Context.Set<TEntity>().Update(entity);
        await Context.SaveChangesAsync();
        
        Context.ChangeTracker.Clear();

        return Mapper.Map<TEntity, TEntityDto>(entity);
    }
}