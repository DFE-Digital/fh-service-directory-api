using fh_service_directory_api.core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fh_service_directory_api.infrastructure.Persistence.Config
{
    public class ContactLink : IEntityTypeConfiguration<OpenReferralLinkContact>
    {
        public void Configure(EntityTypeBuilder<OpenReferralLinkContact> builder)
        {
            builder.Property(t => t.LinkType)
                   .HasMaxLength(255)
                    .IsRequired();
            builder.Property(t => t.LinkId)
                   .IsRequired();
            builder.Property(t => t.Contact)
                   .IsRequired();
        }
    }
}
