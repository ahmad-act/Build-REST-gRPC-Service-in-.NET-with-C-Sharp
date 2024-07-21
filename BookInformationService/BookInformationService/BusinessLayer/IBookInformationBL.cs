using BookInformationService.Models;

namespace BookInformationService.BusinessLayer
{
    public interface IBookInformationBL
    {
        Task<int> CreateBookInformation(BookInformation bookInformation);
        Task<int> DeleteBookInformation(int id);
        Task<BookInformation?> GetBookInformation(int id);
        Task<List<BookInformation>?> GetBookInformations();
        Task<int> UpdateBookInformation(int id, BookInformation bookInformation);
    }
}