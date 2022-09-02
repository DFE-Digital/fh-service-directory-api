using FamilyHubs.SharedKernel;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.core.Interfaces.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fh_service_directory_api.core.Events;


public class UICacheCreatedEvent : DomainEventBase, IUICacheCreatedEvent
{
    public UICacheCreatedEvent(UICache item)
    {
        Item = item;
    }

    public UICache Item { get; }
}

