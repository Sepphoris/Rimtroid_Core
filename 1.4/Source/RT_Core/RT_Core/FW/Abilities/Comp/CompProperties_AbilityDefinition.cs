﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace RT_Core
{
    public class CompProperties_AbilityDefinition : CompProperties
    {
        public SimpleCurve spawnKillCount, spawnDamageTotal;
        public List<AbilityGainEntry> abilities;
    }
}
