using FluentValidation;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.validations
{
	public class CategoryValidator : AbstractValidator<CategoryEditModel>
	{
		private readonly IBlogRepository _blogRepository;


		public CategoryValidator(IBlogRepository blogRepository)
		{
			_blogRepository = blogRepository;

			RuleFor(x => x.Name)
				.NotEmpty()
				.WithMessage("Tên không được để trống")
				.MaximumLength(500)
				.WithMessage("Tên không vượt quá 500 ký tự");

			RuleFor(x => x.Description)
				.NotEmpty()
				 .WithMessage("Nội dung không được để trống");

			RuleFor(x => x.UrlSlug)
				.NotEmpty()
				.WithMessage("slug không được để trống")
				.MaximumLength(1000)
				.WithMessage("Tên không vượt quá 1000 ký tự")
				.MustAsync(async (catModel, slug, cancellationToken) =>
					!await blogRepository.IsPostSlugExitedAsync(
						catModel.Id, slug, cancellationToken))
				.WithMessage("slug,'{PropertyValue}' đã được sử dụng");
		}
	}
}
