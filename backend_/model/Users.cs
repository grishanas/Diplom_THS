namespace backend_.model
{

    public class UserLogin
    {
        public string login { get; set; }
        public string password { get; set; }
    }
    public class User
    {
        public int u_id { get; set; }
        public string u_login { get; set; }
        public string u_password { get; set; }
    }

    public class UserRole
    {
        public int ur_id { get; set; }
         public string? ur_description { get; set; }
    }

    public class UserAndRole
    {
        public int ur_id { get; set; }
        public int u_id { get; set; }

        public List<UserRole> UserRoles { get; set; }
        public List<User> Users { get; set; }
    }
}
