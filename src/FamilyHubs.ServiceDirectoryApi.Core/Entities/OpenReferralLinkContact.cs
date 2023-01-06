using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;
using fh_service_directory_api.core.Interfaces.Entities;
using System;
using System.Collections.Generic;

namespace fh_service_directory_api.core.Entities
{
    public  class OpenReferralLinkContact : EntityBase<string>, IOpenReferralLinkContact, IAggregateRoot
    {

        private OpenReferralLinkContact() { }
        public OpenReferralLinkContact(string id, string linkType, string linkId, OpenReferralContact contact)
        {
            Id = id;
            LinkType = linkType;
            LinkId = linkId;
            Contact = contact;
        }

        public string LinkType { get; set; } = default!;
        public string LinkId { get; set; } = default!;
        public OpenReferralContact Contact { get; set; }
    }
}
