using LaneSelection.Domain.Consts;
using LaneSelection.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaneSelection.Domain.Factories
{
    public interface IConveyorFactory
    {
        Conveyor Create(Guid id, Strategy loadingStrategy, int failurePercentage, int timesRouteRepeated);
        Conveyor CreateWithLanes(Guid id, int loadingStrategy, int failurePercentage, int timesRouteRepeated, int numberOfLanes);
    }
}
