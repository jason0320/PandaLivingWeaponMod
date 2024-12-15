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
            enchantList = EClass.sources.elements.rows.Where((SourceElement.Row e) => (e.encSlot == "weapon" || e.encSlot == "all") && (e.category == "attribute" || e.category == "skill" || e.category == "enchant" || e.category == "resist" || e.category == "ability")).ToArray();
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
            foreach (SourceElement.Row row in array)
            {
                if (EClass.rnd(20) == 0)
                {
                    arrayNum++;
                    if (arrayNum > 10)
                    {
                        break;
                    }
                }
                else
                {
                    continue;
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
