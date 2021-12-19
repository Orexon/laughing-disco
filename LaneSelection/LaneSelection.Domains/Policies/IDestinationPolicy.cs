using LaneSelection.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaneSelection.Domain.Policies
{
    public interface IDestinationPolicy
    {
        bool IsApplicable(PolicyData data);

        IEnumerable<Destination> GenerateDestinations(PolicyData data);
    }
}
