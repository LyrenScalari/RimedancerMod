using BaseLib.Abstracts;
using RimedancerMod.RimedancerModCode.Extensions;
using Godot;

namespace RimedancerMod.RimedancerModCode.Character;

public class RimedancerModRelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => RimedancerMod.Color;

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}