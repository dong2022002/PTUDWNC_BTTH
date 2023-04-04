using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;

namespace TatBlog.Data.Mappings
{
	public class CommentMap : IEntityTypeConfiguration<Comment>
	{

		public void Configure(EntityTypeBuilder<Comment> builder)
		{
			builder.ToTable("Comments");

			builder.HasKey(c => c.Id);


			builder.Property(c => c.Name)
				.HasMaxLength(100)
				.IsRequired();

			builder.Property(p => p.Published)
				  .HasDefaultValue(false)
				  .IsRequired();

			builder.Property(p => p.DateComment)
				.HasColumnType("datetime");

			builder.Property(p => p.Description)
			   .HasMaxLength(5000)
			   .IsRequired();

			builder.HasOne(c => c.Post)
			  .WithMany(p => p.Comments)
			  .HasForeignKey(p => p.PostId)
			  .HasConstraintName("FK_Comments_Post")
			  .OnDelete(DeleteBehavior.Cascade);
		}
	}
}
