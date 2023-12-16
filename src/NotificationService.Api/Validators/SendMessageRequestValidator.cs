using FluentValidation;
using NotificationService.Shared.DTO.Request;

namespace OperationService.Api.Validators
{
    public class SendMessageRequestValidator : AbstractValidator<SendMessageRequest>
    {
        public SendMessageRequestValidator()
        {
            RuleFor(x => x.Message)
                .NotNull()
                .NotEmpty().WithMessage("Message cannot be empty")
                .MaximumLength(500).WithMessage("Message cannot exceed 500 characters");
        }
    }
}
