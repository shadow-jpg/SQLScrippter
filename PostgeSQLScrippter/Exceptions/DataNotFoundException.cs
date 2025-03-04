using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlScrippter.Exceptions
{
    class DataNotFoundException : Exception
    {
        public DateTime ErrorTime { get; } = DateTime.UtcNow;
        public virtual string ErrorCode { get; } = "OrmNoData:0";
    }
}
