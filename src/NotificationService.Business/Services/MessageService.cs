using NotificationService.Business.Abstractions.Services;
using NotificationService.Business.Exceptions;
using NotificationService.Repository;
using NotificationService.Repository.Models;
using NotificationService.Shared.DTO.Response;

namespace NotificationService.Business.Services
{
    public class MessageService : IMessageService
    {
        private readonly IRepository _repository;
        private readonly string administrator = "administrator";
        private readonly string developer = "developer";

        public MessageService(
            IRepository repository
            )
        {
            _repository = repository;
        }

        public async Task<InfoResponse> SendInfoMessage(List<string?> roles, long iduser, string message)
        {
            roles = roles.Where(x => x != null).ToList();
            if (!roles.Contains(administrator) && !roles.Contains(developer))
            {
                throw new HttpStatusException(403, "Invalid permission to send message");
            }

            return await SendInfoMessage(iduser, message);
        }

        public async Task<WarningResponse> SendWarningMessage(List<string?> roles, long iduser, string message)
        {
            roles = roles.Where(x => x != null).ToList();
            if (!roles.Contains(administrator) && !roles.Contains(developer))
            {
                throw new HttpStatusException(403, "Invalid permission to send message");
            }

            return await SendWarningMessage(iduser, message);
        }

        public async Task<CriticalResponse> SendCriticalMessage(List<string?> roles, long iduser, string message)
        {
            roles = roles.Where(x => x != null).ToList();
            if (!roles.Contains(administrator) && !roles.Contains(developer))
            {
                throw new HttpStatusException(403, "Invalid permission to send message");
            }

            return await SendCriticalMessage(iduser, message);
        }

        public async Task<InfoResponse> SendInfoMessage(long iduser, string message)
        {
            InfoTable newInfo = new()
            {
                IDUser = iduser,
                Message = message,
                Date = DateTime.Now
            };

            await _repository.InsertInfo(newInfo);
            await _repository.SaveChangesAsync();

            return new()
            {
                IDInfo = newInfo.IDInfo,
                IDUser = newInfo.IDUser,
                Message = newInfo.Message,
                Date = newInfo.Date
            };
        }

        public async Task<WarningResponse> SendWarningMessage(long iduser, string message)
        {
            WarningTable newWarning = new()
            {
                IDUser = iduser,
                Message = message,
                Date = DateTime.Now
            };

            await _repository.InsertWarning(newWarning);
            await _repository.SaveChangesAsync();

            return new()
            {
                IDWarning = newWarning.IDWarning,
                IDUser = newWarning.IDUser,
                Message = newWarning.Message,
                Date = newWarning.Date
            };
        }

        public async Task<CriticalResponse> SendCriticalMessage(long iduser, string message)
        {
            CriticalTable newCritical = new()
            {
                IDUser = iduser,
                Message = message,
                Date = DateTime.Now
            };

            await _repository.InsertCritical(newCritical);
            await _repository.SaveChangesAsync();

            return new()
            {
                IDCritical = newCritical.IDCritical,
                IDUser = newCritical.IDUser,
                Message = newCritical.Message,
                Date = newCritical.Date
            };
        }

