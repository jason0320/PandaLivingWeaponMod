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
                        menu.AddButton("成長"._("Growth"), (Action)delegate
                        {
                            HP.CreateLayer<LayerThingEditor, Thing>(thing);
                            UIContextMenu obj = menu;
                            if (obj != null)
                            {
                                obj.Hide();
                            }
                        }, true);
                        menu.Show();
                    }
                }
            }
        }
    }
}
