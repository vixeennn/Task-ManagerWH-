using BusinessLogic.Interface;
using Dal.Interface;
using DTO;
using System.Security.Cryptography;
using System.Text;

public class UsersManager : IUsersManager
{
    private readonly IUsersDal _usersDal;

    public UsersManager(IUsersDal usersDal)
    {
        _usersDal = usersDal;
    }

    public List<Users> GetAllUsers()
    {
        return _usersDal.GetAll();
    }

    public Users AddUser(Users user)
    {
        return _usersDal.Insert(user);
    }

    public Users GetUserByUsernameAndPassword(string username, string password)
    {
        return _usersDal.GetUserByUsernameAndPassword(username, password);
    }

    public Users GetUserByUsername(string username)
    {
        return _usersDal.GetUserByUsername(username);
    }

    public Users GetUserById(int userId)
    {
        return _usersDal.GetById(userId);
    }

    public void UpdateUserPassword(Users user)
    {
        _usersDal.UpdatePassword(user);
    }

    public void DeleteUser(int userId)
    {
        _usersDal.Delete(userId);
    }


    public int GetCurrentUserId(string username)
    {
        var user = GetUserByUsername(username);
        return user?.UserID ?? 0; // Return 0 if user is not found
    }

}