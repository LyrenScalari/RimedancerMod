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
using RimedancerMod.RimedancerModCode.Cards;
using RimedancerMod.RimedancerModCode.Orbs;

namespace RimedancerMod.RimedancerModCode.Cards;

public class RazorIce() : RimedancerModCard(0, CardType.Skill, CardRarity.Common, TargetType.Self), ICustomTypeTextCard
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [MainFile.Spell];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(MainFile.Spell),HoverTipFactory.Static(StaticHoverTip.Channeling),HoverTipFactory.FromOrb<Icicle>(),HoverTipFactory.FromPower<ThornsPower>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Channel",2),new PowerVar<ThornsPower>(4)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        RazorIce card = this;
        for (int i = 0; i < DynamicVars["Channel"].IntValue; ++i)
            await OrbCmd.Channel<Icicle>(choiceContext,  card.Owner);
        Powers.RazorIce razorIce = await PowerCmd.Apply<Powers.RazorIce>(choiceContext, this.Owner.Creature, this.DynamicVars.Power<ThornsPower>().BaseValue, this.Owner.Creature, card);
    }
    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        RazorIce cardSource = this;
        if (!participants.Contains<Creature>(cardSource.Owner.Creature) || cardSource.Owner.PlayerCombatState.TurnNumber > 1)
            return;
        await BagofTricks.Shuffle(choiceContext, cardSource.Owner);
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Power<ThornsPower>().UpgradeValueBy(4);
    }
    public IEnumerable<LocString> GetTypeModifiers()
    {
        return [new LocString("gameplay_ui", "RIMEDANCERMOD-RIMEDANCER_MOD-Spell")];
    }
}