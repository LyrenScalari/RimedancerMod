using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;
using RimedancerMod.RimedancerModCode.Cards;
using RimedancerMod.RimedancerModCode.Orbs;

namespace RimedancerMod.RimedancerModCode.Cards;


public class GatheringStorm() : RimedancerModCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Channel",2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Channeling),HoverTipFactory.FromOrb<Hail>(),HoverTipFactory.FromPower<FocusPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        GatheringStorm gatheringStorm = this;
        await CreatureCmd.TriggerAnim(gatheringStorm.Owner.Creature, "Cast", gatheringStorm.Owner.Character.CastAnimDelay);
        for (int i = 0; i < DynamicVars["Channel"].IntValue; ++i)
            await OrbCmd.Channel<Hail>(choiceContext, gatheringStorm.Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Channel"].UpgradeValueBy(1);
    }
}