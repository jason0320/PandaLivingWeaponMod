using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PandaLivingWeaponMod;

public class PatchContextMenu
{
    public static void UIContextMenu_Show(UIContextMenu __instance)
    {
        if (EClass.scene.mouseTarget == null || !(__instance.name == "ContextInteraction(Clone)"))
        {
            return;
        }
        Card card = EClass.scene.mouseTarget.card;
        Thing thing = card as Thing;
        if (thing != null)
        {
            int ele = 653;
            Element element = thing.elements.GetElement(ele);
            if (thing.HasElement(ele))
            {
                if (element.vExp >= element.ExpToNext)
                {
                    EClass.ui.contextMenu?.currentMenu.AddButton(HPUI.__("成長", "Growth"), delegate
                    {
                        HPUI.OpenWidget<HPWidgetThing>(thing);
                        __instance.Hide();
                    });
                }
            }
        }
    }

    public static void InvOwner_ShowContextMenu(InvOwner __instance, ButtonGrid button)
    {
        if ((Object)(object)button == null || button.card == null || button.card.Thing == null)
        {
            return;
        }
        Thing thing = button.card.Thing;
        UIContextMenu menu = EClass.ui.contextMenu?.currentMenu ?? EClass.ui.CreateContextMenuInteraction();
        if (thing != null)
        {
            int ele = 653;
            Element element = thing.elements.GetElement(ele);
            if (thing.HasElement(ele))
            {
                if (element.vExp >= element.ExpToNext)
                {
                    if (!(menu == null))
                    {
                        menu.AddButton(HPUI.__("成長", "Growth"), delegate
                        {
                            HPUI.OpenWidget<HPWidgetThing>(thing);
                            menu?.Hide();
                        });
                        menu.Show();
                    }
                }
            }
        }
    }
}
