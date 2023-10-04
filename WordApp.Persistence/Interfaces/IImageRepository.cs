namespace WordApp.Persistence.Interfaces
{
    public interface IImageRepository
    {
        Task<string> AddImage(string imageBase64, string imageName);
    }
}
