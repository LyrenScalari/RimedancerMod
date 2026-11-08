using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using RimedancerMod.RimedancerModCode.patches;

namespace RimedancerMod.RimedancerModCode.Orbs;

public abstract class EncodedOrbModel : CustomOrbModel
{
    public Creature? Holder => MonsterOrbPatch.OwnerState[this] != null ? MonsterOrbPatch.OwnerState[this] : Owner?.Creature;
    public Player? Source { get; set; }
}