using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PandaMonogame
{
    public interface IPoolable
    {
        int PoolIndex { get; set; }
        bool ObjectAlive { get; set; }

        void New();
        void Delete();
    }
}
