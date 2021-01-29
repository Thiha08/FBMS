using FBMS.Core.Ctos.Filters;
using FBMS.Core.Dtos;
using FBMS.Core.Dtos.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBMS.Core.Interfaces
{
    public interface ITransactionService
    {
        Task<List<TransactionDto>> GetTransactions();

        Task<List<TransactionDto>> GetTransactions(TransactionFilterDto filterDto);

        Task UpdateTransaction(TransactionDto transactionDto);

        Task UpdateTransactions(List<TransactionDto> transactionDtos);

        Task CompleteTransaction(int id, string pricing, string message);

        Task DischargeTransaction(int id);

        Task DeleteTransactions();

        Task DeleteTransactions(TransactionFilterDto filterDto);

        Task CrawlTransactions(TransactionFilterCto filterCto);

        Task TestTransactionsCompletedEmail();
    }
}
