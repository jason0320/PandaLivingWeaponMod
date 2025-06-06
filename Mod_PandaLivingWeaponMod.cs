﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace PandaLivingWeaponMod
{
    [BepInPlugin("panda.livingweapon.mod", "Panda's Living Weapon Mod", "1.0.0.0")]
    public class Mod_PandaLivingWeaponMod : BaseUnityPlugin
    {
        public static ConfigEntry<bool> allowInvokes;
        public static ConfigEntry<bool> allowAbsorbs;
        public static ConfigEntry<bool> allowVital;
        public static ConfigEntry<bool> allowDefence;
        public static ConfigEntry<bool> allowOffence;
        public static ConfigEntry<bool> allowProtection;
        public static ConfigEntry<bool> globalExp;
        public static ConfigEntry<int> maxEnchRoll;
        public static ConfigEntry<int> livingGenRarity;
        private void Start()
        {
            allowInvokes = Config.Bind("General", "allowInvokes", false, "Allow spell trigger enchants spawn in selector");
            allowAbsorbs = Config.Bind("General", "allowAbsorbs", false, "Allow abosrb life/mana/stamina enchants spawn in selector");
            allowVital = Config.Bind("General", "allowVital", false, "Allow life/mana/stamina score enchants spawn in selector");
            allowDefence = Config.Bind("General", "allowDefence", false, "Allow DV/PV/FPV enchants spawn in selector");
            allowOffence = Config.Bind("General", "allowOffence", false, "Allow critical/vorpal/penetration enchants spawn in selector");
            allowProtection = Config.Bind("General", "allowProtection", false, "Allow Physical Damage Reduction/Elemental Damage Reduction/Perfect Evasion enchants spawn in selector");
            globalExp = Config.Bind("General", "globalExp", false, "Allow all equipped living items gain exp from enemy death");
            maxEnchRoll = Config.Bind("General", "maxEnchRoll", 10, "Set the maximum number of enchants in selector");
            livingGenRarity = Config.Bind("General", "livingGenRarity", 20, "Set the rarity of living weapon, 1 in x chance");
            var harmony = new Harmony("Panda's Living Weapon Mod");
            harmony.PatchAll();
        }
    }
}
