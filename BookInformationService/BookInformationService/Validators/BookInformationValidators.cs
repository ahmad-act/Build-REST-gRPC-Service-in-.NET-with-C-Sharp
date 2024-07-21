using FluentValidation;

namespace BookInformationService.Validators
{
    public class GetBookInformationRequestValidator : AbstractValidator<GetBookInformationRequest>
    {
        public GetBookInformationRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }

    public class CreateBookInformationRequestValidator : AbstractValidator<CreateBookInformationRequest>
    {
        public CreateBookInformationRequestValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Stock).GreaterThan(0);
        }
    }

    public class UpdateBookInformationRequestValidator : AbstractValidator<UpdateBookInformationRequest>
    {
        public UpdateBookInformationRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Stock).GreaterThan(0);
        }
    }

    public class DeleteBookInformationRequestValidator : AbstractValidator<DeleteBookInformationRequest>
    {
        public DeleteBookInformationRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }

}
