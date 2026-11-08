using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace RimedancerMod.RimedancerModCode.Cards;


public class Raiton(): RimedancerModCard(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy), ICustomTypeTextCard
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [MainFile.Spell];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(MainFile.Spell)];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CalculationBaseVar(8M),new ExtraDamageVar(1M),new CalculatedDamageVar(ValueProp.Move).WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, _) => 
        (Decimal) PileType.Hand.GetPile(card.Owner).Cards.Count(c => c != card) * card.DynamicVars["HandBonus"].BaseValue)), new IntVar("HandBonus",2)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        Raiton card = this;
        ArgumentNullException.ThrowIfNull((object) play.Target, "cardPlay.Target");
        AttackCommand attackCommand = await DamageCmd.Attack(card.DynamicVars.CalculatedDamage).FromCard((CardModel) card,play).Targeting(play.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
    }
    public override async Task BeforeSideTurnStart( PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        
        Raiton cardSource = this;
        if (!participants.Contains<Creature>(cardSource.Owner.Creature) || cardSource.Owner.PlayerCombatState.TurnNumber > 1)
            return;
        await BagofTricks.Shuffle(choiceContext, cardSource.Owner);
    }
        
    protected override void OnUpgrade()
    {
        DynamicVars["HandBonus"].UpgradeValueBy(2);
    }
    public IEnumerable<LocString> GetTypeModifiers()
    {
        return [new LocString("gameplay_ui", "RIMEDANCERMOD-RIMEDANCER_MOD-Spell")];
    }
}