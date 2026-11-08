using HarmonyLib;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;

namespace RimedancerMod.RimedancerModCode.patches;
[HarmonyPatch]
public class MonsterOrbModifyValuePatch
{
    
    [HarmonyPatch(typeof(OrbModel), nameof(OrbModel.ModifyOrbValue))]
    public static bool Prefix(OrbModel __instance, decimal result, ref decimal __result)
    {
        if (MonsterOrbPatch.OwnerState[__instance] != null)
        {
            if (MonsterOrbPatch.OwnerState[__instance] != null && MonsterOrbPatch.OwnerState[__instance].CombatState != null)
            {
                __result = Hook.ModifyOrbValue(MonsterOrbPatch.OwnerState[__instance].CombatState, __instance, result);
                return false;
            }
            __result = result;
            return false;
        }
        return true;
    }
}