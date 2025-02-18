using pawpals.Models;
using pawpals.Models.DTOs;
using System.Threading.Tasks;

namespace pawpals.Services
{
    public interface IHomeService
    {
        Task<HomeViewModel> GetHomePageDataAsync(int userId);
    }
}
