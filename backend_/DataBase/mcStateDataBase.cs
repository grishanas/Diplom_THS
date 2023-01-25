using Microsoft.Data.SqlClient;
using backend_.model;
using System.Collections.Generic;

namespace backend_.DataBase
{
    public class mcStateDataBase
    {
        private string connectionstring;
        public mcStateDataBase()
        {
            connectionstring = "Server=DESKTOP-FN03PQA;Database=Orsha_CHP;Trusted_Connection=True;TrustServerCertificate=true;MultipleActiveResultSets=true";
        }

        public IEnumerable<mc_state> GetMc_States()
        {
            var list = new List<mc_state>();
            try
            {
                using (var db = new SqlConnection(connectionstring))
                {
                    db.Open();
                    var comand = "SELECT [mc_s_id],[mc_s_discription] from [microcontroller_state]";
                    var cmd = new SqlCommand(comand, db);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var mcGroup = new mc_state();
                        mcGroup.mc_s_id = (int)reader.GetValue(0);
                        mcGroup.mc_s_discription = (string)reader.GetValue(1);
                    }
                    db.Close();
                }

            }
            catch (Exception e)
            {

            }
            return list;
        }

        public bool AddMcState(mc_state mc_state)
        {
            try
            {
                using (var db = new SqlConnection(connectionstring))
                {
                    db.Open();
                    var comand = "INSERT [microcontroller_state]([mc_g_id],[mc_s_discription]) VALUES (@mc_s_id,@mc_s_discription) ";
                    var cmd = new SqlCommand(comand, db);
                    var parameter = new SqlParameter();
                    parameter.ParameterName = "@mc_s_id";
                    parameter.Value = mc_state.mc_s_id;
                    cmd.Parameters.Add(parameter);

                    parameter = new SqlParameter();
                    parameter.ParameterName = "@mc_s_discription";
                    parameter.Value = mc_state.mc_s_discription;
                    cmd.Parameters.Add(parameter);
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception e)
            {
                return false;

            }
            return true;
        }

        public bool DeletemcGroup(int id)
        {
            try
            {
                using (var db = new SqlConnection(connectionstring))
                {
                    db.Open();
                    var comand = "DELETE [mc_s_discription]Where [mc_s_id]=@id";
                    var cmd = new SqlCommand(comand, db);
                    var parameter = new SqlParameter();
                    parameter.ParameterName = "@id";
                    parameter.Value = id;
                    cmd.Parameters.Add(parameter);
                    cmd.ExecuteNonQuery();
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
