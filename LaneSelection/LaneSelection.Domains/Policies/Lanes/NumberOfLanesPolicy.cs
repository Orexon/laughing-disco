using LaneSelection.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaneSelection.Domain.Policies.Lanes
{
    internal sealed class NumberOfLanesPolicy : IDestinationPolicy
    {

        public bool IsApplicable(PolicyData data)
            => data.NumberOfLanes > 0;

        public IEnumerable<Destination> GenerateDestinations(PolicyData data)
        {
            List<Destination> list = new();

            for (int i = 0; i <= data.NumberOfLanes; i++)
            {
                list.Add(new Destination(i, i.ToString()));
            }

            return list;
        }
    }
}
