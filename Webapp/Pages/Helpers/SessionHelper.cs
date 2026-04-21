namespace Webapp.Pages.Helpers
{
    namespace Webapp.Helpers
    {
        public static class SessionHelper
        {
            public static int? GetUserId(HttpContext context)
            {
                return context.Session.GetInt32("UserId");
            }

            public static string? GetRoleName(HttpContext context)
            {
                return context.Session.GetString("RoleName");
            }

            public static string? GetFullName(HttpContext context)
            {
                return context.Session.GetString("FullName");
            }
        }
    }   
}
