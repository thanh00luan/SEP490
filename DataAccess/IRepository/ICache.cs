using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    public interface ICache
    {
        void Set(string key, object value, TimeSpan expiration);
        T Get<T>(string key);
        void Remove(string key);
    }
}
