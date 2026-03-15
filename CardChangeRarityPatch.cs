using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;


namespace PandaLivingWeaponMod
{
    [HarmonyPatch(typeof(Card), nameof(Card.ChangeRarity))]
    public class CardChangeRarityPatch
    {
        public static void Postfix(Card __instance, Rarity q)
        {
            // 1. Only act if the rarity is being set to Legendary
            if (q != Rarity.Legendary) return;

            // 2. Check the call stack to see if TaskHarvest is the caller
            // We look back a few frames to find the TaskHarvest delegate
            StackTrace stackTrace = new StackTrace();
            bool isFromHarvest = false;

            for (int i = 0; i < stackTrace.FrameCount; i++)
            {
                var method = stackTrace.GetFrame(i).GetMethod();
                if (method.DeclaringType != null && method.DeclaringType.FullName.Contains("TaskHarvest"))
                {
                    isFromHarvest = true;
                    break;
                }
            }

            if (isFromHarvest)
            {
                __instance.elements.ModBase(653, 1);
            }
        }
    }
}
