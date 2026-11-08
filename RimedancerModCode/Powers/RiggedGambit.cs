using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using RimedancerMod.RimedancerModCode.Orbs;
using RimedancerMod.RimedancerModCode.Powers;

namespace RimedancerMod.RimedancerModCode.Powers;

public class RiggedGambit() : RimedancerModPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override async Task BeforeDamageReceived(
        PlayerChoiceContext choiceContext,
        Creature target,
        Decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        if (target != this.Owner || dealer == null || !props.IsPoweredAttack() && !(cardSource is Omnislice))
            return;
        this.Flash();
        if (Owner.IsPlayer)
        {
            IcicleTargeting.LastTargeted[Owner.Player.PlayerCombatState] = dealer;
            decimal HitCounter = amount;
            foreach (var orb in Owner.Player.PlayerCombatState.OrbQueue.Orbs)
            {
                if (orb is Icicle && HitCounter > 0)
                {
                    await ((Icicle)orb).ApplyIcicleDamage(orb.PassiveVal,IcicleTargeting.LastTargeted[Owner.Player.PlayerCombatState],choiceContext);
                    HitCounter--;
                }
                if (HitCounter == 0) break;
            }
        }
    }
}