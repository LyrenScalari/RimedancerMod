using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using RimedancerMod.RimedancerModCode.Cards;

namespace RimedancerMod.RimedancerModCode.Cards;


public class HatTrick() : RimedancerModCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new RepeatVar(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(MainFile.Flourish)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        HatTrick card= this;
        await BagofTricks.Flourish(choiceContext,card.DynamicVars.Repeat.BaseValue,card.Owner);
        CardPileAddResult cardPileAddResult = await CardPileCmd.Add(PileType.Draw.GetPile(card.Owner).Cards[0], BagofTricks.bagoftricks);
        
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Repeat.UpgradeValueBy(1M);
    }
}