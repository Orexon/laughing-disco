using LaneSelection.Domain.Exceptions;
using System.Collections.Generic;

namespace LaneSelection.Domain.Entities
{
    public class Destination
    {
        public int LaneIdNumber { get; private set;}
        public string LaneName { get; private set;}

        public IReadOnlyCollection<Load> LoadedAtDestination => _loads.AsReadOnly();

        private readonly List<Load> _loads = new();

        public Destination(int laneIdNumber, string laneName)
        {
            if(laneIdNumber < 0) {
                throw new LaneNumberIdException();
            }
            if (laneName != laneIdNumber.ToString()) {
                throw new LaneNameException();
            }

            LaneIdNumber = laneIdNumber;
            LaneName = laneName;
        }


        public void ProcessLoad(Load load)
        {
            _loads.Add(load);
        }
    }
}