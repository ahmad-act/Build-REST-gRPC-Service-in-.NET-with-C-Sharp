using BookInformationService.DataAccessLayer;
using BookInformationService.Models;

namespace BookInformationService.BusinessLayer
{
    public class BookInformationBL : IBookInformationBL
    {
        private readonly ILogger<object> _logger;
        private readonly IBookInformationDL _bookInformationDL;

        public BookInformationBL(ILogger<object> logger, IBookInformationDL bookInformationDL)
        {
            _logger = logger;
            _bookInformationDL = bookInformationDL;
        }

        public async Task<List<BookInformation>?> GetBookInformations()
        {
            List<BookInformation>? bookInformations = await _bookInformationDL.GetBookInformations();

            if (bookInformations == null)
            {
                return null;
            }
            else
            {
                return bookInformations;// bookInformations.Select(b => _mapper.Map<BookInformationDisplayDto>(b)).ToList();
            }
        }

        public async Task<BookInformation?> GetBookInformation(int id)
        {
            BookInformation? bookInformation = await _bookInformationDL.GetBookInformation(id);
            return bookInformation;
        }

        public async Task<int> CreateBookInformation(BookInformation bookInformation)
        {

            int id = await _bookInformationDL.CreateBookInformation(bookInformation);
            return id; 
        }

        public async Task<int> UpdateBookInformation(int id, BookInformation bookInformation)
        {
            if (bookInformation == null)
            {
                return 0;
            }

            var existingBookInformation = await _bookInformationDL.GetBookInformation(id);

            if (existingBookInformation == null)
            {
                return 0;
            }

            existingBookInformation.Title = bookInformation.Title;
            existingBookInformation.Stock = bookInformation.Stock;

            int result = await _bookInformationDL.UpdateBookInformation(existingBookInformation);

            return id; 
        }

        public async Task<int> DeleteBookInformation(int id)
        {
            BookInformation bookInformation = await _bookInformationDL.GetBookInformation(id);

            int result = await _bookInformationDL.DeleteBookInformation(bookInformation);

            return id;
        }
    }
}
