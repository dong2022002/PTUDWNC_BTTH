using FluentValidation;
using TatBlog.WebApi.Models.TagsModel;

namespace TatBlog.WebApi.Validations
{
	public class TagValidator : AbstractValidator<TagEditModel>
	{
		public TagValidator()
		{

			RuleFor(x => x.Name)
				.NotEmpty()
				.WithMessage("Tên không được để trống")
				.MaximumLength(100)
				.WithMessage("Tên không vượt quá 100 ký tự");

			RuleFor(x => x.Description)
				.NotEmpty()
				 .WithMessage("Nội dung không được để trống");

			RuleFor(x => x.UrlSlug)
				.NotEmpty()
				.WithMessage("slug không được để trống")
				.MaximumLength(100)
				.WithMessage("slug không vượt quá 100 ký tự");
		}
	}
}
