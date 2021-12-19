using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaneSelection.Shared.Exceptions
{
    public abstract class LaneSelectionException : Exception
    {
        protected LaneSelectionException(string message) : base(message)
        {
        }
    }
}
