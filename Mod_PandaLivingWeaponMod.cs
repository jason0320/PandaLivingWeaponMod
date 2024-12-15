using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using HarmonyLib;

namespace PandaLivingWeaponMod
{
    [BepInPlugin("panda.livingweapon.mod", "Panda's Living Weapon Mod", "1.0.0.0")]
    public class Mod_PandaLivingWeaponMod : BaseUnityPlugin
    {
        private void Start()
        {
            var harmony = new Harmony("Panda's Living Weapon Mod");
            harmony.PatchAll();
        }
    }
}
