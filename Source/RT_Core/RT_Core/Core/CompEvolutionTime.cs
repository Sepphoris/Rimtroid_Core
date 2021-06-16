﻿using System;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RT_Rimtroid
{
	public class CompEvolutionTime : ThingComp
	{
		public int curEvolutionTryCount;
		public int nextEvolutionCheckTick;
		public CompProperties_EvolutionTime Props
		{
			get
			{
				return (CompProperties_EvolutionTime)this.props;
			}
		}
		public Pawn Metroid => this.parent as Pawn;

		public override string CompInspectStringExtra()
		{
			return "RT_TimeToEvolution".Translate((int)(Props.timeInYears - Metroid.ageTracker.AgeBiologicalYears));
		}

        public override void PostExposeData()
        {
            base.PostExposeData();
			Scribe_Values.Look(ref curEvolutionTryCount, "curEvolutionTryCount");
			Scribe_Values.Look(ref nextEvolutionCheckTick, "nextEvolutionCheckTick");
		}
    }
}