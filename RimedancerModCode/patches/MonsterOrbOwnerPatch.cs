using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace RimedancerMod.RimedancerModCode.patches;
[HarmonyPatch]
public class MonsterOrbOwnerPatch
{
    public static FieldInfo OwnerField = AccessTools.Field(typeof(OrbModel), "_owner");
    [HarmonyPatch(typeof(OrbModel), nameof(OrbModel.Owner), MethodType.Getter)]
    public static bool Prefix(OrbModel __instance, ref Player __result)
    {
        if (OwnerField.GetValue(__instance) is Player)
            return true;

        if (MonsterOrbPatch.OwnerState[__instance] == null)
            return true;

        var combatState = MonsterOrbPatch.OwnerState[__instance].CombatState;
        if (combatState == null) return true;

        var player = combatState.Players[0];
        if (player == null) return true;

        __result = player;
        return false;
    }
}