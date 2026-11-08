using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using RimedancerMod.RimedancerModCode.Powers;

namespace RimedancerMod.RimedancerModCode.Powers;
public class Rime() : RimedancerModPower
{
    public override PowerType Type =>
        PowerType.Debuff;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Block)];
    public override PowerStackType StackType =>
        PowerStackType.Counter;
    
    public override async Task AfterDamageReceived(
        PlayerChoiceContext choiceContext,
        Creature target,
        DamageResult result,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        Rime power = this;
        if (target != power.Owner || result.UnblockedDamage < 1 || props.IsPoweredAttack() || dealer == null)
            return;
        power.Flash();
        Decimal num = await CreatureCmd.GainBlock(dealer, Math.Max(power.Amount,1), ValueProp.Unpowered, null);
        await PowerCmd.ModifyAmount(choiceContext,(PowerModel) power,-1,power.Owner,cardSource);
    }
    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    { 
       return Task.CompletedTask;
    }
}