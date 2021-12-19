using LaneSelection.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaneSelection.Domain.Factories
{
    public sealed class LoadFactory : ILoadFactory
    {
        public Load Create(int id) => new(id);

        public List<Load> CreateNumberOfLoads(int number)
        {
            List<Load> loads = new();

            for (int i = 1; i <= number; i++)
            {
                loads.Add(new Load(i));
            }

            return loads;
        }
    }
}
