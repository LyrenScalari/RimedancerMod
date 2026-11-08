using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using RimedancerMod.RimedancerModCode.Orbs;

namespace RimedancerMod.RimedancerModCode.Cards;


public class RiggedGambit() : RimedancerModCard(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Fire",2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(MainFile.Aimed),HoverTipFactory.FromOrb<Icicle>(),HoverTipFactory.Static(MainFile.Fire)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}