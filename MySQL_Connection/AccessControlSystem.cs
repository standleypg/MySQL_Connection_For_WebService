using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MySQL_Connection
{
    public class AccessControlSystem 
    {
        private MysqlConnector mysqlConn;
        private readonly IConfiguration config_;
        public AccessControlSystem(IConfiguration configuration)
        {
            config_ = configuration;
        }

        public void SelectDataFromDB()
        {
            //initiate connection
            mysqlConn = new MysqlConnector(config_);

            //read data
            using (DataTable dt = mysqlConn.Select("SELECT * FROM bst_connected WHERE bst_removed = 0;"))
            {
                Console.WriteLine("Total devices: {0}", dt.Rows.Count);
                foreach (DataRow row in dt.Rows)
                {
                    Console.WriteLine("IP: {0}\tDEVICE NAME: {1}", row["bst_ip_add"], row[10]);
                }
            }
            //close connection
            mysqlConn.ConnectionClose();
            /*Console.WriteLine("Ip Address:{0}, Username:{1}, Password:{2}, Port:{3}", "test", config_.GetValue<string>("MySql:Username"), config_.GetValue<string>("MySql:Password"), config_.GetValue<string>("MySql:Port"));

            Console.WriteLine("test");*/
        }

        public void AddToDB()
        {
            mysqlConn = new MysqlConnector(config_);

            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();
            param.Add("id", 2);
            param.Add("value", "gundagundi");

            string query_ = "INSERT INTO a (id, value) VALUES (@id, @value)";
            mysqlConn.AddUpdateDelete(query_, param);
            mysqlConn.ConnectionClose();
        }

        public void UpdateToDB()
        {
            mysqlConn = new MysqlConnector(config_);

            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();
            param.Add("id", 2);
            param.Add("value", "update value");

            string query_ = "UPDATE a SET value = @value where id = @id ";
            mysqlConn.AddUpdateDelete(query_, param);
            mysqlConn.ConnectionClose();
        }

        public void Delete()
        {
            mysqlConn = new MysqlConnector(config_);

            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();
            param.Add("id", 2);

            string query_ = "DELETE FROM a where id = @id ";
            mysqlConn.AddUpdateDelete(query_, param);
            mysqlConn.ConnectionClose();
        }
    }
}
