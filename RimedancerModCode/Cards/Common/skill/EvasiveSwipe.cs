using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using RimedancerMod.RimedancerModCode.Cards;

namespace RimedancerMod.RimedancerModCode.Cards;


public class EvasiveSwipe() : RimedancerModCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(MainFile.Exploit),HoverTipFactory.FromPower<WeakPower>(),HoverTipFactory.Static(MainFile.Flourish)];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1),new RepeatVar(1),new BlockVar(9,ValueProp.Move)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        EvasiveSwipe card= this;
        Decimal num = await CreatureCmd.GainBlock(card.Owner.Creature, card.DynamicVars.Block, play);
        IEnumerable<CardModel> cardModels = await CardPileCmd.Draw(choiceContext, this.DynamicVars.Cards.BaseValue, this.Owner);
        await BagofTricks.Flourish(choiceContext,card.DynamicVars.Repeat.BaseValue,card.Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3M);
        DynamicVars.Cards.UpgradeValueBy(1);
    }
}