using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using PandaLivingWeaponMod;

[HarmonyPatch]
public class Patcher
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(UIContextMenu), "Show", new Type[] { })]
    public static void UIContextMenu_Show(UIContextMenu __instance)
    {
        PatchContextMenu.UIContextMenu_Show(__instance);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(InvOwner), "ShowContextMenu")]
    public static void InvOwner_ShowContextMenu(InvOwner __instance, ButtonGrid button)
    {
        PatchContextMenu.InvOwner_ShowContextMenu(__instance, button);
    }
}
