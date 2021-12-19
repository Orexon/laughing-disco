using LaneSelection.Domain.Consts;
using LaneSelection.Domain.Entities;
using LaneSelection.Domain.Policies;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LaneSelection.Domain.Factories
{
    public sealed class ConveyorFactory : IConveyorFactory
    {
        private readonly IEnumerable<IDestinationPolicy> _policies;

        public ConveyorFactory(IEnumerable<IDestinationPolicy> policies)
        {
            _policies = policies;
        }

        public Conveyor Create(Guid id, Strategy loadingStrategy, int failurePercentage, int timesRouteRepeated) => new(id, loadingStrategy, failurePercentage, timesRouteRepeated);

        public Conveyor CreateWithLanes(Guid id, int loadingStrategy, int failurePercentage, int timesRouteRepeated, int numberOfLanes)
        {
            Strategy strategy = (Strategy)loadingStrategy;

            Conveyor conveyor = Create(id, strategy, failurePercentage, timesRouteRepeated);

            var data = new PolicyData(numberOfLanes);

            var applicablePolicies = _policies.Where(p => p.IsApplicable(data));

            IEnumerable<Destination> destinations = applicablePolicies.SelectMany(p => p.GenerateDestinations(data));

            conveyor.AddLanes(destinations);

            return conveyor;
        }
    }
}
