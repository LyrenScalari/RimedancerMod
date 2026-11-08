using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;
using RimedancerMod.RimedancerModCode.Orbs;
using RimedancerMod.RimedancerModCode.Relics;

namespace RimedancerMod.RimedancerModCode.Relics;


public class CryoniteShard() : RimedancerModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Starter;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Icicles",1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Channeling),HoverTipFactory.FromOrb<Icicle>()];
    public override RelicModel GetUpgradeReplacement() => ModelDb.Relic<ScrollofWinter>();
    public override async Task BeforeSideTurnStart(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        CryoniteShard cryoniteShard = this;
        
        if (!participants.Contains<Creature>(cryoniteShard.Owner.Creature) || cryoniteShard.Owner.PlayerCombatState.TurnNumber > 1)
            return;
        for (int i = 0; (Decimal) i < cryoniteShard.DynamicVars["Icicles"].BaseValue; ++i)
            await OrbCmd.Channel<Icicle>((PlayerChoiceContext) new BlockingPlayerChoiceContext(), cryoniteShard.Owner);
    }
    
}