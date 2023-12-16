using NotificationService.Shared.DTO.Response;

namespace NotificationService.Business.Abstractions.Services
{
    public interface IAuthService
    {
        public Task<ValidateTokenResponse> TokenValidation(string? token);
    }
}