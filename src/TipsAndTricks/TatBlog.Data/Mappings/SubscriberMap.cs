using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;

namespace TatBlog.Data.Mappings
{
    public class SubscriberMap : IEntityTypeConfiguration<Subscriber>
    {
        public void Configure(EntityTypeBuilder<Subscriber> builder)
        {
            builder.ToTable("Subscriber");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Mail)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.DateRegis)
                 .HasColumnType("datetime");

            builder.Property(p => p.DateUnFollow)
               .HasColumnType("datetime");

            builder.Property(p => p.Desc)
              .HasMaxLength(5000);

            builder.Property(p => p.NoteAdmin)
             .HasMaxLength(5000);

            builder.Property(p => p.IsUserUnFollow)
               .HasDefaultValue(null);
        }
    }
}
