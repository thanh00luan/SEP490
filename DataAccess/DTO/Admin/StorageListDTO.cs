using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Admin
{
    public class StorageListDTO
    {
        public int Totals { get; set; }
        public IEnumerable<StorageManaDTO> Storages { get; set; }
    }
}
