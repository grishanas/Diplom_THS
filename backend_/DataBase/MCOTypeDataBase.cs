using Microsoft.Data.SqlClient;
using backend_.model;
using System.Collections.Generic;

namespace backend_.DataBase
{
    public class MCOTypeDataBase
    {
        private string connectionstring;
        public MCOTypeDataBase()
        {
            connectionstring = "Server=DESKTOP-FN03PQA;Database=Orsha_CHP;Trusted_Connection=True;TrustServerCertificate=true;MultipleActiveResultSets=true";
        }

        public IEnumerable<MCOType> GetMCOTypes()
        {
            var list = new List<MCOType>();
            try
            {
                using (var db = new SqlConnection(connectionstring))
                {
                    db.Open();
                    var comand = "SELECT [mco_t_id],[mco_t_discription] from [microcontroller_output_type]";
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

        public bool AddMCOState(MCOType mc_state)
        {
            try
            {
                using (var db = new SqlConnection(connectionstring))
                {
                    db.Open();
                    var comand = "INSERT [microcontroller_output_type]([mco_t_id],[mco_t_discription]) VALUES (@id,@discription) ";
                    var cmd = new SqlCommand(comand, db);
                    var parameter = new SqlParameter();
                    parameter.ParameterName = "@id";
                    parameter.Value = mc_state.mco_t_id;
                    cmd.Parameters.Add(parameter);

                    parameter = new SqlParameter();
                    parameter.ParameterName = "@discription";
                    parameter.Value = mc_state.mco_t_discription;
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
                    var comand = "DELETE [microcontroller_output_type] Where [mco_t_id]=@id";
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
