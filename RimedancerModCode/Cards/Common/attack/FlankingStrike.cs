using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using RimedancerMod.RimedancerModCode.Cards;
using RimedancerMod.RimedancerModCode.Orbs;

namespace RimedancerMod.RimedancerModCode.Cards;


public class FlankingStrike() : RimedancerModCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override HashSet<CardTag> CanonicalTags { get => new HashSet<CardTag>() { CardTag.Strike }; }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8,ValueProp.Move),new DynamicVar("Channel",1),new DynamicVar("Fire",2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Channeling),HoverTipFactory.FromOrb<Icicle>(),HoverTipFactory.Static(MainFile.Fire)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        FlankingStrike card = this;
        ArgumentNullException.ThrowIfNull((object) play.Target, "cardPlay.Target");
        AttackCommand attackCommand = await DamageCmd.Attack(card.DynamicVars.Damage.BaseValue).FromCard((CardModel) card,play).Targeting(play.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        for (int i = 0; i < DynamicVars["Channel"].IntValue; ++i)
            await OrbCmd.Channel<Icicle>(choiceContext,  card.Owner);
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
        DynamicVars["Channel"].UpgradeValueBy(1);
        DynamicVars["Fire"].UpgradeValueBy(1);
    }
}