using LaneSelection.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaneSelection.Domain.Exceptions
{
    public class LaneNameException : LaneSelectionException
    {
        public LaneNameException() : base("Lane name and it's ID must match.")
        {
        }
    }
}
