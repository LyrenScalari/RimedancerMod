using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using RimedancerMod.RimedancerModCode.Cards;
using RimedancerMod.RimedancerModCode.Orbs;
using RimedancerMod.RimedancerModCode.util;

namespace RimedancerMod.RimedancerModCode.Cards;

public class ExplosiveRune() : RimedancerModCard(2,
    CardType.Skill, CardRarity.Common,
    TargetType.AnyEnemy), ICustomTypeTextCard
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [MainFile.Spell];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(MainFile.Spell),HoverTipFactory.Static(MainFile.Encode),HoverTipFactory.FromOrb<FractalCharge>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Encode",1),new BlockVar(10,ValueProp.Move)];
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ExplosiveRune card= this;
        Decimal num = await CreatureCmd.GainBlock(card.Owner.Creature, card.DynamicVars.Block, play);
        ArgumentNullException.ThrowIfNull((object) play.Target, "cardPlay.Target");
        for (int i = 0; i < DynamicVars["Encode"].IntValue; ++i)
            await MonsterOrbCmd.Channel<FractalCharge>(this.Owner, play.Target);
    }

    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        ExplosiveRune cardSource = this;
        if (!participants.Contains<Creature>(cardSource.Owner.Creature) || cardSource.Owner.PlayerCombatState.TurnNumber > 1)
            return;
        await BagofTricks.Shuffle(choiceContext, cardSource.Owner);
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(2);
        DynamicVars["Encode"].UpgradeValueBy(1);
    }
    public IEnumerable<LocString> GetTypeModifiers()
    {
        return [new LocString("gameplay_ui", "RIMEDANCERMOD-RIMEDANCER_MOD-Spell")];
    }
}