using Microsoft.AspNet.SignalR;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PushNotification
{
    public class NotificationComponent
    {
        //Here we will add a function  for register notification (will add sql dependency)

        public void RegisterNotification(DateTime currentTime)
        {
            string conStr = ConfigurationManager.ConnectionStrings["OnlineHelpDbContext"].ConnectionString;
            string sqlCommand = @"SELECT [RequestId],[Requestor],[Email] from [dbo].[Request] where [RequestDateTime] > @RequestDateTime";

            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(sqlCommand, con);
                cmd.Parameters.AddWithValue("@RequestDateTime", currentTime);
                if(con.State != System.Data.ConnectionState.Open)
                {
                    con.Open();
                }
                cmd.Notification = null;
                SqlDependency sqlDep = new SqlDependency(cmd);
                sqlDep.OnChange += sqlDep_OnChange;
                using(SqlDataReader reader = cmd.ExecuteReader())
                {

                }
            }
        }

        void sqlDep_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if(e.Type == SqlNotificationType.Change)
            {
                SqlDependency sqlDep = sender as SqlDependency;
                sqlDep.OnChange -= sqlDep_OnChange;

                var notificationHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                notificationHub.Clients.All.notify("added");

                RegisterNotification(DateTime.Now);
            }
        }

        public List<Request> GetRequests(DateTime afterDate)
        {
            using (OnlineHelpDbContext db = new OnlineHelpDbContext())
            {
                return db.Requests.Where(a => a.RequestDateTime > afterDate).OrderByDescending(a => a.RequestDateTime).ToList();

            }
        }
    }
}