using System.Reflection;
using BaseLib.Patches.Content;
using BaseLib.Utils;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Modding;

namespace RimedancerMod.RimedancerModCode;


[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    public const string ModId = "RimedancerMod"; //Used for resource filepath
    public const string ResPath = $"res://{ModId}";

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
        new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    [CustomEnum, KeywordProperties(AutoKeywordPosition.None)]
    public static CardKeyword Spell;
    
    [CustomEnum]
    public static StaticHoverTip Flourish;
    
    [CustomEnum]
    public static StaticHoverTip Fire;
    
    [CustomEnum]
    public static StaticHoverTip Exploit;
    
    [CustomEnum]
    public static StaticHoverTip Aimed;
    
    [CustomEnum]
    public static StaticHoverTip Deploy;
    
    [CustomEnum]
    public static StaticHoverTip Encode;
    
    [CustomEnum]
    public static StaticHoverTip Decode;
    
    [CustomEnum]
    public static StaticHoverTip Cipher;
    public static void Initialize()
    {
        //If you want to use scripts defined in your mod for Godot scenes, uncomment the following line.
        Godot.Bridge.ScriptManagerBridge.LookupScriptsInAssembly(Assembly.GetExecutingAssembly());
        ModHelper.SubscribeForCombatStateHooks("RimedancerMonsterOrbManager", MonsterOrbManager.CombatStateHooks);
        Harmony harmony = new(ModId);
        harmony.PatchAll();
    }
}