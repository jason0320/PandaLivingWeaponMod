using System;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace PandaLivingWeaponMod
{
    public class ThingEnchantTab : HPLayout<Thing>
    {
        public override void OnLayout()
        {
            SourceElement.Row[] enchantList;
            Thing thing = base.Layer.Data;
            Header(thing.GetName(NameStyle.Full));
            enchantList = EClass.sources.elements.rows.Where((SourceElement.Row e) => (e.category == "attribute" || e.category == "skill" || e.category == "enchant" || e.category == "resist" || (e.category == "ability" && (e.group == "SPELL" || e.type == "ActBreathe")))).ToArray();
            enchantList = enchantList.Where(e => !(e.aliasRef.Contains("mold"))).ToArray();

            string[] blacklist = { "_Void", "living", "r_life", "r_mana", "r_DV", "r_PV", "searchRange", "expMod", "weightMod", "slowDecay", "corruption", "resDecay", "resDamage", "resCurse", "piety", "critical", "vopal", "penetration", "force_weapon", "SpTeleport", "SpTeleportShort", "SpReturn", "SpEvac", "SpIdentify", "SpIdentifyG", "SpUncurse", "SpUncurseG", "SpEnchantWeapon", "SpEnchantWeaponGreat", "SpEnchantArmor", "SpEnchantArmorGreat", "SpMagicMap", "SpLighten", "SpFaith", "SpChangeMaterialLesser", "SpChangeMaterial", "SpChangeMaterialG", "SpReconstruction", "SpLevitate", "SpMutation", "SpWish", "SpRevive", "SpRestoreBody", "SpRestoreMind", "SpRemoveHex", "SpVanishHex", "SpTransmuteBroom", "SpTransmutePutit", "SpExterminate", "SpShutterHex", "SpWardMonster", "SpDrawMonster", "SpDrawMetal", "SpDrawBacker", "noDamage", "onlyPet", "permaCurse", "eheluck", "boostMachine", "meleeDistance", "throwReturn", "PDR", "EDR", "evasionPerfect", "life", "mana", "vigor", "FPV", "DV", "PV" };

            bool allowInvokes = Mod_PandaLivingWeaponMod.allowInvokes.Value;
            bool allowAbsorbs = Mod_PandaLivingWeaponMod.allowAbsorbs.Value;
            bool allowVital = Mod_PandaLivingWeaponMod.allowVital.Value;
            bool allowDefence = Mod_PandaLivingWeaponMod.allowDefence.Value;

            if (!allowInvokes)
            {
                enchantList = enchantList.Where(e => !(e.category == "ability" && (e.group == "SPELL" || e.type == "ActBreathe"))).ToArray();
            }
            if (!allowAbsorbs) {
                blacklist.Append("absorbHP");
                blacklist.Append("absorbMP");
                blacklist.Append("absorbSP");
            }
            if (!allowVital)
            {
                blacklist.Append("life");
                blacklist.Append("mana");
                blacklist.Append("vigor");
            }
            if (!allowDefence)
            {
                blacklist.Append("DV");
                blacklist.Append("PV");
                blacklist.Append("FPV");
            }

            enchantList = enchantList.Where(e => !blacklist.Contains(e.alias)).ToArray();

            int maxEnchRoll = Mod_PandaLivingWeaponMod.maxEnchRoll.Value;
            if (maxEnchRoll < 1)
            {
                maxEnchRoll = 10;
            }

            System.Random rng = new System.Random();
            SourceElement.Row[] array = enchantList
                .OrderBy(x => rng.Next())
                .Take(maxEnchRoll)
                .ToArray();

            foreach (SourceElement.Row row in array)
            {
                string nameJP = row.name_JP;
                string nameEN = row.name;
                if (row.alias.Contains("_") && !row.aliasRef.Contains("mold"))
                {
                    foreach (SourceElement.Row row2 in enchantList)
                    {
                        if (row.aliasRef.Trim() == row2.alias.Trim())
                        {
                            nameJP = row2.name_JP.Trim() + "の" + nameJP;
                            nameEN = row2.name.Trim() + " " + nameEN;
                        }
                    }
                }
                int num = 653;
                Element element = thing.elements.GetElement(num);
                int lv = thing.elements.GetElement("living").vBase;
                float num5 = (float)(3 + Mathf.Min(lv / 10, 15)) + Mathf.Sqrt(lv * thing.encLV / 100);
                int v = EClass.rnd((int)num5) + 1;
                nameJP = nameJP + "(" + v + ")";
                nameEN = nameEN + "(" + v + ")";
                Button(nameJP._(nameEN), delegate
                {
                    if (element.vExp >= element.ExpToNext)
                    {
                        int num2 = element.vExp - element.ExpToNext;
                        int vBase = element.vBase;
                        thing.elements.ModBase(num, 1);
                        element.vExp = Mathf.Clamp(num2 / 2, 0, element.ExpToNext / 2);
                        if (thing.elements.GetOrCreateElement(row.id).ValueWithoutLink == 0)
                        {
                            thing.elements.ModBase(row.id, 1);
                        }
                        thing.elements.ModBase(row.id, v);
                        if (Lang.isJP)
                        {
                            Msg.SayRaw(thing.GetName(NameStyle.Full) + "は嬉しげに震えた。");
                        }
                        else
                        {
                            Msg.SayRaw(thing.GetName(NameStyle.Full) + " vibrates as if she is pleased.");
                        }
                    }
                    this._layer.Close();
                });
            }
        }
    }
}
