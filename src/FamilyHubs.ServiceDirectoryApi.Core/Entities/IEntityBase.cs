namespace FamilyHubs.ServiceDirectoryApi.Core.Entities;
public interface IEntityBase<Tid>
{
    Tid Id { get; set; }
}
