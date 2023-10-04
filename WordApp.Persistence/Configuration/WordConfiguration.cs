using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WordApp.Persistence.Models;

namespace WordApp.Persistence.Configuration
{
    internal class WordConfiguration : IEntityTypeConfiguration<Word>
    {
        public void Configure(EntityTypeBuilder<Word> builder)
        {
            builder.ToTable("words");
            //builder.HasOne(x => x.OriginalLanguage).WithMany(x => x.OriginalWords).HasForeignKey(x => x.OriginalLanguageId);
            //builder.HasOne(x => x.TargetLanguage).WithMany(x => x.TargetWords).HasForeignKey(x => x.TargetLanguageId);
        }
    }
}
