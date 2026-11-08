using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using RimedancerMod.RimedancerModCode.Cards;
using RimedancerMod.RimedancerModCode.Powers;

namespace RimedancerMod.RimedancerModCode.Cards;


public class IceBarrier() : RimedancerModCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Rime>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(7,ValueProp.Move),new PowerVar<Rime>(4M)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        IceBarrier card= this;
        Decimal num = await CreatureCmd.GainBlock(card.Owner.Creature, card.DynamicVars.Block, play);
        Rime rime = await PowerCmd.Apply<Rime>(choiceContext, play.Target,card.DynamicVars.Power<Rime>().IntValue, play.Card.Owner.Creature, card); 
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3M);
        DynamicVars.Power<Rime>().UpgradeValueBy(2);
    }
}