using FamilyHubs.ServiceDirectory.Core.Entities.Abstract;
using FamilyHubs.SharedKernel;

namespace FamilyHubs.ServiceDirectory.Api.Helper
{
    public static class PaginationHelper
    {
        public static PaginatedList<T> PaginatedResults<T>(List<T> fullResults, IPaginationQuery paginationQuery)
        {
            if (paginationQuery.PageNumber.HasValue && paginationQuery.PageSize.HasValue)
            {
                var pageSize = paginationQuery.PageSize.Value;
                var pageNumber = paginationQuery.PageNumber.Value;
                var pageResults = fullResults.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList<T>();

                return new PaginatedList<T>(pageResults, fullResults.Count, pageNumber, pageSize);
            }

            return new PaginatedList<T>(fullResults, fullResults.Count, 1, fullResults.Count);
        }
    }
}
