using LaneSelection.Application;
using LaneSelection.Domain.Entities;
using LaneSelection.Domain.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace LaneSelection
{
    public class Program
    {
        private readonly IConveyorFactory _factory;
        private readonly ILoadFactory _loadFactory;

        static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            host.Services.GetRequiredService<Program>().Run();
        }

        public void Run()
        {
            Console.WriteLine("Welcome to LaneSelection!");
            Setup();
        }


        public Program(IConveyorFactory factory, ILoadFactory loadFactory)
        {
            _factory = factory;
            _loadFactory = loadFactory;
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddTransient<Program>();
                    services.AddApplication();
                });
        }

        public void Setup()
        {
            Console.WriteLine("Please setup the Conveyor");

            int Lanes;
            bool loop = true;
            do
            {
                Console.WriteLine("First enter the number of Available destinations: 0 to N.");
                Console.WriteLine("N = available destinations.");
                bool choice = int.TryParse(Console.ReadLine(), out Lanes);
                if (!choice)
                {
                    Console.WriteLine("Wrong Key press. Try again.");
                    continue;
                }
                else loop = false;
            } while (loop);

            int Strategy;
            bool loop2 = true;
            do
            {
                Console.WriteLine("Please choose the destination selection strategy:\n 0) Round robin(1,2,3,...,N, 1,2,3...N) \n 1) Random");
                bool choice = int.TryParse(Console.ReadLine(), out Strategy);
                if (choice)
                {
                    if (Strategy == 0 || Strategy == 1)
                    {
                        loop2 = false;
                    }
                    else
                    {
                        Console.WriteLine("Wrong Key press. Try again.");
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine("Wrong Key press. Try again.");
                    continue;
                }
            } while (loop2);

            int ConcecutiveLoads;
            bool loop3 = true;
            do
            {
                Console.WriteLine("Please choose the number of consecutive loads that upon arrival to sort station(X) must get the same destination selected: ");
                bool choice = int.TryParse(Console.ReadLine(), out ConcecutiveLoads);
                if (!choice)
                {
                    Console.WriteLine("Wrong Key press. Try again.");
                    continue;
                }
                else loop3 = false;
            } while (loop3);

            int PercentageOfFail;
            bool loop4 = true;
            do
            {
                Console.WriteLine("Please choose percentage(out of 100) of failure for load to be diverted into its destination: ");
                bool choice = int.TryParse(Console.ReadLine(), out PercentageOfFail);
                if (choice)
                {
                    if (PercentageOfFail <= 100)
                    {
                        loop4 = false;
                    }
                    else
                    {
                        Console.WriteLine("Wrong Key press. Try again.");
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine("Wrong Key press. Try again.");
                    continue;
                }
            } while (loop4);

            Conveyor conveyor = CreateConveyor(Strategy, PercentageOfFail, ConcecutiveLoads,Lanes);

            int NumberOfLoads;
            bool loop5 = true;
            do
            {
                Console.WriteLine("Please choose the number of loads, that the application should select a destination for: ");
                bool choice = int.TryParse(Console.ReadLine(), out NumberOfLoads);
                if (!choice)
                {
                    Console.WriteLine("Wrong Key press. Try again.");
                    continue;
                }
                else loop5 = false;
            } while (loop5);

            Queue<Load> laods = CreateLoads(NumberOfLoads, conveyor);
            
            RunConveyer(conveyor);
            CalculateResult(conveyor, NumberOfLoads);
        }

        private Conveyor CreateConveyor(int loadingStrategy, int failurePercentage, int timesRouteRepeated,int numberOfLanes)
        {
            Guid guid = Guid.NewGuid();
            return _factory.CreateWithLanes(guid, loadingStrategy, failurePercentage, timesRouteRepeated, numberOfLanes);
        }

        private Queue<Load> CreateLoads(int NumberOfLoads, Conveyor conveyor)
        {
            List<Load> loads = _loadFactory.CreateNumberOfLoads(NumberOfLoads);

            foreach (var laod in loads)
            {
                conveyor.Loads.Enqueue(laod);
            }

            return conveyor.Loads;
        }

        private void RunConveyer(Conveyor conveyor)
        {
            conveyor.RouteLoads();
        }

        private void CalculateResult(Conveyor conveyor, int NumberOfLoads)
        {
            Console.Clear();
            foreach (var Lane in conveyor.Destinations)
            {

                double percent;
                if (Lane.LoadedAtDestination.Count == 0)
                {
                    percent = 0;
                }
                else
                {
                    percent = (double)Lane.LoadedAtDestination.Count / (double)NumberOfLoads * 100;
                }

                if (Lane.LaneIdNumber == 0)
                {
                    Console.WriteLine($"Default Lane:{Lane.LaneName} has total {Lane.LoadedAtDestination.Count} items. {Math.Round(percent, 2)}% of items have been redirected to it.");
                }
                else
                    Console.WriteLine($"Lane number: {Lane.LaneName} has total {Lane.LoadedAtDestination.Count} items. {Math.Round(percent, 2)}% of loads reached Lane:{Lane.LaneName} ");

                //IF wish to see item id's/Numbers. 
                //foreach (var item in Lane.LoadedAtDestination)
                //{

                //    Console.WriteLine($"Number: {item.Id}");
                //}
            }
        }
    }
}
