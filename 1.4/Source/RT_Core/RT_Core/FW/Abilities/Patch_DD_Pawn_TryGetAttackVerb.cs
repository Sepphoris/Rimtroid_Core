﻿using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace RT_Core
{
    [HarmonyPatch(typeof(Pawn), "TryGetAttackVerb")]
    public static class RT_Pawn_TryGetAttackVerb
    {
        public static void Postfix(Pawn __instance, ref Verb __result, Thing target, bool allowManualCastWeapons)
        {
            if (!__instance.kindDef.HasModExtension<VerbSettingExtension>() || !__instance.kindDef.GetModExtension<VerbSettingExtension>().useExtendedVerbs)
            {
                //Doesn't have the extension, or not set to use extended verbs.
                return;
            }

            IEnumerable<Verb> verbs = VerbUtils.GetPossibleVerbs(__instance);

            if (target != null)
            {
                //If given a target, keep only the verbs in range.
                verbs = verbs.Filter_KeepInRange(target);
            }
            else
            {
                //If not given a target, act as if we're asking about primary verbs.
                verbs = verbs.Filter_KeepRanged();
            }


            if (verbs.Any())
            {
                //Still has verbs.
                __result = verbs.Get_MostPreferred(target == null);
            }
        }
    }
}
