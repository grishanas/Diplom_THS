using Microsoft.Data.SqlClient;
using backend_.model;

namespace backend_.DataBase
{
    public class NetworkDataBase
    {
        private string connectionstring;
        public NetworkDataBase()
        {
            connectionstring = "Server=DESKTOP-FN03PQA;Database=Orsha_CHP;Trusted_Connection=True;TrustServerCertificate=true;MultipleActiveResultSets=true";
        }

        public bool AddNetwork(Network net)
        {
            try
            {
                using(SqlConnection conn = new SqlConnection(connectionstring))
                {
                    conn.Open();
                    string comand = "INSERT [parent_network]([pn_discription],[pn_name]) VALUES (@discript,@name)";
                   
                    SqlCommand cmd = new SqlCommand(comand, conn);
                    var parameter = new SqlParameter();
                    parameter.ParameterName = "@discript";
                    parameter.Value = net.discription;
                    cmd.Parameters.Add(parameter);
                    parameter = new SqlParameter();
                    parameter.ParameterName = "@name";
                    parameter.Value = net.name;
                    conn.Close();
                }

            }catch(Exception e)
            {
                return false;
            }
            return true;
        }

        public bool DeleteNetwork(Network net)
        {
            try
            {
                using(var db= new SqlConnection(connectionstring))
                {
                    db.Open();
                    string comand = "DELETE [parent_network] WHERE pn_id=@pn_id";
                    SqlCommand cmd = new SqlCommand(comand, db);
                    var parameter = new SqlParameter();
                    parameter.ParameterName = "@pn.id";
                    parameter.Value = net.id;
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

        public Network GetNetwork(int id)
        {
            Network net = new Network();
            net.id = id;
            try
            {
                using(var db= new SqlConnection(connectionstring))
                {
                    db.Open();
                    string comand = " Select [pn_name],[pn_discription] from [parent_network] where [pn_id]=@pn_id";
                    var cmd = new SqlCommand(comand, db);
                    var parameter = new SqlParameter();
                    parameter.ParameterName = "@pn_id";
                    parameter.Value=id;
                    cmd.Parameters.Add(parameter);
                    var reader = cmd.ExecuteReader();
                    net.name = reader.GetValue(0).ToString();
                    net.discription = reader.GetValue(1).ToString();
                }

            }catch(Exception e)
            {

            }

            return net;
        }

        public IEnumerable<Network> GetNetworks()
        {
            var list= new List<Network>();

            try
            {
                using(var db= new SqlConnection(connectionstring))
                {
                    db.Open();
                    string comand = "Select [pn_id],[pn_name],[pn_discription] from [parent_network]";
                    var cmd = new SqlCommand(comand);
                    var reader= cmd.ExecuteReader();
                    while(reader.Read())
                    {
                        var tmp = new Network();
                        tmp.id = (int)reader.GetValue(0);
                        tmp.name= (string)reader.GetValue(1);
                        tmp.discription=(string)reader.GetValue(2);
                        list.Add(tmp);
                    }
                     
                }

            }catch(Exception e)
            {

            }

            return list.ToArray();
        }

        public bool UpdateNetwork(Network net)
        {
            try
            {
                using(SqlConnection db= new SqlConnection(connectionstring))
                {
                    db.Open();

                    string Comand = "Update [parent_network] SET [pn_name]=@pn_name,[pn_discription]=@pn_disc where [pn_id]=@id";
                    var cmd = new SqlCommand(Comand, db);
                    var parameter = new SqlParameter();
                    parameter.ParameterName = "@id";
                    parameter.Value=net.id;
                    cmd.Parameters.Add(parameter);
                    parameter = new SqlParameter();
                    parameter.ParameterName = "@pn_name";
                    parameter.Value = net.name;
                    cmd.Parameters.Add(parameter);
                    parameter = new SqlParameter();
                    parameter.ParameterName = "@pn_disc";
                    parameter.Value = net.discription;
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
