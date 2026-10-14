using Godot;

namespace LarpZ.Scripts.Weapons.Ammo.Magazine;

[GlobalClass]
public partial class Magazine : Node
{
    [Export] private int MaxAmmo = 30;
    private int CurrentAmmo = 30;

    [Export] private AmmoType AmmoType;
}

public enum AmmoType
{
    FiveFiveSix,
}