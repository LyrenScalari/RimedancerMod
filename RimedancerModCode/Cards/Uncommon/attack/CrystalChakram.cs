using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using RimedancerMod.RimedancerModCode.Cards;
using RimedancerMod.RimedancerModCode.Orbs;

namespace RimedancerMod.RimedancerModCode.Cards;

public class CrystalChakram() : RimedancerModCard(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(MainFile.Fire),HoverTipFactory.FromOrb<Icicle>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Fire",1), new DamageVar(2,ValueProp.Move), new CardsVar(1)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        CrystalChakram card = this;
        ArgumentNullException.ThrowIfNull((object) play.Target, "cardPlay.Target");
        int HitCounter = DynamicVars["Fire"].IntValue;
        foreach (var orb in Owner.PlayerCombatState.OrbQueue.Orbs)
        {
            if (orb is Icicle && HitCounter > 0)
            {
                await ((Icicle)orb).ApplyIcicleDamage(orb.PassiveVal+card.DynamicVars.Damage.PreviewValue,play.Target,choiceContext);
                IEnumerable<CardModel> cardModels = await CardPileCmd.Draw(choiceContext, this.DynamicVars.Cards.BaseValue, this.Owner);
                HitCounter--;
                break;
            }
            if (HitCounter == 0) break;
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2M);
    }
}