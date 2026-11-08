using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace RimedancerMod.RimedancerModCode;

[HarmonyPatch(typeof(NCreature), "_Ready")]
public static class NCreaturePatch
{
    [HarmonyPostfix]
    public static void Postfix(NCreature __instance)
    {
        if (!__instance.Entity.IsPlayer)
        {
            // Anchor to HpBarContainer: the base game sizes it per-creature
            // (creature bounds + 12px each side) in UpdateLayoutForCreatureBounds,
            // so its rect matches each enemy's visible bar. The fixed-width hitbox
            // and the 0x0 NHealthBar rect do not.
            var stateDisplay = __instance._stateDisplay;
            var healthBar = stateDisplay?._healthBar;
            Control anchor = healthBar?.HpBarContainer
                             ?? stateDisplay?._hpBarHitbox
                             ?? (Control?)healthBar
                             ?? __instance.Hitbox;

            // Parent to the state display rather than the creature, so the crosshair is a
            // sibling of the health bar and inherits its modulate and visibility: it fades
            // out with the bar and the pause dim covers it the same way.
            var indicator = NAimIndicator.Create(__instance, anchor);
            indicator.Name = "Aimed";
            ((Control?)stateDisplay ?? __instance).AddChild(indicator);
            return;
        }
    }
}