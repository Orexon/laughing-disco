using LaneSelection.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaneSelection.Domain.Exceptions
{
    public class LaneNumberIdException : LaneSelectionException
    {
        public LaneNumberIdException() : base("Lane number cannot be negative.")
        {
        }
    }
}
