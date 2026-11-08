using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace RimedancerMod.RimedancerModCode;

public class FervorVal
{
    public static readonly SpireField<OrbModel, int> FervorBonus = new(() => 0); //Default value of 0
    
    public class FervorBonusSingleton() : CustomSingletonModel(HookType.Combat)
    {
        public override Decimal ModifyOrbValue(OrbModel orb, Decimal value)
        {
            return value + FervorVal.FervorBonus[orb];
        }
    }
}