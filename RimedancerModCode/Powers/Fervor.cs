using System.Runtime.InteropServices.JavaScript;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using RimedancerMod.RimedancerModCode.Powers;

namespace RimedancerMod.RimedancerModCode.Powers;

public class Fervor() : RimedancerModPower
{
    public bool Shoulddecrement = false;
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;
    
    public override Decimal ModifyDamageAdditive(
        Creature? target,
        Decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource,
        CardPlay? cardPlay)
    {
        if (cardSource.CanonicalKeywords.Contains(MainFile.Spell))
        {
            Shoulddecrement = true;
        } else Shoulddecrement = false;
        return Owner != dealer || !props.IsPoweredAttack() || cardSource == null || !cardSource.CanonicalKeywords.Contains(MainFile.Spell) ? 0M : Amount;
    }
    public override Decimal ModifyBlockAdditive(
        Creature target,
        Decimal block,
        ValueProp props,
        CardModel? cardSource,
        CardPlay? cardPlay)
    {
        if (cardSource.CanonicalKeywords.Contains(MainFile.Spell))
        {
            Shoulddecrement = true;
        } else Shoulddecrement = false;
        return !props.IsPoweredCardOrMonsterMoveBlock() || cardSource == null || !cardSource.CanonicalKeywords.Contains(MainFile.Spell) ? 0M : Amount;
    }

    public override decimal ModifyPowerAmountGivenAdditive(PowerModel power, Creature giver, decimal amount,
        Creature? target,
        CardModel? cardSource)
    {
        if (cardSource != null && cardSource.CanonicalKeywords.Contains(MainFile.Spell) && (power is not EnergyNextTurnPower || power is not Fervor || power is not FocusPower || power is not TemporaryFocusPower))
        {
            Shoulddecrement = true;
        } else Shoulddecrement = false;
        return cardSource == null || !cardSource.CanonicalKeywords.Contains(MainFile.Spell) ?  amount : Amount+ amount;
    }

    public override int ModifyXValue(CardModel card, int originalValue)
    {
        if (card.CanonicalKeywords.Contains(MainFile.Spell))
        {
            Shoulddecrement = true;
        }
        return !card.CanonicalKeywords.Contains(MainFile.Spell) ? originalValue : Amount+originalValue;
    }
    
    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.CanonicalKeywords.Contains(MainFile.Spell) && Shoulddecrement)
        {
            PowerCmd.Decrement(this);
        }
        return Task.CompletedTask;
    }
    public override Task AfterOrbChanneled(PlayerChoiceContext choiceContext, Player player, OrbModel orb)
    {
        FervorVal.FervorBonus[orb] = Amount;
        PowerCmd.Decrement(this);
        return Task.CompletedTask;
    }
}