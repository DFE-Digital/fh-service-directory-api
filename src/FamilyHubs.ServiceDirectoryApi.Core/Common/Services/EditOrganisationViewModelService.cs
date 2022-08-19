using fh_service_directory_api.core.OrganisationAggregate.Entities;

namespace fh_service_directory_api.core.Common.Services;

public class EditOrOrganisationViewModelService
{
    private readonly IRepository<Organisation> _OrOrganisationRepository;

    public EditOrOrganisationViewModelService
    (
        IRepository<Organisation> OrOrganisationRepository
    //IRepository<CatalogItem> itemRepository,
    //IUriComposer uriComposer,
    //IBasketQueryService basketQueryService
    )
    {
        _OrOrganisationRepository = OrOrganisationRepository;
        //_uriComposer = uriComposer;
        //_basketQueryService = basketQueryService;
        //_itemRepository = itemRepository;
    }
}
