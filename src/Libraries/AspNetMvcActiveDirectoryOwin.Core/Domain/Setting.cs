namespace AspNetMvcActiveDirectoryOwin.Core.Domain
{
    public partial class Setting : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public override int EntityId => Id;
    }
}
