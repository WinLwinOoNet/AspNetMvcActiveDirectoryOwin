using System.Data.Entity.ModelConfiguration;
using AspNetMvcActiveDirectoryOwin.Core.Domain;

namespace AspNetMvcActiveDirectoryOwin.Data.Mapping
{
    public class TraceLogMap : EntityTypeConfiguration<TraceLog>
    {
        public TraceLogMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Controller)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Action)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.PerformedBy)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TraceLog");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Controller).HasColumnName("Controller");
            this.Property(t => t.Action).HasColumnName("Action");
            this.Property(t => t.Message).HasColumnName("Message");
            this.Property(t => t.PerformedOn).HasColumnName("PerformedOn");
            this.Property(t => t.PerformedBy).HasColumnName("PerformedBy");
        }
    }
}
