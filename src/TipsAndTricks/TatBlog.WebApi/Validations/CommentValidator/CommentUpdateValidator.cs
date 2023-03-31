using FluentValidation;
using TatBlog.WebApi.Models.CommentsModel;

namespace TatBlog.WebApi.Validations.CommentValidator
{
	public class CommentUpdateValidator : AbstractValidator<CommentUpdateModel>
	{
		public CommentUpdateValidator()
		{

			RuleFor(x => x.Description)
				.NotEmpty()
				.WithMessage("Nội dung không được để trống")
				.MaximumLength(2000)
				.WithMessage("Nội dung không vượt quá 100 ký tự");

		}
	}
}
