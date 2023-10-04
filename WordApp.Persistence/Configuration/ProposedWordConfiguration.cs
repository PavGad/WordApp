using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WordApp.Persistence.Models;

namespace WordApp.Persistence.Configuration
{
    internal class ProposedWordConfiguration : IEntityTypeConfiguration<ProposedWord>
    {
        public void Configure(EntityTypeBuilder<ProposedWord> builder)
        {
            builder.ToTable("proposedWords");
        }
    }
}
