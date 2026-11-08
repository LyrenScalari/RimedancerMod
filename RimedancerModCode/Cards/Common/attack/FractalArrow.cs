using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using RimedancerMod.RimedancerModCode.Powers;
using Barrage = RimedancerMod.RimedancerModCode.Powers.Barrage;

namespace RimedancerMod.RimedancerModCode.Cards;


public class FractalArrow() : RimedancerModCard(2,
    CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Barrage>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(9,ValueProp.Move),new PowerVar<Barrage>(2M)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        FractalArrow card = this;
        ArgumentNullException.ThrowIfNull((object) play.Target, "cardPlay.Target");
        AttackCommand attackCommand = await DamageCmd.Attack(card.DynamicVars.Damage.BaseValue).FromCard((CardModel) card,play).Targeting(play.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        await CreatureCmd.TriggerAnim(card.Owner.Creature, "PowerUp", card.Owner.Character.PowerUpAnimDelay);
        await OrbCmd.AddSlots(card.Owner, card.DynamicVars.Power<Barrage>().IntValue);
        Barrage barrage = await PowerCmd.Apply<Barrage>(choiceContext,card.Owner.Creature,card.DynamicVars.Power<Barrage>().IntValue, play.Card.Owner.Creature, card); 
    }

    protected override void OnUpgrade()
    {
        
    }
}