using NewsManagement.Domain.DTOs;

namespace NewsManagement.Application.Interfaces.Repositories
{
    public interface INewsRepository
    {
        Task<List<NewsDto>?> GetAllNews();
        Task<List<NewsDto>?>? GetNewsByDays(int daysQuantity);
        Task<List<NewsDto>?> GetNewsByText(string text);
        Task<List<NewsDto>?>? GetLatest5News();
        Task<string> Subscribe();
    }
}
