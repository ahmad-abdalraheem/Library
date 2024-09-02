using Domain.Entities;
using FluentValidation;

namespace Presentation.Validation;

public class MemberValidation
{
	public class MemberValidator : AbstractValidator<Member>
	{
		MemberValidator()
		{
			
		}
	}
	
	public class AddMemberValidator : AbstractValidator<AddMemberDto>
	{
		public AddMemberValidator()
		{
			RuleFor(m => m.Name).NotEmpty().WithMessage("Name is required");
			RuleFor(m=>m.Name.Trim().Length).GreaterThan(0).WithMessage("Name cannot be empty (only white spaces).");
		}
	}
}