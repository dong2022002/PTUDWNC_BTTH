using FluentValidation;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Models.PostsModel;

namespace TatBlog.WebApi.Validations
{
	public class PostValidator :AbstractValidator<PostEditModel>
	{
		


		public PostValidator()
		{
			

			RuleFor(x => x.Title)
				.NotEmpty()
				.WithMessage("Tên không được để trống")
				.MaximumLength(500)
				.WithMessage("Tên không vượt quá 500 ký tự");

			RuleFor(x => x.ShortDescription)
				.NotEmpty()
				 .WithMessage("Giới thiệu không được để trống");

			RuleFor(x => x.Description)
				.NotEmpty()
				 .WithMessage("Nội dung không được để trống");

			RuleFor(x => x.Meta)
				.NotEmpty()
				 .WithMessage("Meta không được để trống")
				.MaximumLength(1000)
				.WithMessage("Tên không vượt quá 1000 ký tự");

			RuleFor(x => x.UrlSlug)
				.NotEmpty()
				.WithMessage("slug không được để trống")
				.MaximumLength(100)
				.WithMessage("slug không vượt quá 100 ký tự");

			RuleFor(x => x.CategoryId)
				.NotEmpty()
				.WithMessage("Bạn phải chọn chủ để cho bài viết");

			RuleFor(x => x.AuthorId)
				.NotEmpty()
				.WithMessage("Bạn phải chọn tác giả của bài viết");

			RuleFor(x => x.SelectedTags)
				.Must(HasAtLeastOneTag!)
				.WithMessage("Bạn phải nhập ít nhất một thẻ");

		

		}

		private bool HasAtLeastOneTag(
			PostEditModel postModel,
			string selectedTags)
		{
			return postModel.GetSelectedTags().Any();
		}
	}
}
