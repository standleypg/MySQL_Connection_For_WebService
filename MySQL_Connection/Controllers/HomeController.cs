using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace MySQL_Connection.Controllers
{
    [Route("mysql")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly IConfiguration config_;
        private AccessControlSystem acs_;

        public HomeController(IConfiguration configuration, AccessControlSystem acs)
        {
            config_ = configuration;
            acs_ = acs;
        }

        [Route("pull")]
        [HttpPost]
        public void SelectDataFromDB()
        {
            acs_.SelectDataFromDB();
        }

        [Route("insert")]
        [HttpPost]
        public void AddToDB()
        {
             acs_.AddToDB();
        }

        [Route("update")]
        [HttpPost]
        public void UpdateToDB()
        {
            acs_.UpdateToDB();
        }

        [Route("delete")]
        [HttpPost]
        public void Delete()
        {
            acs_.Delete();
        }
    }
}
