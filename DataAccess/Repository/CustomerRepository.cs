using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.DTO.Customer;
using DataAccess.IRepository;

namespace DataAccess.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        public Task<IEnumerable<CustomerDTO>> GetAllCustomerAsync()
        {
            throw new System.NotImplementedException();
        }

    }
}