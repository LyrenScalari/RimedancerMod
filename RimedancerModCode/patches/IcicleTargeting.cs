using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using RimedancerMod.RimedancerModCode.Orbs;

namespace RimedancerMod.RimedancerModCode;

public class IcicleTargeting
{
    public static readonly SpireField<PlayerCombatState, Creature?> LastTargeted = new(() => null); //Default no target.
    
    public static event Action<PlayerCombatState, Creature?>? LastAttackTargetChanged;
    
    public class IcicleTargetingSingleton() : CustomSingletonModel(HookType.Combat)
    {
        public override async Task  AfterCardPlayed(PlayerChoiceContext choiceContext,CardPlay cardPlay)
        {
            if (cardPlay.Card.CurrentTarget != null && cardPlay.Card.CurrentTarget.GetType() != typeof(Player))
            {
                if (cardPlay.Card.Owner.PlayerCombatState == null) return;
                if (LastTargeted[cardPlay.Card.Owner.PlayerCombatState] == cardPlay.Card.CurrentTarget) return;
                LastTargeted[cardPlay.Card.Owner.PlayerCombatState] = cardPlay.Card.CurrentTarget;
                LastAttackTargetChanged?.Invoke(cardPlay.Card.Owner.PlayerCombatState, cardPlay.Card.CurrentTarget);
            }
        }
    }
}