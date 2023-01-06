using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;
using fh_service_directory_api.core.Interfaces.Entities;
using System;
using System.Collections.Generic;

namespace fh_service_directory_api.core.Entities
{
    public  class OpenReferralContactLink : EntityBase<string>, IOpenReferralContactLink, IAggregateRoot
    {

        private OpenReferralContactLink() { }
        public OpenReferralContactLink(string id, string linkType, string linkId, string contactId)
        {
            Id = id;
            LinkType = linkType;
            LinkId = linkId;
            ContactId = contactId;
        }

        public string LinkType { get; set; } = default!;
        public string LinkId { get; set; } = default!;
        public string ContactId { get; set; } = default!;
    }
}