        public async Task DeleteInfoMessage(List<string?> roles, long idinfo)
        {
            roles = roles.Where(x => x != null).ToList();
            if (!roles.Contains(administrator) && !roles.Contains(developer))
            {
                throw new HttpStatusException(403, "Invalid permission to delete message");
            }

            InfoTable? info = await _repository.GetInfo(idinfo);
            if (info == null)
            {
                throw new HttpStatusException(404, "This notification not exists");
            }

            _repository.DeleteInfo(info);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteWarningMessage(List<string?> roles, long idwarning)
        {
            roles = roles.Where(x => x != null).ToList();
            if (!roles.Contains(administrator) && !roles.Contains(developer))
            {
                throw new HttpStatusException(403, "Invalid permission to delete message");
            }

            WarningTable? warning = await _repository.GetWarning(idwarning);
            if (warning == null)
            {
                throw new HttpStatusException(404, "This notification not exists");
            }

            _repository.DeleteWarning(warning);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteCriticalMessage(List<string?> roles, long idcritical)
        {
            roles = roles.Where(x => x != null).ToList();
            if (!roles.Contains(administrator) && !roles.Contains(developer))
            {
                throw new HttpStatusException(403, "Invalid permission to delete message");
            }

            CriticalTable? critical = await _repository.GetCritical(idcritical);
            if (critical == null)
            {
                throw new HttpStatusException(404, "This notification not exists");
            }

            _repository.DeleteCritical(critical);
            await _repository.SaveChangesAsync();
        }

        public async Task<InfoResponse> GetInfoMessage(long idRequestingUser, List<string?> roles, long idinfo)
        {
            InfoTable? info = await _repository.GetInfo(idinfo);
            if (info == null)
            {
                throw new HttpStatusException(404, "This notification not exists");
            }

            if (idRequestingUser != info.IDUser)
            {
                roles = roles.Where(x => x != null).ToList();
                if (!roles.Contains(administrator) && !roles.Contains(developer))
                {
                    throw new HttpStatusException(403, "Invalid permission to read notifications of other users");
                }
            }

            return new() { 
                IDInfo = info.IDInfo,
                IDUser = info.IDUser,
                Message = info.Message,
                Date = info.Date
            };
        }

        public async Task<WarningResponse> GetWarningMessage(long idRequestingUser, List<string?> roles, long idwarning)
        {
            WarningTable? warning = await _repository.GetWarning(idwarning);
            if (warning == null)
            {
                throw new HttpStatusException(404, "This notification not exists");
            }

            if (idRequestingUser != warning.IDUser)
            {
                roles = roles.Where(x => x != null).ToList();
                if (!roles.Contains(administrator) && !roles.Contains(developer))
                {
                    throw new HttpStatusException(403, "Invalid permission to read notifications of other users");
                }
            }

            return new()
            {
                IDWarning = warning.IDWarning,
                IDUser = warning.IDUser,
                Message = warning.Message,
                Date = warning.Date
            };
        }

        public async Task<CriticalResponse> GetCriticalMessage(long idRequestingUser, List<string?> roles, long idcritical)
        {
            CriticalTable? critical = await _repository.GetCritical(idcritical);
            if (critical == null)
            {
                throw new HttpStatusException(404, "This notification not exists");
            }

            if (idRequestingUser != critical.IDUser)
            {
                roles = roles.Where(x => x != null).ToList();
                if (!roles.Contains(administrator) && !roles.Contains(developer))
                {
                    throw new HttpStatusException(403, "Invalid permission to read notifications of other users");
                }
            }

            return new()
            {
                IDCritical = critical.IDCritical,
                IDUser = critical.IDUser,
                Message = critical.Message,
                Date = critical.Date
            };
        }

        public async Task<List<InfoResponse>> GetInfoList(long idRequestingUser, List<string?> roles, long iduser)
        {
            if (idRequestingUser != iduser)
            {
                roles = roles.Where(x => x != null).ToList();
                if (!roles.Contains(administrator) && !roles.Contains(developer))
                {
                    throw new HttpStatusException(403, "Invalid permission to read notifications of other users");
                }
            }

            List<InfoResponse> list = new();
            foreach (var item in await _repository.GetInfoList(iduser))
            {
                list.Add(new InfoResponse()
                {
                    IDInfo = item.IDInfo,
                    IDUser = item.IDUser,
                    Message = item.Message,
                    Date = item.Date
                });
            }

            return list;
        }

        public async Task<List<WarningResponse>> GetWarningList(long idRequestingUser, List<string?> roles, long iduser)
        {
            if (idRequestingUser != iduser)
            {
                roles = roles.Where(x => x != null).ToList();
                if (!roles.Contains(administrator) && !roles.Contains(developer))
                {
                    throw new HttpStatusException(403, "Invalid permission to read notifications of other users");
                }
            }

            List<WarningResponse> list = new();
            foreach (var item in await _repository.GetWarningList(iduser))
            {
                list.Add(new WarningResponse()
                {
                    IDWarning = item.IDWarning,
                    IDUser = item.IDUser,
                    Message = item.Message,
                    Date = item.Date
                });
            }

            return list;
        }

        public async Task<List<CriticalResponse>> GetCriticalList(long idRequestingUser, List<string?> roles, long iduser)
        {
            if (idRequestingUser != iduser)
            {
                roles = roles.Where(x => x != null).ToList();
                if (!roles.Contains(administrator) && !roles.Contains(developer))
                {
                    throw new HttpStatusException(403, "Invalid permission to read notifications of other users");
                }
            }

            List<CriticalResponse> list = new();
            foreach (var item in await _repository.GetCriticalList(iduser))
            {
                list.Add(new CriticalResponse()
                {
                    IDCritical = item.IDCritical,
                    IDUser = item.IDUser,
                    Message = item.Message,
                    Date = item.Date
                });
            }

            return list;
        }
    }
}
