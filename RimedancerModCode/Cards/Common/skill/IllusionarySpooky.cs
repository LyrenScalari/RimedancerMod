using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using RimedancerMod.RimedancerModCode.Cards;
using RimedancerMod.RimedancerModCode.Orbs;
using RimedancerMod.RimedancerModCode.util;

namespace RimedancerMod.RimedancerModCode.Cards;


public class IllusionarySpooky() : RimedancerModCard(1,
    CardType.Skill, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Encode",1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(MainFile.Encode),HoverTipFactory.FromOrb<Spectre>()];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull((object) play.Target, "cardPlay.Target");
        for (int i = 0; i < DynamicVars["Encode"].IntValue; ++i)
            await MonsterOrbCmd.Channel<Spectre>(this.Owner, play.Target);
        
    }

    protected override void OnUpgrade()
    {
    }
}