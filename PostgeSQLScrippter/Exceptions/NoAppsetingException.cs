using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlScrippter.Exceptions
{
    class NoAppsetingException:CriticalException
    {
        public override string ErrorMessage { get; } = "File with configuration not found!";
        int depth;

        public NoAppsetingException():base()
        {

        }
        public NoAppsetingException(int depth) : base()
        {
            this.depth = depth;
        }
        public NoAppsetingException(string message,int depth) : base(message)
        {
            this.depth = depth;
        }
        public NoAppsetingException(string message, Exception innerException, int Depth) : base(message, innerException)
        {
            this.depth = depth;
        }
    }
}
