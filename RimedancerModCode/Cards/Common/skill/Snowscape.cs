using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using RimedancerMod.RimedancerModCode.Cards;
using RimedancerMod.RimedancerModCode.Orbs;
using RimedancerMod.RimedancerModCode.Powers;

namespace RimedancerMod.RimedancerModCode.Cards;


public class Snowscape() : RimedancerModCard(0, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Channel",1),new PowerVar<Rime>(4M)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Channeling),HoverTipFactory.FromOrb<Hail>(),HoverTipFactory.FromPower<FocusPower>()];
    public override TargetType TargetType { get => !IsUpgraded ? TargetType.AnyEnemy : TargetType.AllEnemies; }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        Snowscape gatheringStorm = this;
        if (IsUpgraded)
        {
            foreach (Creature enemy in (IEnumerable<Creature>) this.CombatState.HittableEnemies)
            {
                Rime rime = await PowerCmd.Apply<Rime>(choiceContext, play.Target,DynamicVars.Power<Rime>().IntValue, play.Card.Owner.Creature, gatheringStorm); 
            }
        } else
        {
            Rime rime = await PowerCmd.Apply<Rime>(choiceContext, play.Target,DynamicVars.Power<Rime>().IntValue, play.Card.Owner.Creature, gatheringStorm); 
        } 
        await CreatureCmd.TriggerAnim(gatheringStorm.Owner.Creature, "Cast", gatheringStorm.Owner.Character.CastAnimDelay);
        for (int i = 0; i < DynamicVars["Channel"].IntValue; ++i)
            await OrbCmd.Channel<Hail>(choiceContext, gatheringStorm.Owner);
    }

    protected override void OnUpgrade()
    {
    }
}