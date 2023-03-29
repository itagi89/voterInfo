
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using VoterDeatils.DL;
using VoterDetails.DataAccess;

namespace AmsIndia.DataAccess
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {

        #region Private Properties
        private VoterDetailsDBContext _dbContext;
        private bool _disposed = false;
        private readonly ILogger<UnitOfWork> _logger;

        private IDbContextTransaction Transaction;

        #endregion

        #region Constructor
        public UnitOfWork(VoterDetailsDBContext dbContext, ILogger<UnitOfWork> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException($"{nameof(dbContext)}");
            _logger = logger;
        }
        #endregion 

        #region << Property >>

        private GenericRepository<VoterDeatils.DL.Model.VoterDetails> _voterRepository;

        //public GenericRepository<VoterDeatils.DL.Model.VoterDetails> VoterRepository
        //{
        //    get { return _voterRepository ??= new GenericRepository<VoterDeatils.DL.Model.VoterDetails> (_dbContext); }
        //}

        GenericRepository<VoterDeatils.DL.Model.VoterDetails> IUnitOfWork.VoterRepository
        {
            get { return _voterRepository ??= new GenericRepository<VoterDeatils.DL.Model.VoterDetails>(_dbContext); }
        }

        #endregion

        #region Db Context Methods

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            if (Transaction == null)
            {
                Transaction = await _dbContext.Database.BeginTransactionAsync();
            }
        }

        public void CommitTransaction()
        {
            Transaction?.Commit();
        }

        public void RollbackTransaction()
        {
            Transaction?.Rollback();
        }

        #endregion

        #region Dispose Methods

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

      



        #endregion
    }
}
