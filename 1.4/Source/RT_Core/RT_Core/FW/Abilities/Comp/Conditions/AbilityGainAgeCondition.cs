﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace RT_Core
{
    public class AbilityGainAgeCondition : AbilityGainCondition
    {
        public FloatRange ageRange;

        public override bool IsSatisfied(CompAbilityDefinition def)
        {
            return def.SelfPawn.ageTracker.AgeBiologicalYears >= ageRange.TrueMin;
        }

        public override bool IsFulfilled(CompAbilityDefinition def)
        {
            return def.SelfPawn.ageTracker.AgeBiologicalYears > ageRange.TrueMax;
        }
    }
}
