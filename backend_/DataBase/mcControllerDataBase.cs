using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using backend_.model;

namespace backend_.DataBase
{
    public class mcControllerDataBase
    {
        private string connectionstring;
        public mcControllerDataBase()
        {
            connectionstring = "Server=DESKTOP-FN03PQA;Database=Orsha_CHP;Trusted_Connection=True;TrustServerCertificate=true;MultipleActiveResultSets=true";
        }

        public mcController GetMcController(int id)
        {
            var tmp = new mcController();
            tmp.pn_id = id;
            using (SqlConnection db = new SqlConnection(connectionstring))
            {
                db.Open();

                string comand = "SELECT [mc_addres],[mc_discription],[pn_id],[mc_s_id],[mc_name] FROM [Orsha_CHP].[dbo].[microcontroller] WHERE [mc_id]=@id";
                SqlParameter parameter = new SqlParameter();
                parameter.Value = id;
                parameter.ParameterName = "@id";

                SqlCommand cmd = new SqlCommand(comand, db);
                cmd.Parameters.Add(parameter);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    tmp.mc_addres =(int)reader.GetValue(0);
                    tmp.mc_discription = (string)reader.GetValue(1);
                    tmp.pn_id = (int)reader.GetValue(2);
                    tmp.mc_s_id = (int)reader.GetValue(3);
                    tmp.mc_name = (string)reader.GetValue(4);
                }
            }
            return tmp;
        }

        public IEnumerable<mcController> GetMcControllers()
        {
            var list= new List<mcController>();
            try
            {
                using( var db= new SqlConnection(connectionstring))
                {
                    db.Open();
                    string comand = "Select [mc_id],[mc_addres],[mc_discription],[mc_name],[pn_id],[mc_s_id] from [microcontroller]";
                    var cmd = new SqlCommand(comand);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var tmp = new mcController();
                        tmp.mc_id = (int)reader.GetValue(0);
                        tmp.mc_addres = (int)reader.GetValue(1);
                        tmp.mc_discription = (string)reader.GetValue(2);
                        tmp.mc_name = (string)reader.GetValue(3);
                        tmp.pn_id = (int)reader.GetValue(4);
                        tmp.mc_s_id = (int)reader.GetValue(5);
                        list.Add(tmp);
                    }
                }

            }catch(Exception e)
            {

            }
            return list.ToArray();
        }

        public bool AddController(mcController mc)
        {
            try
            {
                using(var db= new SqlConnection(connectionstring))
                {
                    db.Open();

                    var comand = "INSERT [microcontroller]([mc_id],[mc_addres],[mc_discription],[mc_name],[pn_id],[mc_s_id]) values (@mc_id,@mc_addres,@mc_discription,@mc_name,@pn_id,@mc_s_id)";
                    var cmd = new SqlCommand(comand,db);
                    SqlParameter parameter = new SqlParameter();
                    parameter.Value = mc.mc_id;
                    parameter.ParameterName = "@mc_id";
                    cmd.Parameters.Add(parameter);

                    parameter = new SqlParameter();
                    parameter.Value = mc.mc_addres;
                    parameter.ParameterName = "@mc_addres";
                    cmd.Parameters.Add(parameter);

                    parameter = new SqlParameter();
                    parameter.Value = mc.mc_discription;
                    parameter.ParameterName = "@mc_discription";
                    cmd.Parameters.Add(parameter);

                    parameter = new SqlParameter();
                    parameter.Value = mc.mc_name;
                    parameter.ParameterName = "@mc_name";
                    cmd.Parameters.Add(parameter);

                    parameter = new SqlParameter();
                    parameter.Value = mc.pn_id;
                    parameter.ParameterName = "@pn_id";
                    cmd.Parameters.Add(parameter);

                    parameter = new SqlParameter();
                    parameter.Value = mc.mc_s_id;
                    parameter.ParameterName = "@mc_s_id";
                    cmd.Parameters.Add(parameter);

                    cmd.ExecuteNonQuery();
                    db.Close();
                }

            }
            catch(Exception e)
            {
                return false;
            }
            return true;
        }


        public bool DeleteController(int id)
        {
            try
            {
                using (var db= new SqlConnection(connectionstring))
                {
                    db.Open();
                    var comand = "Delete [microcontroller] where [mc_id]=@mc_id";
                    var cmd = new SqlCommand(comand,db);
                    var parameter = new SqlParameter();
                    parameter.ParameterName = "@mc_id";
                    parameter.Value=id;
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
