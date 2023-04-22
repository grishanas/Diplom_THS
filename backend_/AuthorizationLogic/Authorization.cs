using backend_.DataBase.UserDB;
using backend_.Models.UserModels;

namespace backend_.AuthorizationLogic
{
    public class Authorization
    {
        private readonly UserDBContext _dbContext;

        public Authorization(UserDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> UserAuthentication(UserLogin userLogin)
        {
            var user = await _dbContext.Get(userLogin);
            if (user == null)
                throw new Exception("");
            return "";  
        }
    }
}
