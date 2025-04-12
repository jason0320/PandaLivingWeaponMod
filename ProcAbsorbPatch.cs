using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

[HarmonyPatch]
class ProcAbsorbPatch
{
    static MethodBase TargetMethod()
    {
        var outerType = typeof(Card);
        var nestedTypes = outerType.GetNestedTypes(BindingFlags.NonPublic);

        foreach (var nested in nestedTypes)
        {
            var methods = nested.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            foreach (var m in methods)
            {
                if (m.Name.Contains("ProcAbsorb"))
                {
                    return m;
                }
            }
        }

        return null;
    }
    [HarmonyPostfix]
    public static void Postfix(object __instance)
    {
        var traverse = Traverse.Create(__instance);

        // Get fields using ACTUAL names from debug log
        Card origin = traverse.Field("origin").GetValue<Card>();
        AttackSource attackSource = traverse.Field("attackSource").GetValue<AttackSource>();
        int dmg = traverse.Field("dmg").GetValue<int>();
        Thing weapon = traverse.Field("weapon").GetValue<Thing>(); // Field is actually a Thing
        Card defender = traverse.Field("<>4__this").GetValue<Card>(); // Critical fix: Use compiler-generated name

        if (origin != null && defender != null && origin.isChara && defender.isChara)
        {

            int valueOrDefault3 = origin.Evalue(660) + (weapon?.Evalue(660, ignoreGlobalElement: true) ?? 0);

            if (valueOrDefault3 > 0 && attackSource == AttackSource.Melee)
            {
                int num16 = EClass.rnd(2 + Mathf.Clamp(dmg / 10, 0, valueOrDefault3 + 10));

                // Heal attacker
                origin.Chara.hp += num16;

                // Damage defender
                if (defender.IsAliveInCurrentZone) // Use defender's property
                {
                    defender.Chara.hp -= num16;
                }

            }
        }
    }

}