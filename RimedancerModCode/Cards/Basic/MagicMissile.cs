using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using RimedancerMod.RimedancerModCode.Cards;
using RimedancerMod.RimedancerModCode.Orbs;

namespace RimedancerMod.RimedancerModCode.Cards;

public class MagicMissile() : RimedancerModCard(1, CardType.Attack, CardRarity.Basic, TargetType.Self), ICustomTypeTextCard
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [MainFile.Spell];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(MainFile.Spell),HoverTipFactory.Static(MainFile.Fire),HoverTipFactory.FromOrb<Icicle>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Fire",3), new DamageVar(4,ValueProp.Move)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        MagicMissile card = this;
        Creature? target = null;
        int HitCounter = DynamicVars["Fire"].IntValue;
        foreach (var orb in Owner.PlayerCombatState.OrbQueue.Orbs)
        {
            if (orb is Icicle && HitCounter > 0)
            {
                List<Creature> list = this.CombatState.GetOpponentsOf(this.Owner.Creature).Where<Creature>((Func<Creature, bool>) (e => e.IsHittable)).ToList<Creature>();
                target = Owner.RunState.Rng.CombatTargets.NextItem<Creature>((IEnumerable<Creature>) list);
                await ((Icicle)orb).ApplyIcicleDamage(orb.PassiveVal+card.DynamicVars.Damage.PreviewValue,target,choiceContext);
                HitCounter--;
            }
            if (HitCounter == 0) break;
        }
    }
    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        
        MagicMissile cardSource = this;
        if (!participants.Contains<Creature>(cardSource.Owner.Creature) || cardSource.Owner.PlayerCombatState.TurnNumber > 1)
            return;
        await BagofTricks.Shuffle(choiceContext, cardSource.Owner);
    }
    protected override void OnUpgrade()
    {
      DynamicVars["Fire"].UpgradeValueBy(1);
    }
    public IEnumerable<LocString> GetTypeModifiers()
    {
        return [new LocString("gameplay_ui", "RIMEDANCERMOD-RIMEDANCER_MOD-Spell")];
    }
}