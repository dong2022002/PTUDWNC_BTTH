using FluentValidation;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.validations
{
	public class SubcriberValidator : AbstractValidator<SubscriberEditModel>
	{
        public SubcriberValidator()
        {
			RuleFor(x => x.Desc)
			   .NotEmpty()
				.WithMessage("Lý do không được để trống");

			RuleFor(x => x.NoteAdmin);
				
		}
    }
}
