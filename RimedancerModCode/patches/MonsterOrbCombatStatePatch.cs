using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Models;

namespace RimedancerMod.RimedancerModCode.patches;
[HarmonyPatch]
public class MonsterOrbCombatStatePatch
{
    [HarmonyPatch(typeof(OrbModel), nameof(OrbModel.CombatState), MethodType.Getter)]
    public static bool Prefix(OrbModel __instance, ref ICombatState __result)
    {
        if (MonsterOrbPatch.OwnerState[__instance] != null && MonsterOrbPatch.OwnerState[__instance].CombatState != null)
        {
            __result = MonsterOrbPatch.OwnerState[__instance].CombatState;
            return false;
        }
        return true;
    }
}