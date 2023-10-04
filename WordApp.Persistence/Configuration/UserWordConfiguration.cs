using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WordApp.Persistence.Models;

namespace WordApp.Persistence.Configuration
{
    internal class UserWordConfiguration : IEntityTypeConfiguration<UserWord>
    {
        public void Configure(EntityTypeBuilder<UserWord> builder)
        {
            builder.ToTable("userWords");
            //builder.HasOne(x => x.OriginalLanguage).WithMany(x => x.OriginalUserWords).HasForeignKey(x => x.OriginalLanguageId);
            //builder.HasOne(x => x.TargetLanguage).WithMany(x => x.TargetUserWords).HasForeignKey(x => x.TargetLanguageId);
        }
    }
}
