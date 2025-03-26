using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PandaLivingWeaponMod
{
    public class LayerThingEditor : HPLayer<Thing>
    {
        public override void OnLayout()
        {
            CreateTab<ThingEnchantTab>("エンチャント"._("Enchant"), "hp.thing.enchant");
        }
    }

}
