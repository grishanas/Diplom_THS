using backend_.model;
using Microsoft.Data.SqlClient;

namespace backend_.DataBase
{
    public class IpAdresDataBase
    {
        private string connectionstring;
        public IpAdresDataBase()
        {
            connectionstring = "Server=DESKTOP-FN03PQA;Database=Orsha_CHP;Trusted_Connection=True;TrustServerCertificate=true;MultipleActiveResultSets=true";
        }


        public bool Connection(int n_id, int pn_id)
        {
            try
            {
                using (var db = new SqlConnection(connectionstring))
                {
                    db.Open();

                    string comand = "UPDATE [network_prefix] SET [pn_id]=@pn_id where [n_id]=@n_id ";
                    SqlCommand cmd = new SqlCommand(comand, db);
                    SqlParameter parameter = new SqlParameter();
                    parameter.Value = n_id;
                    parameter.ParameterName = "@n_id";
                    cmd.Parameters.Add(parameter);
                    parameter = new SqlParameter();
                    parameter.Value = pn_id;
                    parameter.ParameterName = "@pn_id";
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

        public IpAdres GetIpAdres(int id)
        {

            var tmp = new IpAdres();
            using (SqlConnection db = new SqlConnection(connectionstring))
            {
                db.Open();

                string comand = "SELECT [n_prefix],[n_addres],[n_id] FROM [Orsha_CHP].[dbo].[network_prefix] WHERE [n_id]=@id";
                SqlParameter parameter = new SqlParameter();
                parameter.Value = id;
                parameter.ParameterName = "@id";

                SqlCommand cmd = new SqlCommand(comand, db);
                cmd.Parameters.Add(parameter);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var tmp1 = reader.GetValue(0);
                    tmp.Prefix = ((byte[])(tmp1))[0];
                    tmp.Ipv4 = (int)reader.GetValue(1);
                    tmp.id = (int)reader.GetValue(2);
                }
            }
            return tmp;
        }



        public bool AddIpAdres(IpAdres ipAdres)
        {
            try
            {
                using (SqlConnection db = new SqlConnection(connectionstring))
                {
                    db.Open();

                    string comand = "INSERT [network_prefix]([n_prefix],[n_addres],[n_id]) VALUES (@pref,@ip,@id)";


                    SqlCommand cmd = new SqlCommand(comand, db);

                    var parameter = new SqlParameter();
                    parameter.Value = ipAdres.id;
                    parameter.ParameterName = "@id";
                    cmd.Parameters.Add(parameter);
                    parameter = new SqlParameter();

                    parameter.Value = ipAdres.Prefix;
                    parameter.ParameterName = "@pref";
                    cmd.Parameters.Add(parameter);
                    parameter = new SqlParameter();
                    parameter.Value = ipAdres.Ipv4;
                    parameter.ParameterName = "@ip";
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

        public IpAdres[] GetIpAdreses()
        {
            var IpAdreses = new List<IpAdres>();
            using (SqlConnection db = new SqlConnection(connectionstring))
            {
                db.Open();

                string comand = "SELECT [n_prefix],[n_addres],[n_id] FROM [Orsha_CHP].[dbo].[network_prefix]";

                SqlCommand cmd = new SqlCommand(comand, db);
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        var tmp = new IpAdres();
                        var tmp1 = reader.GetValue(0);
                        tmp.Prefix = ((byte[])(tmp1))[0];
                        tmp.Ipv4 = (int)reader.GetValue(1);
                        tmp.id = (int)reader.GetValue(2);
                        IpAdreses.Add(tmp);

                    }

                }
                reader.Close();
            }
            return IpAdreses.ToArray();
        }

        public bool DeleteAdres(int id)
        {
            try
            {
                using (SqlConnection db = new SqlConnection(connectionstring))
                {
                    db.Open();
                    string comand = "DELETE [network_prefix] where [n_id]=@id";

                    SqlCommand cmd = new SqlCommand(comand, db);
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

        public bool PutchAdres(IpAdres adres)
        {
            try
            {
                using (SqlConnection db = new SqlConnection(connectionstring))
                {
                    db.Open();
                    string comand = "UPDATE [network_prefix] SET [n_prefix] = @pref,[n_addres]=@ip,  where [n_id]=@id";

                    SqlCommand cmd = new SqlCommand(comand, db);
                    var parameter = new SqlParameter();
                    parameter.ParameterName = "@id";
                    parameter.Value = adres.id;
                    cmd.Parameters.Add(parameter);
                    parameter = new SqlParameter();
                    parameter.ParameterName = "@pref";
                    parameter.Value = adres.Prefix;
                    cmd.Parameters.Add(parameter);
                    parameter = new SqlParameter();
                    parameter.ParameterName = "@ip";
                    parameter.Value = adres.Ipv4;
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
