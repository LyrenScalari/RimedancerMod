using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using RimedancerMod.RimedancerModCode.Cards;
using RimedancerMod.RimedancerModCode.Powers;

namespace RimedancerMod.RimedancerModCode.Cards;


public class StreetRink() : RimedancerModCard(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
{
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<VulnerablePower>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<VulnerablePower>(2M)];
    public override TargetType TargetType { get => !IsUpgraded ? TargetType.AnyEnemy : TargetType.AllEnemies; }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        StreetRink card = this;
        await CreatureCmd.TriggerAnim(card.Owner.Creature, "Cast", card.Owner.Character.CastAnimDelay);
        if (IsUpgraded)
        {
            foreach (Creature enemy in (IEnumerable<Creature>) this.CombatState.HittableEnemies)
            {
                VulnerablePower vulnerablePower = await PowerCmd.Apply<VulnerablePower>(choiceContext, enemy, card.DynamicVars.Power<VulnerablePower>().IntValue, this.Owner.Creature, (CardModel) this);
            }
        } else
        {
            VulnerablePower rime = await PowerCmd.Apply<VulnerablePower>(choiceContext, play.Target, card.DynamicVars.Power<VulnerablePower>().IntValue, play.Card.Owner.Creature, card);
        } 
    }

    protected override void OnUpgrade()
    {
        
    }
}