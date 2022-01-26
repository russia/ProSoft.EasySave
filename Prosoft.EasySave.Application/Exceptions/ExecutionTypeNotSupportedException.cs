using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSoft.EasySave.Application.Exceptions
{
    public class ExecutionTypeNotSupportedException : Exception
    {
        public ExecutionTypeNotSupportedException()
        {
        }

        public ExecutionTypeNotSupportedException(string message)
            : base(message)
        {
        }

        public ExecutionTypeNotSupportedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}