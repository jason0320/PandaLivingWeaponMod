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
class ThingOnCreatePatch
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Thing), nameof(Thing.OnCreate))]
    internal static void Postfix(ref Thing __instance)
    {
        if ((__instance.rarity == Rarity.Legendary || __instance.rarity == Rarity.Mythical) && (__instance.IsMeleeWeapon || __instance.IsRangedWeapon || __instance.IsThrownWeapon))
        {
            int livingGenRarity = Mod_PandaLivingWeaponMod.livingGenRarity.Value;
            if (livingGenRarity < 1)
            {
                livingGenRarity = 20;
            }
            if (Rand.rnd(livingGenRarity) == 0)
            {
                List<SourceElement.Row> list = new List<SourceElement.Row>();
                foreach (SourceElement.Row row in EClass.sources.elements.rows)
                {
                    if (row.alias.Contains("living"))
                    {
                        list.Add(row);
                    }
                }
                foreach (SourceElement.Row item2 in list)
                {
                    Tuple<SourceElement.Row, int> enchant = new Tuple<SourceElement.Row, int>(item2, 1);
                    __instance.elements.ModBase(enchant.Item1.id, enchant.Item2);
                }
            }
        }
    }
}