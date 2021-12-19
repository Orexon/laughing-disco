//using LaneSelection.Domain.Entities;
//using System.Collections.Generic;

//namespace LaneSelection.Domain.Policies.Default
//{
//    internal sealed class DefaultPolicy : IDestinationPolicy
//    {
//        //Every Conveyor has Destination 0; Which is Always ID 0 and inserted at Destination 0(first item in list);
//        const int Lane0 = 0;
//        public bool IsApplicable(PolicyData data) => true;

//        public IEnumerable<Destination> GenerateDestinations(PolicyData data) =>
//            new List<Destination>
//            {
//                new Destination(Lane0, Lane0.ToString())
//            };
//    }
//}


//