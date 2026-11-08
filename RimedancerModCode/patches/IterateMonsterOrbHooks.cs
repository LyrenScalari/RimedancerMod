using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Models;

namespace RimedancerMod.RimedancerModCode.patches;
[HarmonyPatch]
public class IterateMonsterOrbHooks
{
    [HarmonyPatch(typeof(CombatState), nameof(CombatState.IterateHookListeners))]
    public static void Postfix(IEnumerable<AbstractModel> __result)
    {
        foreach (MonsterOrbManager mgr in  MonsterOrbManager.Instances.Where(mgr => mgr.Owner != null))
        {
            foreach (OrbModel orb in mgr._orbs)
            {
                __result.Append(orb);
            }
        }
    }
}