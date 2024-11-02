using DTO;

namespace BusinessLogic.Interface
{
    public interface IUsersManager
    {
        
            List<Users> GetAllUsers();
            Users AddUser(Users user);
            Users GetUserByUsernameAndPassword(string username, string password);
            Users GetUserByUsername(string username);
            Users GetUserById(int userId);
            void UpdateUserPassword(Users user);
            void DeleteUser(int userId);



    }
}
