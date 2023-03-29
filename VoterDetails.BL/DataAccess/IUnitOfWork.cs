
using System.Threading.Tasks;
using VoterDeatils.DL.Model;

namespace VoterDetails.DataAccess
{
    public interface IUnitOfWork
    {
        #region "unit of work  method"
        Task SaveAsync();
        Task BeginTransactionAsync();
        void CommitTransaction();
        void RollbackTransaction();
        void Dispose();

        #endregion
        #region Properties
        #endregion

        #region Properties
        GenericRepository<VoterDeatils.DL.Model.VoterDetails> VoterRepository { get; }


        #endregion

    }
}
