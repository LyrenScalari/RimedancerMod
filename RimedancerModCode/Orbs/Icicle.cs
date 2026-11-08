using System.Collections.ObjectModel;
using BaseLib.Abstracts;
using BaseLib.Patches.Content;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace RimedancerMod.RimedancerModCode.Orbs;
public class Icicle : CustomOrbModel 
{
    public override Color DarkenedColor => new Color("7860a7");
    
    protected override string ChannelSfx => "event:/sfx/characters/defect/defect_frost_channel";
    public override Decimal PassiveVal => this.ModifyOrbValue(2M);
    public override Decimal EvokeVal => PassiveVal*2;
    public override Node2D? CreateCustomSprite()
    {
        PackedScene scene = PreloadManager.Cache.GetScene(
            "res://RimedancerMod/scenes/model/orbs/Icicle.tscn");

        return scene.Instantiate<Node2D>();
    }
    public override async Task BeforeTurnEndOrbTrigger(PlayerChoiceContext choiceContext)
    {
        if (IcicleTargeting.LastTargeted[this.Owner.PlayerCombatState] != null)
        {
            await this.Passive(choiceContext, IcicleTargeting.LastTargeted[this.Owner.PlayerCombatState]);
        }
    }
    public override async Task Passive(PlayerChoiceContext choiceContext, Creature? target)
    {
        Icicle icicle = this;
        IEnumerable<Creature> creatures = await icicle.ApplyIcicleDamage(icicle.PassiveVal, target, choiceContext);
    }

    public async Task<IEnumerable<Creature>> EvokeChain(PlayerChoiceContext playerChoiceContext)
    {
        if (IcicleTargeting.LastTargeted[this.Owner.PlayerCombatState] != null)
        {
            foreach (var orb in Owner.PlayerCombatState.OrbQueue.Orbs)
            {
                if (orb is Icicle && orb != this)
                {
                    await OrbCmd.EvokeNext(playerChoiceContext, Owner, true);
                    break;
                }
            }
            return [IcicleTargeting.LastTargeted[this.Owner.PlayerCombatState]];
        }
        return [];
    }
    public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext playerChoiceContext)
    {
        if (IcicleTargeting.LastTargeted[this.Owner.PlayerCombatState] != null)
        {
            Icicle icicle = this;
            await icicle.ApplyIcicleDamage(icicle.EvokeVal, IcicleTargeting.LastTargeted[this.Owner.PlayerCombatState], playerChoiceContext);
            return await icicle.EvokeChain(playerChoiceContext);
        }
        return Enumerable.Empty<Creature>();
    }
    public async Task<IEnumerable<Creature>> ApplyIcicleDamage(
        Decimal value,
        Creature? target,
        PlayerChoiceContext choiceContext)
    {
        Icicle icicle = this;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: object of a compiler-generated type is created
        VfxCmd.PlayOnCreatureCenter(target, "vfx/vfx_shiv_throw");
        icicle.PlayEvokeSfx();
        IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, target, value, ValueProp.Unpowered, icicle.Owner.Creature);
        return [target];
    }
}