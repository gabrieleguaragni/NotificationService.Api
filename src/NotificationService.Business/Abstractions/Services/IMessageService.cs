using NotificationService.Shared.DTO.Response;

namespace NotificationService.Business.Abstractions.Services
{
    public interface IMessageService
    {
        public Task<InfoResponse> SendInfoMessage(List<string?> roles, long iduser, string message);
        public Task<WarningResponse> SendWarningMessage(List<string?> roles, long iduser, string message);
        public Task<CriticalResponse> SendCriticalMessage(List<string?> roles, long iduser, string message);
        public Task<InfoResponse> SendInfoMessage(long iduser, string message);
        public Task<WarningResponse> SendWarningMessage(long iduser, string message);
        public Task<CriticalResponse> SendCriticalMessage(long iduser, string message);
        public Task DeleteInfoMessage(List<string?> roles, long idinfo);
        public Task DeleteWarningMessage(List<string?> roles, long idwarning);
        public Task DeleteCriticalMessage(List<string?> roles, long idcritical);
        public Task<InfoResponse> GetInfoMessage(long idRequestingUser, List<string?> roles, long idinfo);
        public Task<WarningResponse> GetWarningMessage(long idRequestingUser, List<string?> roles, long idwarning);
        public Task<CriticalResponse> GetCriticalMessage(long idRequestingUser, List<string?> roles, long idcritical);
        public Task<List<InfoResponse>> GetInfoList(long idRequestingUser, List<string?> roles, long iduser);
        public Task<List<WarningResponse>> GetWarningList(long idRequestingUser, List<string?> roles, long iduser);
        public Task<List<CriticalResponse>> GetCriticalList(long idRequestingUser, List<string?> roles, long iduser);
    }
}
