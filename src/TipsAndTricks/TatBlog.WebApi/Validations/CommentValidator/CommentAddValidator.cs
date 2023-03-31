using FluentValidation;
using TatBlog.WebApi.Models.CommentsModel;

namespace TatBlog.WebApi.Validations.CommentValidator
{
    public class CommentAddValidator : AbstractValidator<CommentAddModel>
    {
        public CommentAddValidator()
        {

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Tên không được để trống")
                .MaximumLength(2000)
                .WithMessage("Tên không vượt quá 2000 ký tự");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Nội dung không được để trống")
                .MaximumLength(2000)
                .WithMessage("Nội dung không vượt quá 100 ký tự");

            RuleFor(x => x.PostId)
              .NotEmpty()
              .WithMessage("Post không được để trống");


		}
    }
}
