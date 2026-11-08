using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;

namespace RimedancerMod.RimedancerModCode.Orbs;

public class Spectre: EncodedOrbModel 
{
    public override Color DarkenedColor => new Color("7860a7");
    
    protected override string ChannelSfx => "event:/sfx/characters/defect/defect_dark_channel";
    public override Decimal PassiveVal => ModifyOrbValue(15M);
    public override Decimal EvokeVal => 2M;

    public override Node2D? CreateCustomSprite()
    {
        PackedScene scene = PreloadManager.Cache.GetScene(
            "res://RimedancerMod/scenes/model/orbs/Spectre.tscn");

        return scene.Instantiate<Node2D>();
    }
    public override async Task BeforeTurnEndOrbTrigger(PlayerChoiceContext choiceContext)
    {
        await this.Passive(choiceContext,Holder);
    }
    public override async Task Passive(PlayerChoiceContext choiceContext, Creature? target)
    {
        Spectre spectre = this;
    }
    public override Decimal ModifyDamageMultiplicative(
        Creature? target,
        Decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource,
        CardPlay? cardPlay)
    {
        GD.Print("Spectre orb damage modified hook.");
        if (target != Holder) return 1M;
        return 1+(PassiveVal/100);
    }
    public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext choiceContext)
    {
        await PowerCmd.Apply<TemporaryFocusPower>(choiceContext, Owner.Creature, EvokeVal, Owner.Creature,null,false);
        return Enumerable.Empty<Creature>();
    }
}