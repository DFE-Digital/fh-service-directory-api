//using Ardalis.ApiEndpoints;
//using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralOrganisations;
//using FamilyHubs.SharedKernel.Interfaces;
//using Microsoft.AspNetCore.Mvc;
//using Swashbuckle.AspNetCore.Annotations;

//namespace FamilyHubs.ServiceDirectoryApi.Api.Endpoints.OpenReferralOrganisations;
//public class Create : EndpointBaseAsync
//  .WithRequest<CreateOrganisationRequest>
//  .WithActionResult<CreateOrganisationResponse>
//{
//    private readonly IRepository<OpenReferralOrganisation> _repository;

//    public Create(IRepository<OpenReferralOrganisation> repository)
//    {
//        _repository = repository;
//    }

//    [HttpPost("api/organizations")]
//    [SwaggerOperation(
//      Summary = "Creates a new Organisation",
//      Description = "Creates a new Organisation",
//      OperationId = "Organisation.Create",
//      Tags = new[] { "OrganisationEndpoints" })
//    ]
//    public override async Task<ActionResult<CreateOrganisationResponse>> HandleAsync(
//      CreateOrganisationRequest request,
//      CancellationToken cancellationToken = new())
//    {
//        if (request.Name == null)
//        {
//            return BadRequest();
//        }

//        var newOrganisation = new OpenReferralOrganisation(request.Name);
//        var createdItem = await _repository.AddAsync(newOrganisation, cancellationToken);
//        var response = new CreateOrganisationResponse
//        (
//          id: 1,
//          name: createdItem.Name
//        );

//        return Ok(response);
//    }
//}


////public class Create : EndpointBaseAsync
////  .WithRequest<CreateProjectRequest>
////  .WithActionResult<CreateProjectResponse>
////{
////    private readonly IRepository<Project> _repository;

////    public Create(IRepository<Project> repository)
////    {
////        _repository = repository;
////    }

////    [HttpPost("/Projects")]
////    [SwaggerOperation(
////      Summary = "Creates a new Project",
////      Description = "Creates a new Project",
////      OperationId = "Project.Create",
////      Tags = new[] { "ProjectEndpoints" })
////    ]
////    public override async Task<ActionResult<CreateProjectResponse>> HandleAsync(
////      CreateProjectRequest request,
////      CancellationToken cancellationToken = new())
////    {
////        if (request.Name == null)
////        {
////            return BadRequest();
////        }

////        var newProject = new Project(request.Name, PriorityStatus.Backlog);
////        var createdItem = await _repository.AddAsync(newProject, cancellationToken);
////        var response = new CreateProjectResponse
////        (
////          id: createdItem.Id,
////          name: createdItem.Name
////        );

////        return Ok(response);
////    }
////}
