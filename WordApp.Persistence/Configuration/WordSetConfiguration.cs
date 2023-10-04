using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WordApp.Persistence.Models;

namespace WordApp.Persistence.Configuration
{
    internal class WordSetConfiguration : IEntityTypeConfiguration<WordSet>
    {
        public void Configure(EntityTypeBuilder<WordSet> builder)
        {
            builder.ToTable("wordSets");
            builder.HasOne(x => x.CreatedBy).WithMany(x => x.CreatedWordSets).HasForeignKey(x => x.CreatedById);
            builder.HasOne(x => x.ConfirmedBy).WithMany(x => x.ConfirmedWordSets).HasForeignKey(x => x.ConfirmedById);

            //builder.HasOne(x => x.OriginalLanguage).WithMany(x => x.OriginalWordSets).HasForeignKey(x => x.OriginalLanguageId);
            //builder.HasOne(x => x.TargetLanguage).WithMany(x => x.TargetWordSets).HasForeignKey(x => x.TargetLanguageId);
        }
    }
}
