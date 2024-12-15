using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PandaLivingWeaponMod;

public interface IHPWidget<T> where T : ELayer
{
    T Setup(object arg);
}

