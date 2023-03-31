using FluentValidation;
using TatBlog.Core.Entities;
using TatBlog.WebApi.Models.SubscribersModel;

namespace TatBlog.WebApi.Validations.SubscriberValidator
{
    public class SubscriberValidator : AbstractValidator<SubscriberEditModel>
    {
        public SubscriberValidator()
        {

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("mail không được để trống")
                .MaximumLength(100)
                .WithMessage("Mail không vượt quá 100 ký tự");

        }
    }
}