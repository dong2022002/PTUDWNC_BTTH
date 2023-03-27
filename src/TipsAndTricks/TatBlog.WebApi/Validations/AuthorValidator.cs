using FluentValidation;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApp.validations
{
	public class AuthorValidator : AbstractValidator<AuthorEditModel>
	{
		public AuthorValidator()
		{

			RuleFor(x => x.FullName)
				.NotEmpty()
				.WithMessage("Tên không được để trống")
				.MaximumLength(100)
				.WithMessage("Tên không vượt quá 100 ký tự");

			RuleFor(x => x.UrlSlug)
				.NotEmpty()
				.WithMessage("slug không được để trống")
				.MaximumLength(100)
				.WithMessage("slug không vượt quá 100 ký tự");

			RuleFor(x => x.JoinedDate)
				.GreaterThan(DateTime.MinValue)
				.WithMessage("Ngày tham gia không hợp lệ");

			RuleFor(x => x.Email)
			.NotEmpty()
			 .WithMessage("email không được để trống")
			 .MaximumLength(100)
			 .WithMessage("email không vượt quá 100 ký tự"); 

			RuleFor(x => x.Notes)
			 .MaximumLength(500)
			 .WithMessage("Ghi chú không vượt quá 500 ký tự");

		}
		
	}

}
