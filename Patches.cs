using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace PandaLivingWeaponMod
{
    [HarmonyPatch]
    public class PatchContextMenu
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(InvOwner), "ShowContextMenu")]
        public static void InvOwner_ShowContextMenu(InvOwner __instance, ButtonGrid button)
        {
            if (button == null || button.card == null || button.card.Thing == null)
            {
                return;
            }
            Thing thing = button.card.Thing;
            if (thing != null)
            {
                int ele = 653;
                Element element = thing.elements.GetElement(ele);
                if (thing.HasElement(ele))
                {
                    if (element.vExp >= element.ExpToNext)
                    {
                        UIContextMenu menu = EClass.ui.contextMenu?.currentMenu ?? EClass.ui.CreateContextMenuInteraction();
                        if (menu == null)
                        {
                            return;
                        }

                        String buttontext = "Growth";
                        if (Lang.isJP)
                        {
                            buttontext = "成長";
                        }
                        else if (Lang.langCode == "CN")
                        {
                            buttontext = "成长";
                        }
                        else
                        {
                            buttontext = "Growth";
                        }

                        menu.AddButton(buttontext._(buttontext), (Action)delegate
                        {
                            HP.CreateLayer<LayerThingEditor, Thing>(thing);
                            UIContextMenu obj = menu;
                            if (obj != null)
                            {
                                obj.Hide();
                            }
                        }, true);
                        if (Lang.isJP)
                        {
                            Msg.SayRaw(thing.GetName(NameStyle.Full) + "は十分に血を吸い成長できる！");
                            Msg.SayRaw("それは…");
                        }
                        else if (Lang.langCode == "CN")
                        {
                            Msg.SayRaw(thing.GetName(NameStyle.Full) + "已经吸取足够的血液，可以成长了！");
                            Msg.SayRaw("那是…");
                        }
                        else
                        {
                            Msg.SayRaw(thing.GetName(NameStyle.Full) + " sucked enough blood and ready to grow!");
                            Msg.SayRaw("It...");
                        }
                        menu.Show();
                    }
                }
            }
        }
    }
}
