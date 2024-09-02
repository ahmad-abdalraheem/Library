using Domain.Entities;
using FluentValidation;

namespace Presentation.Validation
{
	public class AddBookValidator : AbstractValidator<AddBookDto>
	{
		public AddBookValidator()
		{
			RuleFor(x => x.Title).NotEmpty().NotNull().WithMessage("Title is required.");
			RuleFor(x => x.Title.Length).NotEqual(0).WithMessage("Title cannot contains white space only.");
			RuleFor(x => x.Title).NotEmpty().NotNull().WithMessage("Title is required.");
			RuleFor(x => x.Title.Length).NotEqual(0).WithMessage("Title cannot contains white space only.");
		}
	}

	public class UpdateBookValidator : AbstractValidator<UpdateBookDto>
	{
		public UpdateBookValidator()
		{
			RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id Cannot Be negative or zero.");
			RuleFor(x => x.Title).NotEmpty().NotNull().WithMessage("Title is required.");
			RuleFor(x => x.Title.Length).NotEqual(0).WithMessage("Title cannot contains white space only.");
			RuleFor(x => x.Title).NotEmpty().NotNull().WithMessage("Title is required.");
			RuleFor(x => x.Title.Length).NotEqual(0).WithMessage("Title cannot contains white space only.");
		}
	}
}