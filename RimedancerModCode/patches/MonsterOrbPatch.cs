using System.Reflection;
using BaseLib.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using RimedancerMod.RimedancerModCode.util;

namespace RimedancerMod.RimedancerModCode.patches;
[HarmonyPatch]
internal static class MonsterOrbPatch
{
    internal static SpireField<OrbModel, Creature?> OwnerState = new(() => null);

    internal static bool IsMonster(NCreature creature) => !creature.Entity.IsPlayer && creature.Entity.PetOwner == null;

    internal static bool ContainsRimdancer(ICombatState? state) => state != null && state.Players.Any(p => p.Character is Character.RimedancerMod);

    internal static Creature? GetHolder(OrbModel orb)
    {
        if (OwnerState[orb] != null)
            return OwnerState[orb];
        return MonsterOrbManager.Instances.FirstOrDefault(m => m.GetOrbs().Contains(orb))?.Creature;
    }
}