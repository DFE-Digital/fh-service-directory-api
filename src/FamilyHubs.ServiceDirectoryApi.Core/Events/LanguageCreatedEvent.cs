using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.SharedKernel;

namespace FamilyHubs.ServiceDirectory.Core.Events;

public class LanguageCreatedEvent : DomainEventBase
{
    public LanguageCreatedEvent(Language item)
    {
        Item = item;
    }

    public Language Item { get; }
}
