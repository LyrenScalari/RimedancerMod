using HarmonyLib;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace RimedancerMod.RimedancerModCode.patches;
[HarmonyPatch]
public class OrbTipOnMonsterPatch 
{
    [HarmonyPatch(typeof(OrbModel), nameof(OrbModel.HoverTips), MethodType.Getter)]
    public static bool Prefix(OrbModel __instance, ref IEnumerable<IHoverTip> __result)
    {
        if (__instance.Owner != null || MonsterOrbPatch.OwnerState[__instance] != null)
            return true;

        var owner = MonsterOrbPatch.GetHolder(__instance);
        var extraHoverTips = Traverse.Create(__instance)
            .Property<IEnumerable<IHoverTip>>("ExtraHoverTips").Value;
        var list = extraHoverTips?.ToList() ?? [];

        var hasSmart = LocString.Exists("orbs", $"{__instance.Id.Entry}.smartDescriptionOnMonster");
        if (hasSmart && __instance.IsMutable)
        {
            var smart = new LocString("orbs", $"{__instance.Id.Entry}.smartDescriptionOnMonster");
            smart.Add("Owner", owner?.Name ?? "???");
            smart.Add("Passive", __instance.PassiveVal);
            smart.Add("Evoke", __instance.EvokeVal);
            list.Add(new HoverTip(__instance, smart));
        }
        else
        {
            list.Add(__instance.DumbHoverTip);
        }
        __result = list;
        return false;
    }
}