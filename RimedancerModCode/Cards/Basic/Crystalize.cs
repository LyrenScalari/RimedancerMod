using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using RimedancerMod.RimedancerModCode.Orbs;

namespace RimedancerMod.RimedancerModCode.Cards;


public class Crystalize() : RimedancerModCard(1, CardType.Skill, CardRarity.Basic, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(4,ValueProp.Move),new DynamicVar("Channel",1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Channeling),HoverTipFactory.FromOrb<Icicle>()];
    public override bool GainsBlock => true;
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        Crystalize crystalize= this;
        await CreatureCmd.TriggerAnim(crystalize.Owner.Creature, "Cast", crystalize.Owner.Character.CastAnimDelay);
        Decimal num = await CreatureCmd.GainBlock(crystalize.Owner.Creature, crystalize.DynamicVars.Block, play);
        for (int i = 0; i < DynamicVars["Channel"].IntValue; ++i)
            await OrbCmd.Channel<Icicle>(choiceContext,  crystalize.Owner);
    }

    protected override void OnUpgrade()
    {
       DynamicVars.Block.UpgradeValueBy(3M);
    }
}