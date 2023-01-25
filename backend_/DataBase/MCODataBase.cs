using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using backend_.model;

namespace backend_.DataBase
{
    public class MCODataBase
    {
        private string connectionstring;
        public MCODataBase()
        {
            connectionstring = "Server=DESKTOP-FN03PQA;Database=Orsha_CHP;Trusted_Connection=True;TrustServerCertificate=true;MultipleActiveResultSets=true";
        }


        public IEnumerable<MCO> GetMCOs()
        {
            var list = new List<MCO>();
            try
            {
                using (var db = new SqlConnection(connectionstring))
                {
                    db.Open();
                    string comand = "Select [mc_o_id],[mc_id],[mc_o_discription],[mco_addres],[mco_s_id],[mco_t_id] from [microcontroller_output]";
                    var cmd = new SqlCommand(comand);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var tmp = new MCO();
                        tmp.mc_o_id = (int)reader.GetValue(0);
                        tmp.mc_id = (int)reader.GetValue(1);
                        tmp.mc_o_discription = (string)reader.GetValue(2);
                        tmp.mco_addres = (string)reader.GetValue(3);
                        tmp.mco_s_id = (int)reader.GetValue(4);
                        tmp.mco_t_id = (int)reader.GetValue(5);
                        list.Add(tmp);
                    }
                }

            }
            catch (Exception e)
            {

            }
            return list.ToArray();
        }

        public bool AddControllerЩгезге(MCO mc)
        {
            try
            {
                using (var db = new SqlConnection(connectionstring))
                {
                    db.Open();

                    var comand = "INSERT [microcontroller_output]([mc_o_id],[mc_id],[mc_o_discription],[mco_addres],[mco_s_id],[mco_t_id]) values (@mc_o_id,@mc_id,@mc_o_discription,@mco_addres,@mco_s_id,@mco_t_id)";
                    var cmd = new SqlCommand(comand, db);
                    SqlParameter parameter = new SqlParameter();
                    parameter.Value = mc.mc_o_id;
                    parameter.ParameterName = "@mc_o_id";
                    cmd.Parameters.Add(parameter);

                    parameter = new SqlParameter();
                    parameter.Value = mc.mc_id;
                    parameter.ParameterName = "@mc_id";
                    cmd.Parameters.Add(parameter);

                    parameter = new SqlParameter();
                    parameter.Value = mc.mc_o_discription;
                    parameter.ParameterName = "@mc_o_discription";
                    cmd.Parameters.Add(parameter);

                    parameter = new SqlParameter();
                    parameter.Value = mc.mco_addres;
                    parameter.ParameterName = "@mco_addres";
                    cmd.Parameters.Add(parameter);

                    parameter = new SqlParameter();
                    parameter.Value = mc.mco_s_id;
                    parameter.ParameterName = "@mco_s_id";
                    cmd.Parameters.Add(parameter);

                    parameter = new SqlParameter();
                    parameter.Value = mc.mco_t_id;
                    parameter.ParameterName = "@mco_t_id";
                    cmd.Parameters.Add(parameter);

                    cmd.ExecuteNonQuery();
                    db.Close();
                }

            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }


        public bool DeleteЬСЩ(int id)
        {
            try
            {
                using (var db = new SqlConnection(connectionstring))
                {
                    db.Open();
                    var comand = "Delete [microcontroller_output] where [mc_id]=@id";
                    var cmd = new SqlCommand(comand, db);
                    var parameter = new SqlParameter();
                    parameter.ParameterName = "@id";
                    parameter.Value = id;
                    cmd.Parameters.Add(parameter);
                    cmd.ExecuteNonQuery();
                    db.Close();
                }

            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}
