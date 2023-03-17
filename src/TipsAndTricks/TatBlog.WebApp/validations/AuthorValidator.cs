using FluentValidation;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.validations
{
	public class AuthorValidator : AbstractValidator<AuthorEditModel>
	{
		private readonly IAuthorRepository _authorRepository;


		public AuthorValidator(IAuthorRepository authorRepository)
		{
			_authorRepository = authorRepository;

			RuleFor(x => x.FullName)
				.NotEmpty()
				.WithMessage("Tên không được để trống")
				.MaximumLength(500)
				.WithMessage("Tên không vượt quá 500 ký tự");


			RuleFor(x => x.Email)
				.NotEmpty()
				 .WithMessage("email không được để trống");



			RuleFor(x => x.UrlSlug)
				.NotEmpty()
				.WithMessage("slug không được để trống")
				.MaximumLength(1000)
				.WithMessage("Tên không vượt quá 1000 ký tự");
			//.MustAsync(async (postModel, slug, cancellationToken) =>
			//	!await blogRepository.IsPostSlugExitedAsync(
			//		postModel.Id, slug, cancellationToken))
			//.WithMessage("slug,'{PropertyValue}' đã được sử dụng");



			//When(x => x.Id <= 0, () =>
			//{
			//	RuleFor(x => x.ImageFile)
			//		.Must(x => x is { Length: > 0 })
			//		.WithMessage("Bạn phải chọn hình ảnh cho bài viết");
			//})
			//.Otherwise(() =>
			//{
			//	RuleFor(x => x.ImageFile)
			//		.MustAsync(SetImageIfNotExist!)
			//		.WithMessage("Bạn phải chọn hình ảnh cho bài viết");
			//});
		}
		private async Task<bool> SetImageIfNotExist(
		 AuthorEditModel postModel,
		 IFormFile imageFile,
		 CancellationToken cancellationToken)
		{
			var author = await _authorRepository.GetAuthorFromIDAsync(
				postModel.Id, cancellationToken);

			if (!string.IsNullOrWhiteSpace(author?.ImageUrl))
				return true;

			return imageFile is { Length: > 0 };
		}
	}

}
