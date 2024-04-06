using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DTO.Customer;

namespace DataAccess.IRepository
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<CustomerDTO>> GetAllCustomerAsync();

    }
}
