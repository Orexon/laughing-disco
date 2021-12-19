using LaneSelection.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaneSelection.Domain.Exceptions
{
    public class DestinationWithIdAlreadyExistsException : LaneSelectionException
    {
        public int DestinationId { get; }

        public DestinationWithIdAlreadyExistsException(int destinationId)
            : base($"Conveyor system: has already defined lane with id '{destinationId}'")
        {
            DestinationId = destinationId;
        }
    }
}
