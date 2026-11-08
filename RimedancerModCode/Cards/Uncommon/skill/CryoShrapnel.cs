using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using RimedancerMod.RimedancerModCode.Cards;
using RimedancerMod.RimedancerModCode.Orbs;

namespace RimedancerMod.RimedancerModCode.Cards;


public class CryoShrapnel() : RimedancerModCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(MainFile.Fire),HoverTipFactory.FromOrb<Icicle>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Fire",2), new PowerVar<WeakPower>(1)];

    protected override async Task OnPlay( PlayerChoiceContext choiceContext, CardPlay play)
    {
        CryoShrapnel card = this;
        Creature? target = null;
        int HitCounter = DynamicVars["Fire"].IntValue;
        foreach (var orb in Owner.PlayerCombatState.OrbQueue.Orbs)
        {
            if (orb is Icicle && HitCounter > 0)
            {
                await ((Icicle)orb).ApplyIcicleDamage(orb.PassiveVal,IcicleTargeting.LastTargeted[Owner.PlayerCombatState],choiceContext);
                WeakPower weakPower = await PowerCmd.Apply<WeakPower>(choiceContext, IcicleTargeting.LastTargeted[Owner.PlayerCombatState], card.DynamicVars.Weak.BaseValue, card.Owner.Creature, card);
                HitCounter--;
            }
            if (HitCounter == 0) break;
        }
    }
    protected override void OnUpgrade()
    {
      DynamicVars["Fire"].UpgradeValueBy(1);
    }
}