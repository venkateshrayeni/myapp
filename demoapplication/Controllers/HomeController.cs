using demoapplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Net.Http;
using System.Web;
using System.Xml.Linq;
namespace demoapplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            var userIp = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            ConnectDB(userIp);
            HomeModel model = new HomeModel();
            model.IpAddress = userIp;
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private void ConnectDB(string userIp)
        {
            string connectionString = "Server=tcp:rayeni-poc-sql1.database.windows.net,1433;Database=pocDb;User ID=venkat;Password='';Encrypt=true;Connection Timeout=30;";
            SqlConnection conn = new SqlConnection(connectionString);
            string sql = "insert into UserData(IPAddress) values ('"+ userIp + "')";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string msg = "Insert Error:";
                msg += ex.Message;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
