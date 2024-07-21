// The address of the gRPC server.
using BookInformationService;
using Grpc.Net.Client;

using var channel = GrpcChannel.ForAddress("https://localhost:3111");
var client = new BookInformationGrpc.BookInformationGrpcClient(channel);

// Example: CreateBookInformation
var createBookInfoRequest = new CreateBookInformationRequest { Title = "New Book", Stock = 10 };
var createBookInfoResponse = await client.CreateBookInformationAsync(createBookInfoRequest);
Console.WriteLine($"New Book Created with ID={createBookInfoResponse.Id}");

// Example: GetBookInformation by ID
var getBookInfoRequest = new GetBookInformationRequest { Id = createBookInfoResponse.Id };
var getBookInfoResponse = await client.GetBookInformationAsync(getBookInfoRequest);
Console.WriteLine($"Book Info: ID={getBookInfoResponse.Id}, Title={getBookInfoResponse.Title}, Stock={getBookInfoResponse.Stock}, Available={getBookInfoResponse.Available}");

// Example: ListBookInformation
var listBookInfoRequest = new ListBookInformationRequest();
var listBookInfoResponse = await client.ListBookInformationAsync(listBookInfoRequest);
foreach (var book in listBookInfoResponse.Bookinformations)
{
    Console.WriteLine($"Book Info: ID={book.Id}, Title={book.Title}, Stock={book.Stock}, Available={book.Available}");
}


// Example: UpdateBookInformation
var updateBookInfoRequest = new UpdateBookInformationRequest { Id = createBookInfoResponse.Id, Title = "Updated Book", Stock = 15 };
var updateBookInfoResponse = await client.UpdateBookInformationAsync(updateBookInfoRequest);
Console.WriteLine($"Book Updated with ID={updateBookInfoResponse.Id}");

// Example: DeleteBookInformation
var deleteBookInfoRequest = new DeleteBookInformationRequest { Id = createBookInfoResponse.Id };
var deleteBookInfoResponse = await client.DeleteBookInformationAsync(deleteBookInfoRequest);
Console.WriteLine($"Book Deleted with ID={deleteBookInfoResponse.Id}");


Console.ReadLine();