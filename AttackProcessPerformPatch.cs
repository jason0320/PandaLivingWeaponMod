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
                                Msg.SayRaw(w.GetName(NameStyle.Full) + "は十分に血を味わった！");
                            }
                            else
                            {
                                Msg.SayRaw(w.GetName(NameStyle.Full) + " has tasted enough blood!");
                            }
                        }
                    }
                    Card tc = __instance.TC;
                    bool globalExp = Mod_PandaLivingWeaponMod.globalExp.Value;
                    if (globalExp)
                    {
                        if (!tc.IsPC)
                        {
                            foreach (BodySlot slot in Act.CC.body.slots)
                            {
                                Thing w1 = slot.thing;
                                if (w1 != null)
                                {
                                    int ele1 = 653;
                                    Element element1 = w1.elements.GetElement(ele1);
                                    if (w1.HasElement(ele1))
                                    {
                                        element1.vExp = element1.vExp + Rand.rnd(tc.LV / (w1.elements.GetElement("living").vBase)) + 1;
                                        if (element1.vExp >= element1.ExpToNext)
                                        {
                                            element1.vExp = element1.ExpToNext;
                                            if (Lang.isJP)
                                            {
                                                Msg.SayRaw(w1.GetName(NameStyle.Full) + "は十分に血を味わった！");
                                            }
                                            else
                                            {
                                                Msg.SayRaw(w1.GetName(NameStyle.Full) + " has tasted enough blood!");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}