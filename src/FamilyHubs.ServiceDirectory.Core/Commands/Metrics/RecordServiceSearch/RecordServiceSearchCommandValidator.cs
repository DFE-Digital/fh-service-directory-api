using FamilyHubs.ServiceDirectory.Core.Commands.Metrics.RecordServiceSearch;
using FluentValidation;

namespace FamilyHubs.ServiceDirectory.Core;

public class RecordServiceSearchCommandValidator : AbstractValidator<RecordServiceSearchCommand>
{
    public RecordServiceSearchCommandValidator()
    {
        RuleFor(v => v.ServiceSearch)
            .NotNull();
        
        RuleFor(v => v.ServiceSearch.RequestTimestamp)
            .NotEmpty()
            // Requests must be made either within a 60 second window, or...
            .GreaterThan(DateTime.UtcNow.AddMinutes(-1))
            // up until now. No future requests allowed.
            .LessThan(DateTime.UtcNow);
        
        RuleFor(v => v.ServiceSearch.SearchTriggerEventId)
            .NotEmpty();
        
        RuleFor(v => v.ServiceSearch.Id)
            .Empty();

    }
}
