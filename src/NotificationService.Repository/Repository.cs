using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NotificationService.Repository.Models;

namespace NotificationService.Repository
{
    public class Repository : IRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public Repository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public void SaveChanges() => _applicationDbContext.SaveChanges();

        public async Task SaveChangesAsync() => await _applicationDbContext.SaveChangesAsync();

        public IDbContextTransaction BeginTransaction() => _applicationDbContext.Database.BeginTransaction();

        public async Task<IDbContextTransaction> BeginTransactionAsync() => await _applicationDbContext.Database.BeginTransactionAsync();

        public async Task InsertInfo(InfoTable info)
        {
            await _applicationDbContext.Info.AddAsync(info);
        }

        public async Task InsertWarning(WarningTable warning)
        {
            await _applicationDbContext.Warning.AddAsync(warning);
        }

        public async Task InsertCritical(CriticalTable critical)
        {
            await _applicationDbContext.Critical.AddAsync(critical);
        }

        public void DeleteInfo(InfoTable info)
        {
            _applicationDbContext.Info.Remove(info);
        }

        public void DeleteWarning(WarningTable warning)
        {
            _applicationDbContext.Warning.Remove(warning);
        }

        public void DeleteCritical(CriticalTable critical)
        {
            _applicationDbContext.Critical.Remove(critical);
        }

        public async Task<InfoTable?> GetInfo(long idinfo)
        {
            return await _applicationDbContext.Info.Where(x => x.IDInfo == idinfo).SingleOrDefaultAsync();
        }

        public async Task<WarningTable?> GetWarning(long idwarning)
        {
            return await _applicationDbContext.Warning.Where(x => x.IDWarning == idwarning).SingleOrDefaultAsync();
        }

        public async Task<CriticalTable?> GetCritical(long idcritical)
        {
            return await _applicationDbContext.Critical.Where(x => x.IDCritical == idcritical).SingleOrDefaultAsync();
        }

        public async Task<List<InfoTable>> GetInfoList(long iduser)
        {
            return await _applicationDbContext.Info.Where(x => x.IDUser == iduser).ToListAsync();
        }

        public async Task<List<WarningTable>> GetWarningList(long iduser)
        {
            return await _applicationDbContext.Warning.Where(x => x.IDUser == iduser).ToListAsync();
        }

        public async Task<List<CriticalTable>> GetCriticalList(long iduser)
        {
            return await _applicationDbContext.Critical.Where(x => x.IDUser == iduser).ToListAsync();
        }
    }
}