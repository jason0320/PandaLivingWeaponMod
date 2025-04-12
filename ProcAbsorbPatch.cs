using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;
using PandaLivingWeaponMod;
using System.Linq;
using System.IO;
using System.Text;

[HarmonyPatch]
class ProcAbsorbPatch
{
    internal static MethodBase TargetMethod()
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
    internal static void Postfix(object __instance)
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
    [HarmonyTranspiler]
    internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var codes = new List<CodeInstruction>(instructions);

        // Target sequence from your IL dump:
        // IL_0116: callvirt  instance class Chara Card::get_Chara()
        // IL_011B: ldfld     bool Chara::ignoreSPAbsorb
        // IL_0120: brtrue.s  IL_0197
        for (int i = 0; i < codes.Count - 2; i++)
        {
            if (codes[i].opcode == OpCodes.Callvirt &&
                codes[i].operand.ToString().Contains("get_Chara()") &&
                codes[i + 1].opcode == OpCodes.Ldfld &&
                codes[i + 1].operand.ToString().Contains("ignoreSPAbsorb") &&
                codes[i + 2].opcode == OpCodes.Brtrue)
            {
                // 1. Remove Card instance from stack (originally consumed by get_Chara)
                codes[i] = new CodeInstruction(OpCodes.Pop);

                // 2. Load false instead of checking the field
                codes[i + 1] = new CodeInstruction(OpCodes.Ldc_I4_0);

                return codes;
            }
        }
        return codes;
    }
}