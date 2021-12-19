using LaneSelection.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaneSelection.Domain.Factories
{
    public interface ILoadFactory
    {
        Load Create(int id);
        List<Load> CreateNumberOfLoads(int number);
    }
}
