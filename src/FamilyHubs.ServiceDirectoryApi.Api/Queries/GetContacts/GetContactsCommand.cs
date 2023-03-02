using AutoMapper;
using FamilyHubs.ServiceDirectory.Api.Helper;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Services;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.SharedKernel;
using MediatR;

namespace FamilyHubs.ServiceDirectory.Api.Queries.GetContacts
{
    public class GetContactsCommand : IRequest<Result<PaginatedList<ContactDto>>>
    {
        public GetContactsCommand(
            string? id, string? title, string? name, string? telephone, string? textPhone, string? url, string? email, int? pageNumber, int? pageSize)
        {
            QueryValues = new ContactQuery
            { 
                Id = id, 
                Name = name,
                Title= title,
                Telephone = telephone,
                TextPhone = textPhone,
                Url = url,
                Email = email,
                PageNumber = pageNumber, 
                PageSize =pageSize
            };
        }

        public ContactQuery QueryValues { get; }
    }

    public class GetContactsCommandHandler : IRequestHandler<GetContactsCommand, Result<PaginatedList<ContactDto>>>
    {
        private readonly IContactService _contactsService;
        private readonly IMapper _mapper;

        public GetContactsCommandHandler(IContactService contactsService, IMapper mapper)
        {
            _contactsService = contactsService;
            _mapper = mapper;
        }

        public async Task<Result<PaginatedList<ContactDto>>> Handle(GetContactsCommand request, CancellationToken cancellationToken)
        {
            var contacts = await _contactsService.GetContacts(request.QueryValues);
            var contactsDto = _mapper.Map<List<ContactDto>>(contacts);
            var paginatedList = PaginationHelper.PaginatedResults(contactsDto, request.QueryValues);

            return Result<PaginatedList<ContactDto>>.Success(paginatedList);
        }
    }
}
