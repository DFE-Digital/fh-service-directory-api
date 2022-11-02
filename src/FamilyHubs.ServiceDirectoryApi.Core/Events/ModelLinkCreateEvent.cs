using FamilyHubs.SharedKernel;
using fh_service_directory_api.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fh_service_directory_api.core.Events;

public class ModelLinkCreateEvent : DomainEventBase
{
    public ModelLinkCreateEvent(ModelLink item)
    {
        Item = item;
    }

    public ModelLink Item { get; }
}
