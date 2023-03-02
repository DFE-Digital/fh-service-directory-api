using Ardalis.Specification;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.SharedKernel;
using IdGen;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.ServiceDirectory.Infrastructure.Services
{
    public abstract class BaseRepositoryService<T, TService> where T : EntityBase<string>
    {
        protected ILogger<TService> Logger { get; private set; }
        protected ApplicationDbContext DbContext { get; private set; }
        private readonly IIdGenerator<long> _idGenerator;
        private readonly DbSet<T> _dbSet;
        private readonly string _entityType;

        public BaseRepositoryService(ILogger<TService> logger, IIdGenerator<long> idGenerator, ApplicationDbContext context, DbSet<T> dbSet)
        {
            Logger = logger;
            _idGenerator = idGenerator;
            DbContext = context;
            _dbSet = dbSet;
            _entityType = typeof(T).Name;
        }

        public virtual async Task<T?> GetById(string id)
        {
            return await _dbSet.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<T> Upsert(T entity)
        {
            Logger.LogDebug($"Upsert {_entityType} - Begin");

            if (DbContext.Entry(entity).State != EntityState.Detached)
            {
                return await UpdateEntity(entity, entity);
            }

            if (string.IsNullOrEmpty(entity.Id))
            {
                entity.Id = GetNewId();
            }
            else
            {
                var existingEntity = await GetById(entity.Id);
                if (existingEntity != null)
                {
                    return await UpdateEntity(existingEntity, entity);
                }
            }

            return await AddEntity(entity);

        }

        protected string GetNewId()
        {
            return _idGenerator.CreateId().ToString();
        }

        protected virtual void UpdateEntityValues(T existing, T modified)
        {
            throw new NotImplementedException("UpdateEntityValues not implemented, this is requried for upsert funtionality");
        }

        private async Task<T> AddEntity(T entity)
        {
            Logger.LogDebug($"AddEntity {_entityType} - Begin");

            _dbSet.Add(entity);

            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var message = $"AddEntity {_entityType} - failed {ex.Message}";
                Logger.LogError("{message}{stacktrace}", message, ex.StackTrace);
                throw new Exception(message, ex);
            }

            var addedEntity = await GetById(entity.Id);

            Logger.LogInformation("AddEntity {entityType} - Success for : {id}", _entityType, entity.Id);
            return addedEntity!;
        }

        private async Task<T> UpdateEntity(T existing, T modified)
        {
            Logger.LogDebug($"Update {_entityType} - Begin");

            UpdateEntityValues(existing, modified);
            
            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var message = $"UpdateEntity {_entityType} - failed {ex.Message}";
                Logger.LogError("{message}{stacktrace}", message, ex.StackTrace);
                throw new Exception(message, ex);
            }

            Logger.LogInformation("UpdateEntity {entityType} - Success for : {id}", _entityType, existing.Id);
            return existing;
        }

    }
}
