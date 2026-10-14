using Godot;
using LarpZ.Scripts.Weapons.Ammo.Magazine;

namespace LarpZ.Scripts.Weapons.Guns.BaseGun;

[GlobalClass]
public partial class Gun : Node2D
{
    [Export] private FireType FireType;
    [Export] private float FireRate = 1;
    [Export] private PackedScene BulletScene;

    [Export] private AmmoType AmmoType;
    [Export] private Magazine CurrentMag;

    private Timer FireRateTimer;

    public override void _Ready()
    {
        base._Ready();

        var timer = new Timer();
        timer.WaitTime = FireRate;
        FireRateTimer = timer;
        timer.OneShot = true;
        
        AddChild(timer);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        switch (FireType)
        {
            case FireType.Semi:
                if (Input.IsActionJustPressed("FireWeapon"))
                {
                    FireWeapon();
                }
                break;
            case FireType.Auto:
                if (Input.IsActionPressed("FireWeapon"))
                {
                    FireWeapon();
                }
                break;
        }

    }

    private void FireWeapon()
    {
        if (FireRateTimer.TimeLeft > 0)
            return;
        
        FireRateTimer.Start();
        RefreshTimer();

        GD.Print("fired gun!");
    }

    public void RefreshTimer()
    {
        FireRateTimer.WaitTime = FireRate;
    }
}

public enum FireType
{
    Pump,
    Semi,
    Burst,
    Auto,
}
