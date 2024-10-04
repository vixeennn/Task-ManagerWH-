using DTO;


namespace Dal.Interface
{
    public interface IUsersDal
    {
        List<Users> GetAll();
        Users Insert(Users users);
        Users GetUserByUsernameAndPassword(string username, string password);
        Users GetUserByUsername(string username);
        void UpdatePassword(Users user);
        void Delete(int userId);
        Users GetById(int userId); 
    }
}
