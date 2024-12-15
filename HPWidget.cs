using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PandaLivingWeaponMod;

public abstract class HPWidget : ELayer, IHPWidget<HPWidget>
{
    public abstract HPWidget Setup(object arg);

    private void LateUpdate()
    {
        foreach (Window window in windows)
        {
            window.cg.alpha = (window.setting.transparent ? window.Skin.transparency : 1f);
        }
    }
}
