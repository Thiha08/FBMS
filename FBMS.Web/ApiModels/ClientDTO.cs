using FBMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBMS.Web.ApiModels
{
    // Note: doesn't expose events or behavior
    public class ClientDTO
    {
        public int Id { get; set; }
        public string Account { get; set; }
        
        public static ClientDTO FromClient(Client item)
        {
            return new ClientDTO()
            {
                Id = item.Id,
                Account = item.Account
            };
        }
    }
}
