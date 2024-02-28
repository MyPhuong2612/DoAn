using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class DataProvider
    {
        private static DataProvider instance; // Ctrl + R + E

        public static DataProvider Instance
        {
            get { if (instance == null) instance = new DataProvider(); return DataProvider.instance; }
            private set { DataProvider.instance = value; }
        }

        private DataProvider() { }

        string connecttionSTR = "Data Source=MSI;Initial Catalog=QuanLyQuanCafe_9;Integrated Security=True";

        public DataTable ExecuteQuery(string query, object[] parameter = null)
        {
            DataTable data = new DataTable();

            using(SqlConnection connecttion = new SqlConnection(connecttionSTR))
            {

                connecttion.Open();

                SqlCommand command = new SqlCommand(query, connecttion);

                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }

                    }
                }

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(data);

                connecttion.Close();

            }
            return data;
        }

        public int ExecuteNonQuery(string query, object[] parameter = null)
        {
            int data = 0;

            using (SqlConnection connecttion = new SqlConnection(connecttionSTR))
            {

                connecttion.Open();

                SqlCommand command = new SqlCommand(query, connecttion);

                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }
                data = command.ExecuteNonQuery();
                connecttion.Close();

            }
            return data;
        }

        public object ExecuteSacalar(string query, object[] parameter = null)
        {
            object data = 0;

            using (SqlConnection connecttion = new SqlConnection(connecttionSTR))
            {

                connecttion.Open();

                SqlCommand command = new SqlCommand(query, connecttion);

                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }
                data = command.ExecuteScalar();
                connecttion.Close();

            }
            return data;
        }

    }
}
