using Microsoft.Data.SqlClient;
using backend_.model;
using System.Collections.Generic;

namespace backend_.DataBase
{
    public class MCOStateDataBase
    {
        private string connectionstring;
        public MCOStateDataBase()
        {
            connectionstring = "Server=DESKTOP-FN03PQA;Database=Orsha_CHP;Trusted_Connection=True;TrustServerCertificate=true;MultipleActiveResultSets=true";
        }

        public IEnumerable<MCOState> GetMCOStates()
        {
            var list = new List<MCOState>();
            try
            {
                using (var db = new SqlConnection(connectionstring))
                {
                    db.Open();
                    var comand = "SELECT [mco_s_id],[mco_s_discription] from [microcontroller_output_state]";
                    var cmd = new SqlCommand(comand, db);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var mcGroup = new MCOState();
                        mcGroup.mco_s_id = (int)reader.GetValue(0);
                        mcGroup.mco_s_discription = (string)reader.GetValue(1);
                    }
                    db.Close();
                }

            }
            catch (Exception e)
            {

            }
            return list;
        }

        public bool AddMCOState(MCOState mc_state)
        {
            try
            {
                using (var db = new SqlConnection(connectionstring))
                {
                    db.Open();
                    var comand = "INSERT [microcontroller_output_state]([mco_s_id],[mco_s_discription]) VALUES (@id,@discription) ";
                    var cmd = new SqlCommand(comand, db);
                    var parameter = new SqlParameter();
                    parameter.ParameterName = "@id";
                    parameter.Value = mc_state.mco_s_id;
                    cmd.Parameters.Add(parameter);

                    parameter = new SqlParameter();
                    parameter.ParameterName = "@discription";
                    parameter.Value = mc_state.mco_s_discription;
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
                    var comand = "DELETE [microcontroller_output_state] Where [mco_s_id]=@id";
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
