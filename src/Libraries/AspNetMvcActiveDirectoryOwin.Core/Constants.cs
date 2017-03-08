namespace AspNetMvcActiveDirectoryOwin.Core
{
    public class Constants
    {
        public static class Areas
        {
            public const string Administration = "Administration";
        }

        public static class EmailTemplates
        {
            public const string AddNewUserNotification = "Add New User Notification";
        }

        public static class MainPages
        {
            public const string Home = "ASP.NET MVC Active Directory OWIN";
            public const string Sample = "Sample";

            // System pages
            public const string Dashboard = "Dashboard";
            public const string EmailTemplates = "Email Templates";
            public const string EmailTemplateEdit = "Email Template Edit";
            public const string Settings = "Settings";
            public const string SettingEdit = "Edit Setting";
            public const string Login = "Login";
            public const string Logs = "Application Logs";
            public const string TraceLogs = "Trace Logs";
            public const string Users = "Users";
            public const string UserCreate = "Add New User";
            public const string UserEdit = "Edit User";
            public const string ReleaseHistory = "Release History";
            public const string PageNotFound = "Page Not Found";
            public const string Error = "Error";
            public const string AntiForgery = "Oops!";
            public const string AccessDenied = "Access Denied";
            public const string AdAccountNotFound = "AD Account Not Found";
        }

        public static class Messages
        {
            public const string Error = "An error occurred while processing your request. " +
                                        "If these issue persists, then please contact customer service.";

            public const string PageNotFound = "Sorry, the page you're looking for cannot be found. " +
                                               "If these issue persists, then please contact customer service.";

            public const string InvalidData = "One or more fields contain invalid data. Please fix and submit again. " +
                                              "If these issue persists, then please contact customer service.";

            public const string NotAuthorized = "You does not have access to Application. Please contact your manager.";

            public const string AccessDenied = "You do not have permission to perform the selected operation. Please contact your manager.";

            public const string AdAccountNotFound = "Your AD Account is not found. Please contact your manager.";
        }

        public static class RoleNames
        {
            public const string Developer = "Developer";
            public const string ApplicationManager = "Application Manager";
            public const string User = "User";
        }

        public static class Settings
        {
            public const string LogClearDays = "log.clear.days";
            public const string EmailSend = "email.send";
            public static string EmailAddresses = "email.to.addresses";
        }

        /// <summary>
        /// Cache time in minutes.
        /// </summary>
        public static class CacheTimes
        {
            public const int Default = 60;
        }
    }
}