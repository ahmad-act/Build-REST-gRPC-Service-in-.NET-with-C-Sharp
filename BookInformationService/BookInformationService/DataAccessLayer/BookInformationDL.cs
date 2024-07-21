using BookInformationService.DatabaseContext;
using BookInformationService.Models;
using Microsoft.EntityFrameworkCore;

namespace BookInformationService.DataAccessLayer
{
    public class BookInformationDL : IBookInformationDL
    {
        private readonly ILogger<object> _logger;
        private readonly SystemDbContext _dbContext;

        public BookInformationDL(ILogger<object> logger, SystemDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<List<BookInformation>?> GetBookInformations()
        {
            return await _dbContext.BookInformations.ToListAsync();
        }

        public async Task<BookInformation?> GetBookInformation(int id)
        {
            return await _dbContext.BookInformations.FindAsync(id);
        }

        public async Task<int> CreateBookInformation(BookInformation bookInformation)
        {
            _dbContext.BookInformations.Add(bookInformation);
            int result = await _dbContext.SaveChangesAsync();
            return bookInformation.Id;
        }

        public async Task<int> UpdateBookInformation(BookInformation bookInformation)
        {
            _dbContext.BookInformations.Update(bookInformation);
            int result = await _dbContext.SaveChangesAsync();
            return bookInformation.Id;
        }

        public async Task<int> DeleteBookInformation(BookInformation bookInformation)
        {
            _dbContext.BookInformations.Remove(bookInformation);
            int result = await _dbContext.SaveChangesAsync();
            return bookInformation.Id;
        }
    }
}
