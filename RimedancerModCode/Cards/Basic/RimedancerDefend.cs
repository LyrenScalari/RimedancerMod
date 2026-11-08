using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using RimedancerMod.RimedancerModCode.Cards;

namespace RimedancerMod.RimedancerModCode.Cards;


public class RimedancerDefend() : RimedancerModCard(1,
    CardType.Skill, CardRarity.Basic,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(5,ValueProp.Move)];
    public override bool GainsBlock => true;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        RimedancerDefend defend= this;
        Decimal num = await CreatureCmd.GainBlock(defend.Owner.Creature, defend.DynamicVars.Block, play);
    }
    protected override void OnUpgrade() { DynamicVars.Block.UpgradeValueBy(3M); }
}