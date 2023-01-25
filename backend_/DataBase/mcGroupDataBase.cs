using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using backend_.model;

namespace backend_.DataBase
{
    public class mcGroupDataBase
    {
        private string connectionstring;
        public mcGroupDataBase()
        {
            connectionstring = "Server=DESKTOP-FN03PQA;Database=Orsha_CHP;Trusted_Connection=True;TrustServerCertificate=true;MultipleActiveResultSets=true";
        }

        public IEnumerable<mcGroup> GetMcGroups()
        {
            var list = new List<mcGroup>();
            try
            {
                using (var db = new SqlConnection(connectionstring))
                {
                    db.Open();
                    var comand = "SELECT [mc_g_id],[mc_g_discription] from [microcontroller_group]";
                    var cmd= new SqlCommand(comand,db);
                    var reader= cmd.ExecuteReader();
                    while(reader.Read())
                    {
                        var mcGroup = new mcGroup();
                        mcGroup.mc_g_id = (int)reader.GetValue(0);
                        mcGroup.mc_g_discription = (string)reader.GetValue(1);
                    }
                    db.Close();
                }

            }catch(Exception e)
            {

            }
            return list;
        }

        public bool AddmcGroup(mcGroup mcGroup)
        {
            try
            {
                using (var db = new SqlConnection(connectionstring))
                {
                    db.Open();
                    var comand = "INSERT [microcontroller_group]([mc_g_id],[mc_g_discription]) VALUES (@mc_g_id,@mc_g_discription) ";
                    var cmd = new SqlCommand(comand, db);
                    var parameter = new SqlParameter();
                    parameter.ParameterName = "@mc_g_id";
                    parameter.Value = mcGroup.mc_g_id;
                    cmd.Parameters.Add(parameter);

                    parameter = new SqlParameter();
                    parameter.ParameterName = "@mc_g_discription";
                    parameter.Value = mcGroup.mc_g_discription;
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
                    var comand = "DELETE [microcontroller_group]Where [mc_g_id]=@mc_g_id";
                    var cmd = new SqlCommand(comand, db);
                    var parameter = new SqlParameter();
                    parameter.ParameterName = "@mc_g_id";
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
