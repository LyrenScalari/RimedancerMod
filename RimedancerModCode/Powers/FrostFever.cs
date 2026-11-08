using BaseLib.Hooks;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using RimedancerMod.RimedancerModCode.Powers;

namespace RimedancerMod.RimedancerModCode.Powers;

public class FrostFever() : RimedancerModPower
{
    public override PowerType Type =>
        PowerType.Debuff;
    public override PowerStackType StackType =>
        PowerStackType.Counter;
    protected override IEnumerable<DynamicVar> CanonicalVars
    {
        get
        {
            return [new DynamicVar("LifeLoss", 0)];
        }
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (!participants.Contains<Creature>(this.Owner))
            return;
        await this.Trigger();
    }
    public async Task Trigger()
    {
            IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Owner, DynamicVars["LifeLoss"].BaseValue, ValueProp.Unblockable | ValueProp.Unpowered, (CardModel) null, (CardPlay) null);
            if (this.Owner.IsAlive)
            {
                PowerModel powertarget = this;
                foreach (var power in Owner.Powers)
                {
                    if (power.Type == PowerType.Debuff && power.Amount > (powertarget.Amount))
                    {
                        powertarget = power;
                    }
                }
                await PowerCmd.Decrement(powertarget);
            }
            else
                await Cmd.CustomScaledWait(0.1f, 0.25f);
    }
    public override Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier,
        CardModel? cardSource)
    {
        DynamicVars["LifeLoss"].BaseValue = highestDebuffAmount();
        InvokeDisplayAmountChanged();
        return base.AfterPowerAmountChanged(choiceContext, power, amount, applier, cardSource);
    }

    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        DynamicVars["LifeLoss"].BaseValue = highestDebuffAmount();
        InvokeDisplayAmountChanged();
        return Task.CompletedTask;
    }

    public int highestDebuffAmount()
    {
        int damage = 0;
        foreach (var power in Owner.Powers)
        {
            if (power.Type == PowerType.Debuff && power.Amount > damage)
            {
                damage = power.Amount;
            }
        }
        return damage;
    }
    public override IEnumerable<HealthBarForecastSegment> GetHealthBarForecastSegments(HealthBarForecastContext context)
    {
        return Owner.CombatState == null ? base.GetHealthBarForecastSegments(context) : [new HealthBarForecastSegment((int)Hook.ModifyDamage(Owner.CombatState.RunState, base.Owner.CombatState, base.Owner, null, highestDebuffAmount(), ValueProp.Unblockable | ValueProp.Unpowered, null, null, ModifyDamageHookType.All, CardPreviewMode.None, out IEnumerable<AbstractModel> _), new Color(0x006dc9bc), HealthBarForecastDirection.FromRight)]; // change the color to whatever you want the color of bar to be or whatever
    }
}