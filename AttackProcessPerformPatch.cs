using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using HarmonyLib;
using PandaLivingWeaponMod;
using UnityEngine;

[HarmonyPatch]
class AttackProcessPerformPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(AttackProcess), nameof(AttackProcess.Perform))]
    internal static void Postfix(ref AttackProcess __instance)
    {
        Thing w = __instance.weapon;
        if (w != null)
        {
            int ele = 653;
            Element element = w.elements.GetElement(ele);

            if (__instance.TC != null)
            {
                int targetLV = __instance.TC.LV;
                if (!__instance.TC.IsAliveInCurrentZone)
                {

                    if (w.HasElement(ele))
                    {
                        element.vExp = element.vExp + Rand.rnd(targetLV / (w.elements.GetElement("living").vBase)) + 1;
                        if (element.vExp >= element.ExpToNext)
                        {
                            element.vExp = element.ExpToNext;
                            if (Lang.isJP)
                            {
                                Msg.SayRaw(w.GetName(NameStyle.Full) + "は十分に血を吸い成長できる!");
                            }
                            else
                            {
                                Msg.SayRaw(w.GetName(NameStyle.Full) + " sucked enough blood and ready to grow!");
                            }
                        }
                    }

                }
            }
        }
    }
}