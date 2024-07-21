using BookInformationService.BusinessLayer;
using BookInformationService.Models;
using FluentValidation;
using Grpc.Core;

namespace BookInformationService.Services
{
    public class BookInformationGrpcService : BookInformationGrpc.BookInformationGrpcBase
    {
        private readonly ILogger<BookInformationGrpcService> _logger;
        private readonly IBookInformationBL _bookInformationBL;
        private readonly IValidator<GetBookInformationRequest> _getValidator;
        private readonly IValidator<CreateBookInformationRequest> _createValidator;
        private readonly IValidator<UpdateBookInformationRequest> _updateValidator;
        private readonly IValidator<DeleteBookInformationRequest> _deleteValidator;

        public BookInformationGrpcService(
            ILogger<BookInformationGrpcService> logger,
            IBookInformationBL bookInformationBL,
            IValidator<GetBookInformationRequest> getValidator,
            IValidator<CreateBookInformationRequest> createValidator,
            IValidator<UpdateBookInformationRequest> updateValidator,
            IValidator<DeleteBookInformationRequest> deleteValidator)
        {
            _logger = logger;
            _bookInformationBL = bookInformationBL;
            _getValidator = getValidator;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _deleteValidator = deleteValidator;
        }

        // Get by ID
        public override async Task<GetBookInformationResponse> GetBookInformation(GetBookInformationRequest request, ServerCallContext context)
        {
            var validationResult = await _getValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, string.Join(", ", validationResult.Errors)));
            }

            var bookInformation = await _bookInformationBL.GetBookInformation(request.Id);
            if (bookInformation == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Book not found"));
            }

            return new GetBookInformationResponse
            {
                Id = bookInformation.Id,
                Title = bookInformation.Title,
                Stock = bookInformation.Stock,
                Available = bookInformation.Available
            };
        }

        // List 
        public override async Task<ListBookInformationResponse> ListBookInformation(ListBookInformationRequest request, ServerCallContext context)
        {
            var bookInformations = await _bookInformationBL.GetBookInformations();

            var response = new ListBookInformationResponse();
            bookInformations?.ForEach(bookInformation =>
            {
                response.Bookinformations.Add(new GetBookInformationResponse
                {
                    Id = bookInformation.Id,
                    Title = bookInformation.Title,
                    Stock = bookInformation.Stock,
                    Available = bookInformation.Available
                });
            });

            return response;
        }

        // Create 
        public override async Task<CreateBookInformationResponse> CreateBookInformation(CreateBookInformationRequest request, ServerCallContext context)
        {
            var validationResult = await _createValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, string.Join(", ", validationResult.Errors)));
            }

            var bookInformation = new BookInformation()
            {
                Title = request.Title,
                Stock = request.Stock,
                Available = request.Stock
            };

            var id = await _bookInformationBL.CreateBookInformation(bookInformation);

            return new CreateBookInformationResponse { Id = id };
        }

        // Update
        public override async Task<UpdateBookInformationResponse> UpdateBookInformation(UpdateBookInformationRequest request, ServerCallContext context)
        {
            var validationResult = await _updateValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, string.Join(", ", validationResult.Errors)));
            }

            var bookInformation = new BookInformation
            {
                Title = request.Title,
                Stock = request.Stock,
                Available = request.Stock
            };

            var id = await _bookInformationBL.UpdateBookInformation(request.Id, bookInformation);
            if (id == 0)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Book not found"));
            }

            return new UpdateBookInformationResponse { Id = id };
        }

        // Delete
        public override async Task<DeleteBookInformationResponse> DeleteBookInformation(DeleteBookInformationRequest request, ServerCallContext context)
        {
            var validationResult = await _deleteValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, string.Join(", ", validationResult.Errors)));
            }

            var id = await _bookInformationBL.DeleteBookInformation(request.Id);
            if (id == 0)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Book not found"));
            }

            return new DeleteBookInformationResponse { Id = id };
        }
    }
}
