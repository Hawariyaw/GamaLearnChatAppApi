using Hangfire.Dashboard;

namespace Server.Filters
{

    public class HangFireAuthorizationFilter : IDashboardAuthorizationFilter
    {

        public bool Authorize(DashboardContext context)
        {
            return true;
        }
    }
}