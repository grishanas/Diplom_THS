using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using backend_.model;

namespace backend_.DataBase
{
    public class m2mGroupControllerDateBase
    {
        private string connectionstring;
        public m2mGroupControllerDateBase()
        {
            connectionstring = "Server=DESKTOP-FN03PQA;Database=Orsha_CHP;Trusted_Connection=True;TrustServerCertificate=true;MultipleActiveResultSets=true";
        }

        public IEnumerable<m2m_GroupController> GetGroupGroup(int id)
        {
            var list = new List<m2m_GroupController>();

            try
            {
                using(SqlConnection db = new SqlConnection(connectionstring))
                {
                    db.Open();
                    var comand = "SELECT [mc_id] from [m2m_mcg_mc] where [mc_g_id]=@id";
                    var cmd= new SqlCommand(comand, db);
                    var parameter = new SqlParameter();
                    parameter.ParameterName = "@id";
                    parameter.Value = id;
                    cmd.Parameters.Add(parameter);
                    var reader = cmd.ExecuteReader();
                    while(reader.Read())
                    {
                        var tmp = new m2m_GroupController();
                        tmp.mc_id = reader.GetInt32(0);
                        tmp.mc_g_id = id;
                        list.Add(tmp);
                    }
                }

            }catch(Exception e)
            {

            }

            return list;

        }


        public IEnumerable<m2m_GroupController> GetGroupController(int id)
        {
            var list = new List<m2m_GroupController>();

            try
            {
                using (SqlConnection db = new SqlConnection(connectionstring))
                {
                    db.Open();
                    var comand = "SELECT [mc_g_id] from [m2m_mcg_mc] where [mc_id]=@id";
                    var cmd = new SqlCommand(comand, db);
                    var parameter = new SqlParameter();
                    parameter.ParameterName = "@id";
                    parameter.Value = id;
                    cmd.Parameters.Add(parameter);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var tmp = new m2m_GroupController();
                        tmp.mc_g_id = reader.GetInt32(0);
                        tmp.mc_id = id;
                        list.Add(tmp);
                    }
                }

            }
            catch (Exception e)
            {

            }

            return list;

        }

        public bool DeleteGroping(int mc_id,int mc_g_id)
        {
            try
            {
                using (var db = new SqlConnection(connectionstring))
                {
                    db.Open();

                    var comand = "DELETE [m2m_mcg_mc] where [mc_id]=@mc_id and [mc_g_id]=@mc_g_id" ;
                    var cmd = new SqlCommand(comand, db);
                    var parameter = new SqlParameter();
                    parameter.ParameterName = "@mc_id";
                    parameter.Value = mc_id;
                    cmd.Parameters.Add(parameter);

                    parameter = new SqlParameter();
                    parameter.ParameterName = "@mc_g_id";
                    parameter.Value = mc_g_id;
                    cmd.Parameters.Add(parameter);

                    cmd.ExecuteNonQuery();

                    db.Close();
                }

            }catch(Exception e)
            {
                return false;
            }
            return true;
        }


    }
}
