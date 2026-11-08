using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace RimedancerMod.RimedancerModCode.Orbs;

public class Hail : CustomOrbModel 
{
    public override Color DarkenedColor => new Color("7860a7");
    
    protected override string ChannelSfx => "event:/sfx/characters/defect/defect_frost_channel";
    public override Decimal PassiveVal => this.ModifyOrbValue(2M);
    public override Decimal EvokeVal => 2M;
    public bool Justchanneled = true;
    public override Node2D? CreateCustomSprite()
    {
        PackedScene scene = PreloadManager.Cache.GetScene(
            "res://RimedancerMod/scenes/model/orbs/Hail.tscn");

        return scene.Instantiate<Node2D>();
    }
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Type == CardType.Skill && cardPlay.Card.Owner == Owner && !Justchanneled)
        {
            await TriggerPassive(choiceContext, null);
        }
        if (Justchanneled)
        {
            Justchanneled = false;
        }
    }
    public override async Task Passive(PlayerChoiceContext choiceContext, Creature? target)
    {
        Hail hail = this;
        IEnumerable<Creature> creatures = await hail.ApplyHailDamage(PassiveVal, null, choiceContext);
    }
    
    public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext choiceContext)
    {
        await PowerCmd.Apply<TemporaryFocusPower>(choiceContext, Owner.Creature, EvokeVal, Owner.Creature,null,false);
        return Enumerable.Empty<Creature>();
    }
    private async Task<IEnumerable<Creature>> ApplyHailDamage(
        Decimal value,
        Creature? target,
        PlayerChoiceContext choiceContext)
    {
        Hail hail = this;
        IEnumerable<Creature> list = hail.CombatState.GetOpponentsOf(hail.Owner.Creature).Where<Creature>((Func<Creature, bool>) (e => e.IsHittable)).ToList<Creature>();
        if (!list.Any())
            return (IEnumerable<Creature>) Array.Empty<Creature>();
        
        target = hail.Owner.RunState.Rng.CombatTargets.NextItem(list);
        VfxCmd.PlayOnCreature(target, "vfx/vfx_small_magic_missile");
        hail.PlayEvokeSfx();
        IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, target, value, ValueProp.Unpowered, hail.Owner.Creature);
        return [target];
    }
}