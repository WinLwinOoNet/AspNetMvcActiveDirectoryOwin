namespace AspNetMvcActiveDirectoryOwin.Core.Data
{
    public class UserPagedDataRequest : PagedDataRequest
    {
        public string RoleName { get; set; }
        public string LastName { get; set; }
        public bool? Active { get; set; }

        public UserSortField SortField { get; set; }
        public SortOrder SortOrder { get; set; }

        public UserPagedDataRequest()
        {
            SortOrder = SortOrder.Ascending;
            SortField = UserSortField.LastName;
        }
    }
    public enum UserSortField
    {
        UserName,
        FirstName,
        LastName,
        Initials,
        Active,
        CreatedOn,
        CreatedBy,
        ModifiedOn,
        ModifiedBy
    }
}