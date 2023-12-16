using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Business.Abstractions.Services;
using NotificationService.Shared.DTO.Request;
using NotificationService.Shared.DTO.Response;
using NotificationService.Shared.Enums;

namespace NotificationService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMessageService _messageService;
        private readonly IValidator<SendMessageRequest> _sendMessageRequestValidator;

        public MessageController(
            IAuthService authService,
            IMessageService messageService,
            IValidator<SendMessageRequest> sendMessageRequestValidator
            )
        {
            _authService = authService;
            _messageService = messageService;
            _sendMessageRequestValidator = sendMessageRequestValidator;
        }

        [HttpGet("details/{IDMessage}")]
        public async Task<IActionResult> GetInfoMessage([FromRoute] long IDMessage, [FromBody] NotificationTypeRequest filter) // Get notification detail
        {
            ValidateTokenResponse validateTokenResponse = await _authService.TokenValidation(HttpContext.Request.Headers.Authorization);

            switch (filter.Type)
            {
                case NotificationType.Info:
                    return Ok(new { message = await _messageService.GetInfoMessage(validateTokenResponse.IDUser, validateTokenResponse.Roles, IDMessage) });
                case NotificationType.Warning:
                    return Ok(new { message = await _messageService.GetWarningMessage(validateTokenResponse.IDUser, validateTokenResponse.Roles, IDMessage) });
                case NotificationType.Critical:
                    return Ok(new { message = await _messageService.GetCriticalMessage(validateTokenResponse.IDUser, validateTokenResponse.Roles, IDMessage) });
                default:
                    return BadRequest();
            }
        }

        [HttpGet("user/{IDUser}")]
        public async Task<IActionResult> GetUserMessages([FromRoute] long IDUser, [FromBody] NotificationTypeRequest filter) // Get notifications from a user
        {
            ValidateTokenResponse validateTokenResponse = await _authService.TokenValidation(HttpContext.Request.Headers.Authorization);

            switch (filter.Type)
            {
                case NotificationType.Info:
                    return Ok(new { message = await _messageService.GetInfoList(validateTokenResponse.IDUser, validateTokenResponse.Roles, IDUser) });
                case NotificationType.Warning:
                    return Ok(new { message = await _messageService.GetWarningList(validateTokenResponse.IDUser, validateTokenResponse.Roles, IDUser) });
                case NotificationType.Critical:
                    return Ok(new { message = await _messageService.GetCriticalList(validateTokenResponse.IDUser, validateTokenResponse.Roles, IDUser) });
                default:
                    return BadRequest();
            }
        }

        [HttpPost("delete/{IDMessage}")]
        public async Task<IActionResult> DeleteMessage([FromRoute] long IDMessage, [FromBody] NotificationTypeRequest filter) // Delete notification
        {
            ValidateTokenResponse validateTokenResponse = await _authService.TokenValidation(HttpContext.Request.Headers.Authorization);

            switch (filter.Type)
            {
                case NotificationType.Info:
                    await _messageService.DeleteInfoMessage(validateTokenResponse.Roles, IDMessage);
                    return Ok(new { message = "Info notification deleted" });
                case NotificationType.Warning:
                    await _messageService.DeleteWarningMessage(validateTokenResponse.Roles, IDMessage);
                    return Ok(new { message = "Warning notification deleted" });
                case NotificationType.Critical:
                    await _messageService.DeleteCriticalMessage(validateTokenResponse.Roles, IDMessage);
                    return Ok(new { message =  "Critical notification deleted" });
                default:
                    return BadRequest();
            }
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest sendMessageRequest) // Send new notification
        {
            var validationResult = await _sendMessageRequestValidator.ValidateAsync(sendMessageRequest);
            if (!validationResult.IsValid)
            {
                return StatusCode(400, new { message = validationResult.Errors.First().ErrorMessage });
            }

            ValidateTokenResponse validateTokenResponse = await _authService.TokenValidation(HttpContext.Request.Headers.Authorization);

            switch (sendMessageRequest.Type)
            {
                case NotificationType.Info:
                    return Ok(new { message = await _messageService.SendInfoMessage(validateTokenResponse.Roles, sendMessageRequest.IDUser, sendMessageRequest.Message) });
                case NotificationType.Warning:
                    return Ok(new { message = await _messageService.SendWarningMessage(validateTokenResponse.Roles, sendMessageRequest.IDUser, sendMessageRequest.Message) });
                case NotificationType.Critical:
                    return Ok(new { message = await _messageService.SendCriticalMessage(validateTokenResponse.Roles, sendMessageRequest.IDUser, sendMessageRequest.Message) });
                default:
                    return BadRequest();
            }
        }
    }
}