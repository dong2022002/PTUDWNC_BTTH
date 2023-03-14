using FluentValidation;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.validations
{
    public class PostValidator :AbstractValidator<PostEditModel>
    {
        private readonly IBlogRepository _blogRepository;


        public PostValidator(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;

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
                .MaximumLength(1000)
                .WithMessage("Tên không vượt quá 1000 ký tự")
                .MustAsync(async (postModel, slug, cancellationToken) =>
                    !await blogRepository.IsPostSlugExitedAsync(
                        postModel.Id, slug, cancellationToken))
                .WithMessage("slug,'{PropertyValue}' đã được sử dụng");

            RuleFor(x => x.CategoryId)
                .NotEmpty()
                .WithMessage("Bạn phải chọn chủ để cho bài viết");

            RuleFor(x => x.AuthorId)
                .NotEmpty()
                .WithMessage("Bạn phải chọn tác giả của bài viết");

            RuleFor(x => x.SelectedTags)
                .Must(HasAtLeastOneTag!)
                .WithMessage("Bạn phải nhập ít nhất một thẻ");

            When(x => x.Id <= 0, () =>
            {
                RuleFor(x => x.ImageFile)
                    .Must(x => x is { Length: > 0 })
                    .WithMessage("Bạn phải chọn hình ảnh cho bài viết");
            })
            .Otherwise(() =>
            {
                RuleFor(x => x.ImageFile)
                    .MustAsync(SetImageIfNotExist!)
                    .WithMessage("Bạn phải chọn hình ảnh cho bài viết");
            });

        }

        private async Task<bool> SetImageIfNotExist(
            PostEditModel postModel,
            IFormFile imageFile,
            CancellationToken cancellationToken)
        {
            var post = await _blogRepository.GetPostByIdAsync(
                postModel.Id, false, cancellationToken);

            if (!string.IsNullOrWhiteSpace(post?.ImageUrl))
                return true;

            return imageFile is { Length: > 0 };
        }

        private bool HasAtLeastOneTag(
            PostEditModel postModel,
            string selectedTags)
        {
            return postModel.GetSelectedTags().Any();
        }
    }
}
