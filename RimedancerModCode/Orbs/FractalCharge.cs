using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace RimedancerMod.RimedancerModCode.Orbs;

public class FractalCharge: EncodedOrbModel 
{
    public override Color DarkenedColor => new Color("7860a7");
    
    protected override string ChannelSfx => "event:/sfx/characters/defect/defect_dark_channel";
    
    public Decimal _evokeVal = 8M;
    public override Decimal PassiveVal => ModifyOrbValue(6M);
    public override Decimal EvokeVal => _evokeVal;

    public override Node2D? CreateCustomSprite()
    {
        PackedScene scene = PreloadManager.Cache.GetScene(
            "res://RimedancerMod/scenes/model/orbs/Spectre.tscn");

        return scene.Instantiate<Node2D>();
    }
    public override async Task BeforeTurnEndOrbTrigger(PlayerChoiceContext choiceContext)
    {
        await this.TriggerPassive(choiceContext, (Creature) null);
    }

    public override Task Passive(PlayerChoiceContext choiceContext, Creature? target)
    {
        if (target != null)
            throw new InvalidOperationException("Dark orbs cannot target creatures.");
        this.ActivatePassive();
        this._evokeVal += this.PassiveVal;
        NCombatRoom.Instance?.GetCreatureNode(this.Owner.Creature)?.OrbManager?.UpdateVisuals(OrbEvokeType.None);
        if (_evokeVal > 30)
        {
            OrbCmd.Evoke(choiceContext, Source,this,true);
        }
        return Task.CompletedTask;
    }
    
    public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext playerChoiceContext)
    {
        this.PlayEvokeSfx();
        this.ActivateEvoke([Holder]);
        if (_evokeVal > 30)
        {
            foreach (Creature hittableEnemy in (IEnumerable<Creature>) this.CombatState.HittableEnemies)
            {
                IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(playerChoiceContext, hittableEnemy,
                    this.EvokeVal, ValueProp.Unpowered, this.Owner.Creature);
            }
        }else
        {
            IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(playerChoiceContext, Holder,
                this.EvokeVal, ValueProp.Unpowered, this.Owner.Creature);
        }
        return [Holder];
    }
}