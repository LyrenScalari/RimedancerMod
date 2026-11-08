using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using RimedancerMod.RimedancerModCode.Orbs;
using RimedancerMod.RimedancerModCode.Powers;
using RimedancerMod.RimedancerModCode.Relics;

namespace RimedancerMod.RimedancerModCode.Relics;

public class ScrollofWinter() : RimedancerModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<Fervor>(4M),new DynamicVar("Icicles",1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Fervor>(),HoverTipFactory.Static(StaticHoverTip.Channeling),HoverTipFactory.FromOrb<Icicle>()];
    public override async Task BeforeSideTurnStart(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        ScrollofWinter scrollOfWinter = this;
        if (!participants.Contains<Creature>(scrollOfWinter.Owner.Creature) || scrollOfWinter.Owner.PlayerCombatState.TurnNumber > 1)
            return;
        for (int i = 0; (Decimal) i < scrollOfWinter.DynamicVars["Icicles"].BaseValue; ++i)
            await OrbCmd.Channel<Icicle>((PlayerChoiceContext) new BlockingPlayerChoiceContext(), scrollOfWinter.Owner);
    }
    
}