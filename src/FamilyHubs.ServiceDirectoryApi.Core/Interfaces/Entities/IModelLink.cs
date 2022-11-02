namespace fh_service_directory_api.core.Interfaces.Entities;

public interface IModelLink : IEntityBase<string>
{
    string LinkType { get; set; }
    string ModelOneId { get; set; }
    string ModelTwoId { get; set; }
}
