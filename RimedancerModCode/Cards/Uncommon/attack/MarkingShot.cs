using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using RimedancerMod.RimedancerModCode.Cards;
using RimedancerMod.RimedancerModCode.Orbs;

namespace RimedancerMod.RimedancerModCode.Cards;

public class MarkingShot() : RimedancerModCard(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(MainFile.Fire),HoverTipFactory.FromOrb<Icicle>(),HoverTipFactory.Static(MainFile.Exploit),HoverTipFactory.FromPower<WeakPower>(),HoverTipFactory.FromPower<VulnerablePower>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Fire",1), new DamageVar(2,ValueProp.Move), new PowerVar<VulnerablePower>(2M)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        MarkingShot card = this;
        ArgumentNullException.ThrowIfNull((object) play.Target, "cardPlay.Target");
        foreach (var orb in Owner.PlayerCombatState.OrbQueue.Orbs)
        {
            if (orb is Icicle)
            {
                await ((Icicle)orb).ApplyIcicleDamage(orb.PassiveVal+card.DynamicVars.Damage.PreviewValue,play.Target,choiceContext);
                if (play.Target.GetPower<WeakPower>() != null)
                {
                    VulnerablePower vulnerablePower = await PowerCmd.Apply<VulnerablePower>(choiceContext, play.Target, card.DynamicVars.Power<VulnerablePower>().IntValue, this.Owner.Creature, (CardModel) this);
                }
                break;
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2M);
    }
}