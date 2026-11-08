using BaseLib.Abstracts;
using BaseLib.Extensions;
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
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using RimedancerMod.RimedancerModCode.Cards;
using RimedancerMod.RimedancerModCode.Powers;

namespace RimedancerMod.RimedancerModCode.Cards;

public class Hyoton() : RimedancerModCard(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy), ICustomTypeTextCard
{   
    public override IEnumerable<CardKeyword> CanonicalKeywords => [MainFile.Spell];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(MainFile.Spell),HoverTipFactory.Static(MainFile.Exploit),HoverTipFactory.FromPower<Rime>(),HoverTipFactory.FromPower<WeakPower>(),HoverTipFactory.FromPower<FrostFever>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(14,ValueProp.Move),new PowerVar<WeakPower>(2M),new PowerVar<FrostFever>(2M)];

    protected override async Task OnPlay( PlayerChoiceContext choiceContext, CardPlay play)
    {
        Hyoton card = this;
        ArgumentNullException.ThrowIfNull((object) play.Target, "cardPlay.Target");
        AttackCommand attackCommand = await DamageCmd.Attack(card.DynamicVars.Damage.BaseValue).FromCard((CardModel) card,play).Targeting(play.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        if (play.Target.GetPower<Rime>() != null)
        {
            WeakPower weakPower = await PowerCmd.Apply<WeakPower>(choiceContext, play.Target, card.DynamicVars.Weak.BaseValue, card.Owner.Creature, card);
            FrostFever frostFever = await PowerCmd.Apply<FrostFever>(choiceContext, play.Target, card.DynamicVars.Power<FrostFever>().BaseValue, card.Owner.Creature, card);
        }
    }
    public override async Task BeforeSideTurnStart( PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        
        Hyoton cardSource = this;
        if (!participants.Contains<Creature>(cardSource.Owner.Creature) || cardSource.Owner.PlayerCombatState.TurnNumber > 1)
            return;
        await BagofTricks.Shuffle(choiceContext, cardSource.Owner);
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4);
        DynamicVars.Weak.UpgradeValueBy(1);
        DynamicVars.Power<FrostFever>().UpgradeValueBy(1);
    }
    public IEnumerable<LocString> GetTypeModifiers()
    {
        return [new LocString("gameplay_ui", "RIMEDANCERMOD-RIMEDANCER_MOD-Spell")];
    }
}