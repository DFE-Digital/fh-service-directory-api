using FamilyHubs.SharedKernel;

namespace FamilyHubs.ServiceDirectory.Core.Events;

public class OpenReferralEvent<T> : DomainEventBase
{
    public OpenReferralEvent(T item)
    {
        Item = item;
    }

    public T Item { get; }
}