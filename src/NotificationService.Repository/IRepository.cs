using Microsoft.EntityFrameworkCore.Storage;
using NotificationService.Repository.Models;

namespace NotificationService.Repository
{
    public interface IRepository
    {
        public void SaveChanges();
        public Task SaveChangesAsync();
        public IDbContextTransaction BeginTransaction();
        public Task<IDbContextTransaction> BeginTransactionAsync();
        public Task InsertInfo(InfoTable info);
        public Task InsertWarning(WarningTable warning);
        public Task InsertCritical(CriticalTable critical);
        public void DeleteInfo(InfoTable info);
        public void DeleteWarning(WarningTable warning);
        public void DeleteCritical(CriticalTable critical);
        public Task<InfoTable?> GetInfo(long idinfo);
        public Task<WarningTable?> GetWarning(long idwarning);
        public Task<CriticalTable?> GetCritical(long idcritical);
        public Task<List<InfoTable>> GetInfoList(long iduser);
        public Task<List<WarningTable>> GetWarningList(long iduser);
        public Task<List<CriticalTable>> GetCriticalList(long iduser);

    }
}
