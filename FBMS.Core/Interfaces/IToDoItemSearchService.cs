using Ardalis.Result;
using FBMS.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBMS.Core.Interfaces
{
    public interface IToDoItemSearchService
    {
        Task<Result<ToDoItem>> GetNextIncompleteItemAsync();
        Task<Result<List<ToDoItem>>> GetAllIncompleteItemsAsync(string searchString);
    }
}
