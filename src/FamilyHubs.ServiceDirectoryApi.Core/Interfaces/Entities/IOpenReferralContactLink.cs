using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fh_service_directory_api.core.Interfaces.Entities
{
    public  interface IOpenReferralContactLink : IEntityBase<string>
    {
         string LinkType { get; set; } 
         string LinkId { get; set; } 
         string ContactId { get; set; } 
    }
}
