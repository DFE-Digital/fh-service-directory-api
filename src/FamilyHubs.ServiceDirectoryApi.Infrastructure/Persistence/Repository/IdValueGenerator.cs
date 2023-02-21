using FamilyHubs.SharedKernel;
using IdGen;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository
{
    public class IdValueGenerator : ValueGenerator<string>
    {
        private readonly IIdGenerator<long> _idGenerator;

        public override bool GeneratesTemporaryValues => false;

        public IdValueGenerator(IIdGenerator<long> idGenerator)
        {
            _idGenerator = idGenerator;
        }

        private string GetNewId(EntityEntry entry)
        {
            var entity = (EntityBase<string>)entry.Entity;

            if (string.IsNullOrEmpty(entity.Id))
            {
                return _idGenerator.CreateId().ToString();
            }

            return entity.Id;
            
        }

        public override string Next(EntityEntry entry) => GetNewId(entry);

        protected override object NextValue(EntityEntry entry) => GetNewId(entry);
    }
}
