using System;
using System.Collections.Generic;
using System.Text;

namespace Laba5
{
    interface IShowInfo:IComparable<IShowInfo>
    {
        void ShowInfo();
        static ConsoleKey Choice { protected get; set; }
    }
}
