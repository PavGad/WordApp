namespace WordApp.Domain.Interfaces
{
    public interface IPasswordService
    {
        string HashPassword(string password);
        bool CheckPassword(string password, string hash);
    }
}
