using FamilyHubs.SharedKernel;

namespace fh_service_directory_api.core.Events;

public class OpenReferralEvent<T> : DomainEventBase
{
    public OpenReferralEvent(T item)
    {
        Item = item;
    }

    public T Item { get; }
}