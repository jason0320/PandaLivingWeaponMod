using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using PandaLivingWeaponMod;
using static HotItemLayout;
using System.Reflection.Emit;
using static BiomeProfile.Cluster;
using System.ComponentModel.Design;
using System.ComponentModel;
using UnityEngine.Windows;
using System.Globalization;
using HarmonyLib;

public class HPWidgetThing : HPWidget
{
    public class TabThingEnchant : HPLayout<Thing>
    {
        private SourceElement.Row[] enchantList;

        private Thing? thing;

        private GridLayout? grid;

        public TabThingEnchant()
        {
            bool allowInvokes = Mod_PandaLivingWeaponMod.allowInvokes.Value;
            if (allowInvokes)
            {
                enchantList = EClass.sources.elements.rows.Where((SourceElement.Row e) =>
                    !(e.alias.Contains("mod_")) &&
                    !(e.alias == ("living")
                    || e.alias == ("r_life")
                    || e.alias == ("r_mana")
                    || e.alias == ("r_DV")
                    || e.alias == ("r_PV")
                    || e.alias == ("searchRange")
                    || e.alias == ("expMod")
                    || e.alias == ("weightMod")
                    || e.alias == ("slowDecay")
                    || e.alias == ("corruption")
                    || e.alias == ("piety")
                    || e.alias == ("critical")
                    || e.alias == ("vopal")
                    || e.alias == ("penetration")
                    || e.alias == ("force_weapon")
                    || e.alias == ("SpTeleport")
                    || e.alias == ("SpTeleportShort")
                    || e.alias == ("SpReturn")
                    || e.alias == ("SpEvac")
                    || e.alias == ("SpIdentify")
                    || e.alias == ("SpIdentifyG")
                    || e.alias == ("SpUncurse")
                    || e.alias == ("SpUncurseG")
                    || e.alias == ("SpEnchantWeapon")
                    || e.alias == ("SpEnchantWeaponGreat")
                    || e.alias == ("SpEnchantArmor")
                    || e.alias == ("SpEnchantArmorGreat")
                    || e.alias == ("SpMagicMap")
                    || e.alias == ("SpLighten")
                    || e.alias == ("SpFaith")
                    || e.alias == ("SpChangeMaterialLesser")
                    || e.alias == ("SpChangeMaterial")
                    || e.alias == ("SpChangeMaterialG")
                    || e.alias == ("SpReconstruction")
                    || e.alias == ("SpLevitate")
                    || e.alias == ("SpMutation")
                    || e.alias == ("SpWish")
                    || e.alias == ("SpRevive")
                    || e.alias == ("SpRestoreBody")
                    || e.alias == ("SpRestoreMind")
                    || e.alias == ("SpRemoveHex")
                    || e.alias == ("SpVanishHex")
                    || e.alias == ("SpTransmuteBroom")
                    || e.alias == ("SpTransmutePutit")
                    || e.alias == ("SpExterminate")
                    || e.alias == ("SpShutterHex")
                    || e.alias == ("SpWardMonster")
                    || e.alias == ("SpDrawMonster")
                    || e.alias == ("SpDrawMetal")
                    || e.alias == ("SpDrawBacker")
                    || e.alias == ("breathe_")
                    || e.alias == ("ball_")
                    || e.alias == ("bolt_")
                    || e.alias == ("hand_")
                    || e.alias == ("arrow_")
                    || e.alias == ("funnel_")
                    || e.alias == ("miasma_")
                    || e.alias == ("weapon_")
                    || e.alias == ("puddle_")
                    || e.alias == ("noDamage")
                    || e.alias == ("onlyPet")
                    || e.alias == ("permaCurse")
                    || e.alias == ("absorbHP")
                    || e.alias == ("absorbMP")
                    || e.alias == ("absorbSP")
                    || e.alias == ("eheluck")
                    || e.alias == ("boostMachine")
                    || e.alias == ("meleeDistance")
                    || e.alias == ("throwReturn")
                    || e.alias == ("PDR")
                    || e.alias == ("EDR")
                    || e.alias == ("evasionPerfect")
                    || e.alias == ("life")
                    || e.alias == ("mana")
                    || e.alias == ("vigor")
                    || e.alias == ("FPV")
                    || e.alias == ("DV")
                    || e.alias == ("PV")) &&
                    (e.category == "attribute" || e.category == "skill" || e.category == "enchant" || e.category == "resist" || (e.category == "ability" && e.group == "SPELL"))).ToArray();
            }
            else
            {
                enchantList = EClass.sources.elements.rows.Where((SourceElement.Row e) =>
                    !(e.alias.Contains("mod_")) &&
                    !(e.alias == ("living")
                    || e.alias == ("r_life")
                    || e.alias == ("r_mana")
                    || e.alias == ("r_DV")
                    || e.alias == ("r_PV")
                    || e.alias == ("searchRange")
                    || e.alias == ("expMod")
                    || e.alias == ("weightMod")
                    || e.alias == ("slowDecay")
                    || e.alias == ("corruption")
                    || e.alias == ("piety")
                    || e.alias == ("critical")
                    || e.alias == ("vopal")
                    || e.alias == ("penetration")
                    || e.alias == ("force_weapon")
                    || e.alias == ("SpTeleport")
                    || e.alias == ("SpTeleportShort")
                    || e.alias == ("SpReturn")
                    || e.alias == ("SpEvac")
                    || e.alias == ("SpIdentify")
                    || e.alias == ("SpIdentifyG")
                    || e.alias == ("SpUncurse")
                    || e.alias == ("SpUncurseG")
                    || e.alias == ("SpEnchantWeapon")
                    || e.alias == ("SpEnchantWeaponGreat")
                    || e.alias == ("SpEnchantArmor")
                    || e.alias == ("SpEnchantArmorGreat")
                    || e.alias == ("SpMagicMap")
                    || e.alias == ("SpLighten")
                    || e.alias == ("SpFaith")
                    || e.alias == ("SpChangeMaterialLesser")
                    || e.alias == ("SpChangeMaterial")
                    || e.alias == ("SpChangeMaterialG")
                    || e.alias == ("SpReconstruction")
                    || e.alias == ("SpLevitate")
                    || e.alias == ("SpMutation")
                    || e.alias == ("SpWish")
                    || e.alias == ("SpRevive")
                    || e.alias == ("SpRestoreBody")
                    || e.alias == ("SpRestoreMind")
                    || e.alias == ("SpRemoveHex")
                    || e.alias == ("SpVanishHex")
                    || e.alias == ("SpTransmuteBroom")
                    || e.alias == ("SpTransmutePutit")
                    || e.alias == ("SpExterminate")
                    || e.alias == ("SpShutterHex")
                    || e.alias == ("SpWardMonster")
                    || e.alias == ("SpDrawMonster")
                    || e.alias == ("SpDrawMetal")
                    || e.alias == ("SpDrawBacker")
                    || e.alias == ("breathe_")
                    || e.alias == ("ball_")
                    || e.alias == ("bolt_")
                    || e.alias == ("hand_")
                    || e.alias == ("arrow_")
                    || e.alias == ("funnel_")
                    || e.alias == ("miasma_")
                    || e.alias == ("weapon_")
                    || e.alias == ("puddle_")
                    || e.alias == ("noDamage")
                    || e.alias == ("onlyPet")
                    || e.alias == ("permaCurse")
                    || e.alias == ("absorbHP")
                    || e.alias == ("absorbMP")
                    || e.alias == ("absorbSP")
                    || e.alias == ("eheluck")
                    || e.alias == ("boostMachine")
                    || e.alias == ("meleeDistance")
                    || e.alias == ("throwReturn")
                    || e.alias == ("PDR")
                    || e.alias == ("EDR")
                    || e.alias == ("evasionPerfect")
                    || e.alias == ("life")
                    || e.alias == ("mana")
                    || e.alias == ("vigor")
                    || e.alias == ("FPV")
                    || e.alias == ("DV")
                    || e.alias == ("PV")) &&
                    (e.category == "attribute" || e.category == "skill" || e.category == "enchant" || e.category == "resist")).ToArray();
            }
        }

