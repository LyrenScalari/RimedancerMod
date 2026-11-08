using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using RimedancerMod.RimedancerModCode.Cards;
using RimedancerMod.RimedancerModCode.Powers;

namespace RimedancerMod.RimedancerModCode.Cards;

public class PillarofFrost() : RimedancerModCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Fervor>(),HoverTipFactory.FromKeyword(MainFile.Spell)];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<Fervor>(4M),new BlockVar(4M,ValueProp.Move)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        PillarofFrost card = this;
        Decimal num = await CreatureCmd.GainBlock(card.Owner.Creature, card.DynamicVars.Block, play);
        Fervor Fervor = await PowerCmd.Apply<Fervor>(choiceContext,card.Owner.Creature,card.DynamicVars.Power<Fervor>().IntValue, play.Card.Owner.Creature, card); 
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(2M);
        DynamicVars.Power<Fervor>().UpgradeValueBy(2M);
    }
}