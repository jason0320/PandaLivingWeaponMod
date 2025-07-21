using System;
using System.Globalization;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace PandaLivingWeaponMod
{
    public class ThingEnchantTab : HPLayout<Thing>
    {
        Thing thing;
        bool buttonClicked = false;
        public override void OnLayout()
        {
            SourceElement.Row[] enchantList;
            thing = base.Layer.Data;
            Header(thing.GetName(NameStyle.Full));
            enchantList = EClass.sources.elements.rows.Where((SourceElement.Row e) => (e.category == "attribute" || e.category == "skill" || e.category == "enchant" || e.category == "resist" || (e.category == "ability" && (e.group == "SPELL" || e.type == "ActBreathe")))).ToArray();
            enchantList = enchantList.Where(e => !(e.aliasRef.Contains("mold"))).ToArray();

            string[] blacklist = { "ball_Void", "breathe_Void", "bolt_Void", "hand_Void", "arrow_Void", "funnel_Void", "miasma_Void", "weapon_Void", "puddle_Void", "sword_Void", "eleVoid", "living", "r_life", "r_mana", "r_DV", "r_PV", "searchRange", "expMod", "weightMod", "slowDecay", "corruption", "resDecay", "resDamage", "resCurse", "piety", "critical", "SpTeleportShort", "SpReturn", "SpEvac", "SpIdentify", "SpIdentifyG", "SpUncurse", "SpUncurseG", "SpEnchantWeapon", "SpEnchantWeaponGreat", "SpEnchantArmor", "SpEnchantArmorGreat", "SpMagicMap", "SpLighten", "SpFaith", "SpChangeMaterialLesser", "SpChangeMaterial", "SpChangeMaterialG", "SpReconstruction", "SpLevitate", "SpMutation", "SpWish", "SpRevive", "SpRestoreBody", "SpRestoreMind", "SpRemoveHex", "SpVanishHex", "SpTransmuteBroom", "SpTransmutePutit", "SpExterminate", "SpShutterHex", "SpWardMonster", "SpDrawMonster", "SpDrawMetal", "SpDrawBacker", "noDamage", "onlyPet", "permaCurse", "meleeDistance", "throwReturn" };

            bool allowInvokes = Mod_PandaLivingWeaponMod.allowInvokes.Value;
            bool allowAbsorbs = Mod_PandaLivingWeaponMod.allowAbsorbs.Value;
            bool allowVital = Mod_PandaLivingWeaponMod.allowVital.Value;
            bool allowDefence = Mod_PandaLivingWeaponMod.allowDefence.Value;
            bool allowOffence = Mod_PandaLivingWeaponMod.allowOffence.Value;
            bool allowProtection = Mod_PandaLivingWeaponMod.allowProtection.Value;

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
            if (!allowOffence)
            {
                blacklist.Append("critical");
                blacklist.Append("vopal");
                blacklist.Append("penetration");
            }
            if (!allowProtection)
            {
                blacklist.Append("PDR");
                blacklist.Append("EDR");
                blacklist.Append("evasionPerfect");
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
                int num = 653;
                Element element = thing.elements.GetElement(num);
                int lv = thing.elements.GetElement("living").vBase;
                float num5 = (float)(3 + Mathf.Min(lv / 10, 15)) + Mathf.Sqrt(lv * thing.encLV / 100);
                int v = EClass.rnd((int)num5) + 1;

                if (row.alias.Contains("_") && !row.aliasRef.Contains("mold") && row.category.Contains("ability"))
                {
                    foreach (SourceElement.Row row2 in enchantList)
                    {
                        if (row.aliasRef.Trim() == row2.alias.Trim())
                        {
                            nameJP = row2.name_JP.Trim() + "の" + nameJP + " 能力発動" + v + "を獲得する";
                            nameEN = "Add " + row2.name.Trim() + " " + nameEN + " Spell Trigger by " + v;
                        }
                    }
                }
                else if (row.type.Contains("Resistance") && row.group.Contains("SKILL") && row.category.Contains("resist"))
                {
                    nameJP = nameJP + v + "獲得する";
                    nameEN = "Add " + nameEN + " by " + v;
                }
                else if (row.type.Contains("Skill") && row.group.Contains("SKILL") && row.category.Contains("enchant") && row.categorySub.Contains("eleAttack"))
                {
                    nameJP = nameJP + "属性追加ダメージ" + v + "を獲得する";
                    nameEN = "Add " + nameEN + " Damage by " + v;
                }
                else if (row.type.Contains("AttbMain") && row.group.Contains("SKILL") && row.category.Contains("attribute"))
                {
                    nameJP = nameJP + v + "上昇を獲得する";
                    nameEN = "Increase " + nameEN + " by " + v;
                }
                else if (row.type.Contains("Skill") && row.group.Contains("SKILL") && row.category.Contains("skill"))
                {
                    nameJP = nameJP + "スキル上昇" + v + "を獲得する";
                    nameEN = "Add " + nameEN + " Skill Bonus by " + v;
                }
                else if (row.type.Contains("Skill") && row.group.Contains("ENC") && row.category.Contains("enchant"))
                {
                    nameJP = nameJP + v + "を獲得する";
                    nameEN = "Add " + nameEN + " by " + v;
                }
                else
                {
                    nameJP = nameJP + v + "を獲得する";
                    nameEN = "Add " + nameEN + " by " + v;
                }

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
                            Msg.SayRaw(nameJP);
                        }
                        else
                        {
                            Msg.SayRaw(nameEN);
                        }
                    }
                    if (Lang.isJP)
                    {
                        Msg.SayRaw(thing.GetName(NameStyle.Full) + "は嬉しげに震えた。");
                    }
                    else
                    {
                        Msg.SayRaw(thing.GetName(NameStyle.Full) + " vibrates as if it is pleased.");
                    }
                    buttonClicked = true;
                    this._layer.Close();
                });
            }
        }
        void OnDestroy()
        {
            if (!buttonClicked)
            {
                if (Lang.isJP)
                {
                    Msg.SayRaw(thing.GetName(NameStyle.Full) + "は不満そうに震えた。");
                }
                else
                {
                    Msg.SayRaw(thing.GetName(NameStyle.Full) + " vibrates as if it is displeased.");
                }
            }
        }
        /*
        void OnDisable()
        {
            if (!buttonClicked)
            {
                if (Lang.isJP)
                {
                    Msg.SayRaw(thing.GetName(NameStyle.Full) + "は不満そうに震えた。");
                }
                else
                {
                    Msg.SayRaw(thing.GetName(NameStyle.Full) + " vibrates as if it is displeased.");
                }
            }
        }
        */
    }
}
