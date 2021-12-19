using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaneSelection.Domain.Policies
{
    public class PolicyData
    {
        public int NumberOfLanes { get; set; }

        public PolicyData(int lanes)
        {
            NumberOfLanes = lanes;
        }
    }
}
