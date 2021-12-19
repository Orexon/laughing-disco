using LaneSelection.Domain.Consts;
using LaneSelection.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LaneSelection.Domain.Entities
{
    public class Conveyor
    {
        public Guid Id { get; private set; }
        public Strategy LoadingStrategy { get; private set; }
        public int FailurePercentage { get; private set; }
        public int TimesRouteRepeated { get; private set; }

        public Queue<Load> Loads = new();
        public IReadOnlyCollection<Destination> Destinations => _destinations.AsReadOnly();
        private readonly List<Destination> _destinations = new();


        internal Conveyor(Guid id, Strategy loadingStrategy, int failurePercentage, int timesRouteRepeated)
        {
            Id = id;
            LoadingStrategy = loadingStrategy;
            FailurePercentage = failurePercentage;
            TimesRouteRepeated = timesRouteRepeated;
        }

        internal Conveyor(Guid id, Strategy loadingStrategy, int failurePercentage, int timesRouteRepeated, List<Destination> destinations) 
            : this(id,loadingStrategy,failurePercentage,timesRouteRepeated)
        {
            _destinations = destinations;
        }

        private static readonly Random random = new();
        private static readonly object syncLock = new();

        public void RouteLoads() //Queue<Load> loads, List<Destination> destinations, Strategy loadingStrategy, int failurePercentage
        {
            var range = Enumerable.Range(1, Destinations.Count - 1); //Conveyor destinations. 1 to total destinations minus default destination(0).

            int repeat = TimesRouteRepeated;                         //Consecutive redirect count. a.k.a how many times destination is repeated. 
            int CCRC = 0;                                            //Tracks current consecutive redirect count.

            int currentRoute = range.First();                                
            int lastRoute = range.Last();

            int newRoute = 0;
            int rndRoute = ReturnRandomRoute(random, Destinations);

            while (Loads.Count > 0)
            {
                Load load = Loads.Dequeue();
                

                if (LoadingStrategy == Strategy.RoundRobin)
                {
                    CCRC++; //Or above 2lines
                    SelectRoute(Destinations, load, currentRoute, FailurePercentage);

                    if (CCRC == repeat)
                    {
                        CCRC = 0;                            //Reset current repeat count.

                        if (currentRoute < lastRoute)
                        {
                            currentRoute++;                  //If by the 3rd repeat From is less than LAST route change route by 1+ one.
                        }
                        else if (currentRoute == lastRoute)  //If by the 3rd repeat From is equal to the last route change it to First. Round Robin.
                        {
                            currentRoute = range.First();
                        }
                    }
                } else // Strategy Random.
                {                 
                    if (CCRC != repeat && newRoute == 0)
                    {
                        SelectRoute(Destinations, load, rndRoute, FailurePercentage);

                        CCRC++; 
                    } else if(CCRC == repeat)
                    {
                        CCRC = 0; //Reset current repeat count.

                        newRoute = ReturnRandomRoute(random, Destinations);
                        SelectRoute(Destinations, load, newRoute, FailurePercentage);
                                   
                        CCRC++;          
                    } else {
                        SelectRoute(Destinations, load, newRoute, FailurePercentage);
                        CCRC++;
                    }
                }
            }
        }

        public static void SelectRoute(IReadOnlyCollection<Destination> destinations, Load load,int routeNum, int failurePercentage)
        {
            // Example; Rolls 99(max). Fail chance is 99. Means there is a chance for success. If roll(max 99) and fail chance 100% means 100 out 100 fails.
            // Example; Rolls 0(min). Fail chance is 0(in other words 100% success). 0 is equal to 0. 
            if (RandomChanceOfFailure(random) >= failurePercentage)
            {
                var lane = destinations.ElementAt(routeNum);
                lane.ProcessLoad(load);
            } else {
                var lane = destinations.ElementAt(0);
                lane.ProcessLoad(load);
            }
        }

        public int ReturnRandomRoute(Random rnd, IReadOnlyCollection<Destination> destinations)
        {
            var range = Enumerable.Range(1, destinations.Count); //not -1 as random.Next() range of return values includes the first element, but not the second.

            int First = range.First();
            int Last = range.Last();                


            lock (syncLock)
            { // synchronize
                return rnd.Next(First, Last);
            }
        }

        public static int RandomChanceOfFailure(Random rnd)
        {
            lock (syncLock)
            { // synchronize
                return rnd.Next(100);
            }
        }

        public void AddLane(Destination destination)
        {
            var alreadyExists = _destinations.Any(i => i.LaneIdNumber == destination.LaneIdNumber);

            if (alreadyExists)
            {
                throw new DestinationWithIdAlreadyExistsException(destination.LaneIdNumber);
            }

            _destinations.Insert(destination.LaneIdNumber, destination);
        }

        public void AddLanes(IEnumerable<Destination> destinations)
        {
            foreach (var destination in destinations)
            {
                AddLane(destination);
            }
        }
    }
}

//A user firstly is requested to set up the number of available destinations (0-n)                         
//A user then is requested to set up the destination selection strategy (one of the following 2)

//o Round robin (1,2,3,..n, 1,2,3,…, n, … )

//o Random(select a destination randomly with the same probability weight for destination)

//· A user then is requested to choose the number of consecutive loads that upon arrival to X mark must get the same destination selected.

//· A user then is requested to choose a percentage of failure for load to be diverted into its destination.

//· A user finally is requested to choose the number of loads, that the application should select a destination for.

//· Afterwards the console should print out all the reached destination numbers according to the input specified by user, and print out for every destination number the percentage of loads that have reached it.