namespace AlutaApp.Constants
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionsForModule(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.Create",
                $"Permissions.{module}.View",
                $"Permissions.{module}.Edit",
                $"Permissions.{module}.Delete",
            };
        }

        public static class ApplicationUsers
        {
            public const string View = "Permissions.ApplicationUser.View";
            public const string Create = "Permissions.ApplicationUser.Create";
            public const string Edit = "Permissions.ApplicationUser.Edit";
            public const string Delete = "Permissions.ApplicationUser.Delete";
        }

        public static class Institutions
        {
            public const string View = "Permissions.Institutions.View";
            public const string Create = "Permissions.Institutions.Create";
            public const string Edit = "Permissions.Institutions.Edit";
            public const string Delete = "Permissions.Institutions.Delete";
        }

        public static class Departments
        {
            public const string View = "Permissions.Departments.View";
            public const string Create = "Permissions.Departments.Create";
            public const string Edit = "Permissions.Departments.Edit";
            public const string Delete = "Permissions.Departments.Delete";
        }

        public static class Points
        {
            public const string View = "Permissions.Points.View";
            public const string Create = "Permissions.Points.Create";
            public const string Edit = "Permissions.Points.Edit";
            public const string Delete = "Permissions.Points.Delete";
        }

        public static class Users
        {
            public const string View = "Permissions.Users.View";
            public const string Create = "Permissions.Users.Create";
            public const string Edit = "Permissions.Users.Edit";
            public const string Delete = "Permissions.Users.Delete";
        }

        public static class Posts
        {
            public const string View = "Permissions.Posts.View";
            public const string Create = "Permissions.Posts.Create";
            public const string Edit = "Permissions.Posts.Edit";
            public const string Delete = "Permissions.Posts.Delete";
        }

        public static class Documents
        {
            public const string View = "Permissions.Documents.View";
            public const string Create = "Permissions.Documents.Create";
            public const string Edit = "Permissions.Documents.Edit";
            public const string Delete = "Permissions.Documents.Delete";
        }

        public static class Trivias
        {
            public const string View = "Permissions.Trivias.View";
            public const string Create = "Permissions.Trivias.Create";
            public const string Edit = "Permissions.Trivias.Edit";
            public const string Delete = "Permissions.Trivias.Delete";
        }

        public static class TGIFs
        {
            public const string View = "Permissions.TGIFs.View";
            public const string Create = "Permissions.TGIFs.Create";
            public const string Edit = "Permissions.TGIFs.Edit";
            public const string Delete = "Permissions.TGIFs.Delete";
        }

        public static class TimeTables
        {
            public const string View = "Permissions.TimeTables.View";
            public const string Create = "Permissions.TimeTables.Create";
            public const string Edit = "Permissions.TimeTables.Edit";
            public const string Delete = "Permissions.TimeTables.Delete";
        }

        public static class CPGAs
        {
            public const string View = "Permissions.CPGAs.View";
            public const string Create = "Permissions.CPGAs.Create";
            public const string Edit = "Permissions.CPGAs.Edit";
            public const string Delete = "Permissions.CPGAs.Delete";
        }

        public static class DocumentComments
        {
            public const string View = "Permissions.DocumentComments.View";
            public const string Create = "Permissions.DocumentComments.Create";
            public const string Edit = "Permissions.DocumentComments.Edit";
            public const string Delete = "Permissions.DocumentComments.Delete";
        }

        public static class Status
        {
            public const string View = "Permissions.Status.View";
            public const string Create = "Permissions.Status.Create";
            public const string Edit = "Permissions.Status.Edit";
            public const string Delete = "Permissions.Status.Delete";
        }

        public static class Notifications
        {
            public const string View = "Permissions.Notifications.View";
            public const string Create = "Permissions.Notifications.Create";
            public const string Edit = "Permissions.Notifications.Edit";
            public const string Delete = "Permissions.Notifications.Delete";
        }

        public static class Promotions
        {
            public const string View = "Permissions.Promotions.View";
            public const string Create = "Permissions.Promotions.Create";
            public const string Edit = "Permissions.Promotions.Edit";
            public const string Delete = "Permissions.Promotions.Delete";
        }

        public static class BannerAds
        {
            public const string View = "Permissions.BannerAds.View";
            public const string Create = "Permissions.BannerAds.Create";
            public const string Edit = "Permissions.BannerAds.Edit";
            public const string Delete = "Permissions.BannerAds.Delete";
        }

        public static class ChatGroups
        {
            public const string View = "Permissions.ChatGroups.View";
            public const string Create = "Permissions.ChatGroups.Create";
            public const string Edit = "Permissions.ChatGroups.Edit";
            public const string Delete = "Permissions.ChatGroups.Delete";
        }

        public static class Messages
        {
            public const string View = "Permissions.Messages.View";
            public const string Create = "Permissions.Messages.Create";
            public const string Edit = "Permissions.Messages.Edit";
            public const string Delete = "Permissions.Messages.Delete";
        }
    }
}
