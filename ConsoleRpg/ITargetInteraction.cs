using System;
using System.Collections.Generic;
using System.Text;

namespace Laba5
{
    interface ITargetInteraction<Target>
    {
        void UniqueAbility(Target currentTarget);
        void DealDamage(Target currentTarget);
    }
}
