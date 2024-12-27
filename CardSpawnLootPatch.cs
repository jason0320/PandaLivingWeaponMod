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
class CardSpawnLootPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Card), nameof(Card.SpawnLoot))]
    internal static void Postfix(ref Card __instance)
    {
        bool globalExp = Mod_PandaLivingWeaponMod.globalExp.Value;
        if (globalExp)
        {
            if (!__instance.IsPC)
            {
                foreach (BodySlot slot in Act.CC.body.slots)
                {
                    Thing w = slot.thing;
                    if (w != null)
                    {
                        int ele = 653;
                        Element element = w.elements.GetElement(ele);
                        if (w.HasElement(ele))
                        {
                            element.vExp = element.vExp + Rand.rnd(__instance.LV / (w.elements.GetElement("living").vBase)) + 1;
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
    }