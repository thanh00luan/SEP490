using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Admin
{
    public class UserListDTO
    {
        public int TotalUsers { get; set; }
        public IEnumerable<UserManaDTO> Users { get; set; }
    }
}
