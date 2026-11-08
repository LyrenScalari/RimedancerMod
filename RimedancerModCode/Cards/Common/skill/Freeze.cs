using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using RimedancerMod.RimedancerModCode.Cards;
using RimedancerMod.RimedancerModCode.Powers;

namespace RimedancerMod.RimedancerModCode.Cards;


public class Freeze() : RimedancerModCard(2, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy), ICustomTypeTextCard
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain,MainFile.Spell];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Rime>(),HoverTipFactory.FromKeyword(MainFile.Spell)];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<Rime>(8M)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        Freeze card = this;
        ArgumentNullException.ThrowIfNull((object) play.Target, "cardPlay.Target");
        await CreatureCmd.TriggerAnim(card.Owner.Creature, "Cast", card.Owner.Character.CastAnimDelay);
        Rime rime = await PowerCmd.Apply<Rime>(choiceContext, play.Target,card.DynamicVars.Power<Rime>().IntValue, play.Card.Owner.Creature, card); 
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<Rime>().UpgradeValueBy(3);
    }
    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        
        Freeze cardSource = this;
        if (!participants.Contains<Creature>(cardSource.Owner.Creature) || cardSource.Owner.PlayerCombatState.TurnNumber > 1)
            return;
        await BagofTricks.Shuffle(choiceContext, cardSource.Owner);
    }
    public IEnumerable<LocString> GetTypeModifiers()
    {
        return [new LocString("gameplay_ui", "RIMEDANCERMOD-RIMEDANCER_MOD-Spell")];
    }
}