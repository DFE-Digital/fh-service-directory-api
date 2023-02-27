namespace FamilyHubs.ServiceDirectory.Core.Entities.Abstract
{
    public interface IPaginationQuery
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}