        public override void OnCreate(Thing thing)
        {
            this.thing = thing;
            layout.childControlHeight = true;
            layout.childForceExpandHeight = true;
            HPLayout<Thing>.ScrollLayout scrollLayout = AddScrollLayout(layout.transform);
            scrollLayout.headerRect.SetActive(enable: false);
            SourceElement.Row[] array = enchantList;

            int arrayNum = 0;
            int maxEnchRoll = Mod_PandaLivingWeaponMod.maxEnchRoll.Value;
            if (maxEnchRoll < 1)
            {
                maxEnchRoll = 10;
            }

            System.Random r = new System.Random((int)DateTime.Now.Ticks);
            array = array.OrderBy(x => r.Next()).ToArray();

            foreach (SourceElement.Row row in array)
            {
                arrayNum++;
                if (arrayNum > maxEnchRoll)
                {
                    break;
                }

                string nameJP = row.name_JP;
                string name = row.name;
                TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
                if (row.alias.Contains("_") && row.aliasRef.Contains("ele"))
                {
                    foreach (SourceElement.Row row2 in array)
                    {
                        if (row.aliasRef == row2.alias) {
                            nameJP = row2.altname_JP.Split(',')[0].ToString().Trim() + "の" + nameJP;
                            name = myTI.ToTitleCase(row2.altname.Split(',')[0].ToString().Trim()) + " " + name;
                        }
                    }
                }



                int num = 653;
                Element element = thing.elements.GetElement(num);

                if (thing.HasElement(num))
                {
                    int lv = thing.elements.GetElement("living").vBase;
                    float num5 = (float)(3 + Mathf.Min(lv / 10, 15)) + Mathf.Sqrt(lv * thing.encLV / 100);
                    int v = EClass.rnd((int)num5) + 1;
                    nameJP = nameJP + "(" + v + ")";
                    name = name + "(" + v + ")";
                    AddButton(HPUI.__(nameJP, name), delegate
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
                            thing.elements.SetBase(row.id, v);
                            if (Lang.isJP)
                            {
                                Msg.SayRaw(thing.GetName(NameStyle.Full) + "は嬉しげに震えた。");
                            }
                            else
                            {
                                Msg.SayRaw(thing.GetName(NameStyle.Full) + " vibrates as if she is pleased.");
                            }
                        }
                        this.window.Close();
                    }, scrollLayout.root).gameObject.GetComponent<RectTransform>();
                }

            }
            
        }
    }

    public override HPWidget Setup(object arg)
    {
        if (!(arg is Thing thing))
        {
            return this;
        }
        Window window = this.AddWindow(new Window.Setting
        {
            textCaption = HPUI.__("メニュー", "Menu"),
            bound = new Rect(0f, 0f, 680f, 500f),
            transparent = false,
            allowMove = true
        });
        try
        {
            window.AddTab(HPUI.__("エンチャント", "Enchant"), HPUI.CreatePage<TabThingEnchant, Thing>("HP.tool.enchant", window, thing).root);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
        return this;
    }
}
