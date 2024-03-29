﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using FamilyHubs.ServiceDirectory.Data.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.ServiceDirectory.Shared.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Core.Queries.Taxonomies.GetTaxonomies;

public class GetTaxonomiesCommand : IRequest<PaginatedList<TaxonomyDto>>
{
    public GetTaxonomiesCommand(TaxonomyType taxonomyType, int? pageNumber, int? pageSize, string? text)
    {
        PageNumber = pageNumber ?? 1;
        PageSize = pageSize ?? int.MaxValue;
        TaxonomyType = taxonomyType;
        Text = text;
    }

    public int PageNumber { get; }
    public int PageSize { get; }
    public TaxonomyType TaxonomyType { get; }
    public string? Text { get; }
}

public class GetTaxonomiesCommandHandler : IRequestHandler<GetTaxonomiesCommand, PaginatedList<TaxonomyDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTaxonomiesCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<TaxonomyDto>> Handle(GetTaxonomiesCommand request, CancellationToken cancellationToken)
    {
        var filteredTaxonomies = _context.Taxonomies
            .Where(t => request.Text == null || t.Name.Contains(request.Text))
            .Where(t => request.TaxonomyType == TaxonomyType.NotSet || t.TaxonomyType == request.TaxonomyType)

            .AsNoTracking()

            .ProjectTo<TaxonomyDto>(_mapper.ConfigurationProvider)

            .AsQueryable();

        var result = await PaginatedList<TaxonomyDto>.CreateAsync(filteredTaxonomies, request.PageNumber, request.PageSize);

        return result;
    }
}