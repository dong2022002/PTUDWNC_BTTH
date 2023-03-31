using FluentValidation;
using TatBlog.WebApi.Models.SubscribersModel;

namespace TatBlog.WebApi.Validations.SubscriberValidator
{
	public class BlockSubscriberValidator : AbstractValidator<BlockSubscriber>
	{
		public BlockSubscriberValidator()
		{
			RuleFor(x => x.Id)
			   .NotEmpty()
			   .WithMessage("Id không được để trống");
		}
	}
}
