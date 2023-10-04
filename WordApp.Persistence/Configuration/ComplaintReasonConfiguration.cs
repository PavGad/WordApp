using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WordApp.Persistence.Models;

namespace WordApp.Persistence.Configuration
{
    internal class ComplaintReasonConfiguration : IEntityTypeConfiguration<ComplaintReason>
    {
        public void Configure(EntityTypeBuilder<ComplaintReason> builder)
        {
            builder.ToTable("complaintReasons");
            //builder.HasData(
            //        new ComplaintReason() { Id = 1, Name = "Unacceptable content", Description = "unacceptable content" },
            //        new ComplaintReason() { Id = 2, Name = "Content mistakes", Description = "Content mistakes" }
            //    );
        }
    }
}
