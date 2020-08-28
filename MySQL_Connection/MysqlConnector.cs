using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MySQL_Connection
{
    public class MysqlConnector
    {
        private MySqlConnection conn;
        private string connStr;

        public MysqlConnector(IConfiguration configuration)
        {
            connStr = "server=" + configuration.GetValue<string>("MySql:Server") + ";user=" + configuration.GetValue<string>("MySql:Username")
                + ";password=" + configuration.GetValue<string>("MySql:Password") + ";port=" + configuration.GetValue<string>("MySql:Port")
                + ";database=" + configuration.GetValue<string>("MySql:Database") + ";keepalive=5000;Convert Zero Datetime=True";

            conn = new MySqlConnection(connStr);

            try
            {
                this.conn.Open();
                if (this.conn.State == ConnectionState.Open)
                {
                    Console.WriteLine("Connection opened.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public DataTable Select(string query)
        {
            DataTable table = new DataTable();
            MysqlReconnect();
            using (MySqlCommand command = new MySqlCommand(query, conn))
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                table.Load(reader);
            }
            return table;
        }

        private void MysqlReconnect()
        {
            if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        //Console.WriteLine("Mysql connection opened.");
                    }
                    else if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
                    {
                        Console.WriteLine("Mysql reconnect failed, try with new mysqlconnection");
                        conn = new MySqlConnection(connStr);
                        conn.Open();
                        if (conn.State == ConnectionState.Open)
                        {
                            Console.WriteLine("New mysql connection opened.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        internal void AddUpdateDelete(string query, Dictionary<string, dynamic> param)
        { 
            MysqlReconnect();
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                for (int i = 0; i < param.Count; ++i)
                {
                    cmd.Parameters.AddWithValue(param.Keys.ElementAt(i), param[param.Keys.ElementAt(i)]);
                }
                cmd.ExecuteNonQuery();
            }
        }

        public void ConnectionClose()
        {
            if (this.conn.State == ConnectionState.Open)
            {
                this.conn.Close();
                Console.WriteLine("Connection closed.");
            }
        }
    }
}
