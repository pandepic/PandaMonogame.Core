using System;
using System.Collections.Generic;
using System.Text;

namespace PandaMonogame
{
    public interface IPoolableStruct
    {
        bool IsAlive { get; set; }

        void Reset();
    }
}
