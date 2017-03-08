using System.Data.Entity.ModelConfiguration;
using AspNetMvcActiveDirectoryOwin.Core.Domain;

namespace AspNetMvcActiveDirectoryOwin.Data.Mapping
{
    public class LogMap : EntityTypeConfiguration<Log>
    {
        public LogMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Username)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Thread)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.Level)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Logger)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.Message)
                .IsRequired()
                .HasMaxLength(4000);

            this.Property(t => t.Exception)
                .HasMaxLength(2000);

            // Table & Column Mappings
            this.ToTable("Log");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Date).HasColumnName("Date");
            this.Property(t => t.Username).HasColumnName("Username");
            this.Property(t => t.Thread).HasColumnName("Thread");
            this.Property(t => t.Level).HasColumnName("Level");
            this.Property(t => t.Logger).HasColumnName("Logger");
            this.Property(t => t.Message).HasColumnName("Message");
            this.Property(t => t.Exception).HasColumnName("Exception");
        }
    }
}
