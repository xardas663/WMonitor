using System;
using System.Data.SqlClient;

namespace Wojtek
{
    public class SQLconnection:MainActivity
    {


        
        public static void connect(bool is_save_settings)
        {


            SqlConnection con = new SqlConnection();


            con.ConnectionString = @"Data Source = xardas663.mssql.somee.com;
            Initial Catalog = xardas663;
            User ID =xardas663_SQLLogin_1; Password = eg3dbmcqnj";

            //con.ConnectionString = @"Data Source = tcp:192.168.43.64,49172;
            //Initial Catalog = Arduino;
            //User ID = wojtek; Password = wojtek";


            try
            {


                con.Open();


            }
            catch (System.Exception)
            {               

            }


            if (is_save_settings == true)
            {
                 string query_settings = "UPDATE settings set AMOUNT=@amount, COLOR_THEME =@colortheme, ALERT=@alert WHERE [USERID]=@userid";
                using (SqlCommand command_save_settings = new SqlCommand(query_settings, con))
                {
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@amount";
                    param.Value = Amount;
                    command_save_settings.Parameters.Add(param);

                    SqlParameter param2 = new SqlParameter();
                    param2.ParameterName = "@colortheme";
                    param2.Value = Color_theme;
                    command_save_settings.Parameters.Add(param2);

                    SqlParameter param3 = new SqlParameter();
                    param3.ParameterName = "@alert";
                    param3.Value = Alert;
                    command_save_settings.Parameters.Add(param3);

                    SqlParameter param4 = new SqlParameter();
                    param4.ParameterName = "@userid";
                    param4.Value = 1;
                    command_save_settings.Parameters.Add(param4);


                    command_save_settings.CommandTimeout = 50;

                    using (SqlDataReader reader = command_save_settings.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Amount = Convert.ToInt16(reader[0]);
                            Color_theme = Convert.ToInt16(reader[1]);
                            Alert = Convert.ToInt16(reader[2]);

                        }
                    }
                }

            }





                if (is_save_settings == false)
            {
                string query_settings = "SELECT  [AMOUNT],[COLOR_THEME],[ALERT] FROM [settings] where [USERID]=@userid ";
                using (SqlCommand command_settings = new SqlCommand(query_settings, con))
                {
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@userid";
                    param.Value = 1;
                    command_settings.Parameters.Add(param);
                    command_settings.CommandTimeout = 50;
                    using (SqlDataReader reader = command_settings.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Amount = Convert.ToInt16(reader[0]);
                            Color_theme = Convert.ToInt16(reader[1]);
                            Alert = Convert.ToInt16(reader[2]);

                        }
                    }
                }





                // wyniki

                string query_results = "SELECT TOP (@amount) [RESULT], [RESULT_TIME], [RESULT_DATE] FROM [temperature] ORDER BY [RESULT_DATE] DESC, [RESULT_TIME] DESC";
                using (SqlCommand command_results = new SqlCommand(query_results, con))
                {
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@amount";
                    command_results.Parameters.Add(param);
                    param.Value = Amount;
                    command_results.CommandTimeout = 50;
                    using (SqlDataReader reader = command_results.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            MResults.Add(new Records()
                            {

                                value = Convert.ToDouble(reader[0]),
                                actualtime = Convert.ToDateTime(reader[1]),
                                string_actualtime = reader[1].ToString(),
                                Actualdate = reader[2].ToString()
                            });

                        }
                    }
                }




                // wykres
                string query_results_today = "SELECT [RESULT], [RESULT_TIME], [RESULT_DATE] FROM [temperature] WHERE [RESULT_DATE]=@date ORDER BY [RESULT_TIME] DESC ";
                using (SqlCommand command_results_today = new SqlCommand(query_results_today, con))
                {
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@date";
                    param.Value =Today_string;
                    Console.WriteLine("data   " + Today_string);
                    command_results_today.Parameters.Add(param);
                    command_results_today.CommandTimeout = 50;
                    using (SqlDataReader reader = command_results_today.ExecuteReader())
                    {
                        Console.WriteLine("jest okkkk");
                        while (reader.Read())
                        {

                            MResults_chart.Add(new Records()
                            {

                                value = Convert.ToDouble(reader[0]),
                                actualtime = Convert.ToDateTime(reader[1]),
                                string_actualtime = reader[1].ToString(),
                                Actualdate = reader[2].ToString()

                            });

                        }
                    }
                }



                // srednia
                string query_average = "SELECT [RESULT],[RESULT_TIME], [RESULT_DATE] FROM [avg_temperature] ORDER BY [RESULT_DATE] DESC";
                using (SqlCommand command_average = new SqlCommand(query_average, con))
                {
                    using (SqlDataReader reader = command_average.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MAverage.Add(new Records()
                            {

                                value = Convert.ToDouble(reader[0]),
                                Actualdate = reader[2].ToString(),
                                string_actualtime = reader[1].ToString(),
                                actualtime = Convert.ToDateTime(reader[1])
                            });
                        }
                    }
                }

            }

        }
    }
}