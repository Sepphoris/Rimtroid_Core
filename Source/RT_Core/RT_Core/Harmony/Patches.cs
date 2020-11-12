﻿using RimWorld;
using Verse;
using Verse.AI;
using HarmonyLib;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace RT_Core
{
    [StaticConstructorOnStartup]
    class Main
    {
        static Main()
        {
            var harmony = new Harmony("com.drazzii.rimworld.mod.RimtroidCore");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            DefDatabase<ThingDef>.GetNamed("RT_BanteeMetroid").race.predator = true;
            DefDatabase<ThingDef>.GetNamed("RT_MetroidLarvae").race.predator = true;
            DefDatabase<ThingDef>.GetNamed("RT_AlphaMetroid").race.predator = true;
            DefDatabase<ThingDef>.GetNamed("RT_GammaMetroid").race.predator = true;
            DefDatabase<ThingDef>.GetNamed("RT_ZetaMetroid").race.predator = true;
            DefDatabase<ThingDef>.GetNamed("RT_OmegaMetroid").race.predator = true;
            DefDatabase<ThingDef>.GetNamed("RT_QueenMetroid").race.predator = true;
        }
    }

    public class RT_DesiccatorExt : DefModExtension
    {

        public ThingDef RT_DesiccatedDef;
    }


    [HarmonyPatch(typeof(Need_Food), "NeedInterval")]
    public static class RT_NeedInterval_Patch
    {
        public static float GetBerserkChance(float curFoodLevel, Dictionary<float, float> hungerValues)
        {
            var keys = hungerValues.Keys.OrderByDescending(x => x);
            float result = 0;
            foreach (var key in keys)
            {
                if (key >= curFoodLevel)
                {
                    result = hungerValues[key];
                }
            }
            return result;
        }
        public static void Postfix(Need_Food __instance, Pawn ___pawn)
        {
            var options = ___pawn.kindDef.GetModExtension<HungerBerserkOptions>();
            if (options != null)
            {
                var berserkChance = GetBerserkChance(__instance.CurLevelPercentage, options.hungerBerserkChanges);
                //Log.Message(___pawn + " has " + berserkChance + " berserk chance, cur food level: " + __instance.CurLevelPercentage, true);
                if (berserkChance > 0)
                {
                    if (!___pawn.InMentalState && Rand.Chance(berserkChance))
                    {
                        //Log.Message(___pawn + " gets berserk state", true);
                        if (___pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Berserk, null, forceWake: true))
                        {
                            if (___pawn.Faction == Faction.OfPlayer && Rand.Chance(options.chanceToBecomeWildIfBerserkAndTamed))
                            {
                                ___pawn.SetFaction(null);
                            }
                        }
                    }
                }
                else if (___pawn.mindState.mentalStateHandler.CurStateDef == MentalStateDefOf.Berserk)
                {
                    //Log.Message(___pawn + " recovers from berserk state", true);
                    ___pawn.MentalState.RecoverFromState();
                }
            }
        }
    }

    //[HarmonyPatch(typeof(Pawn), "Kill")]
    //public static class RT_Desiccator_Pawn_Kill_Patch
    //{
        //[HarmonyPostfix]
        //public static void Postfix(Pawn __instance, DamageInfo? dinfo, Hediff exactCulprit)
        //{
            //if (dinfo.HasValue)
            //{
                //if (dinfo.Value.Instigator != null)
                //{
                    //Thing inst = dinfo.Value.Instigator;
                    //RT_DesiccatorExt desiccator = inst.def.GetModExtension<RT_DesiccatorExt>();
                    //if (desiccator != null)
                    //{
                        //if (desiccator.RT_DesiccatedDef != null)
                        //{
                            //FieldInfo corpse = typeof(Pawn).GetField("Corpse", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);
                            //Traverse.Create(__instance);
                            //corpse.SetValue(__instance, ThingMaker.MakeThing(desiccator.RT_DesiccatedDef));
                        //}
                        //else
                        //{
                            //CompRottable compRottable = __instance.Corpse.TryGetComp<CompRottable>();
                            //compRottable.RotImmediately();
                       // }
                    //}
                //}
            //}
            //HediffDef def = DefDatabase<HediffDef>.GetNamed("RT_LifeDrainSickness");
            //if (__instance.health.hediffSet.HasHediff(def))
            //{
                //CompRottable compRottable = __instance.Corpse.TryGetComp<CompRottable>();
                //compRottable.RotImmediately();
            //}
            //if (__instance.Corpse.GetRotStage() == RotStage.Fresh)
            //{
                //Log.Message(__instance + " failed rot");
            //}
            /*
            else
            {
                Log.Message(__instance + " rotted by");
            }
            */
        //}
    //}
}