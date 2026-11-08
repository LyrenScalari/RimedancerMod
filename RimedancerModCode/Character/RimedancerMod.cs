using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using RimedancerMod.RimedancerModCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using RimedancerMod.RimedancerModCode.Cards;
using RimedancerMod.RimedancerModCode.Relics;

namespace RimedancerMod.RimedancerModCode.Character;



public class RimedancerMod : PlaceholderCharacterModel
{
    public const string CharacterId = "RimedancerMod";

    public static readonly Color Color = new("42b3f5");

    public override Color NameColor => Color;
    public override Color DialogueColor => Color;
    public override VfxColor SpeechBubbleColor => VfxColor.Blue;

    public override Color MapDrawingColor => Color;

    public override Color RemoteTargetingLineColor => Color;

    public override Color RemoteTargetingLineOutline => Color;

    public override CharacterGender Gender => CharacterGender.Masculine;
    public override int StartingHp => 62;

    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<RimedancerStrike>(),
        ModelDb.Card<RimedancerStrike>(),
        ModelDb.Card<RimedancerStrike>(),
        ModelDb.Card<RimedancerStrike>(),
        ModelDb.Card<HailstoneHaze>(),
        ModelDb.Card<RimedancerDefend>(),
        ModelDb.Card<RimedancerDefend>(),
        ModelDb.Card<RimedancerDefend>(),
        ModelDb.Card<RimedancerDefend>(),
        ModelDb.Card<Crystalize>(),
        ModelDb.Card<MagicMissile>(),
        ModelDb.Card<FractalFlash>(),
        ModelDb.Card<Frostbite>()
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<CryoniteShard>()
    ];

    public override CardPoolModel CardPool => ModelDb.CardPool<RimedancerModCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<RimedancerModRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<RimedancerModPotionPool>();

    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets.
        These are just some of the simplest assets, given some placeholders to differentiate your character with.
        You don't have to, but you're suggested to rename these images. */
    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }
    public override int BaseOrbSlotCount => 4;
    public override string CustomIconTexturePath => "character_icon_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_char_name_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_char_name.png".CharacterUiPath();
}