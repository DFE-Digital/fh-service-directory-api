using fh_service_directory_api.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fh_service_directory_api.core.Interfaces.Entities
{
    public  interface IOpenReferralLinkContact : IEntityBase<string>
    {
         string LinkType { get; set; } 
         string LinkId { get; set; }
        OpenReferralContact Contact { get; set; }
    }
}
