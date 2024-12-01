using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using HarmonyLib;
using UnityEngine;

[HarmonyPatch]
class CardSpawnLootPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Card), nameof(Card.SpawnLoot))]
    internal static void Postfix(ref Card __instance)
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
                        int num = element.vExp - element.ExpToNext;
                        int vBase = element.vBase;
                        w.elements.ModBase(ele, 1);
                        element.vExp = Mathf.Clamp(num / 2, 0, element.ExpToNext / 2);
                        w.AddEnchant(Rand.rnd(vBase + 10) + vBase);
                        Msg.SayRaw(w.GetName(NameStyle.Full) + " has sucked enough blood and grows!");
                    }
                }
            }
        }
    }
}