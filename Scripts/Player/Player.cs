using System.Collections.Generic;
using Godot;
using LarpZ.Scripts.Items;

namespace LarpZ.Scripts.Player;

public partial class Player : CharacterBody2D
{
    [Export] private float Speed = 400;
    [Export] private float Acceleration = 0.1f;

    public override void _PhysicsProcess(double delta)
    {
        HandleMovement();
    }

    public override void _UnhandledKeyInput(InputEvent args)
    {
        base._UnhandledKeyInput(args);
        
        if (Input.IsActionJustPressed("DropItem"))
            DropItem();

        if (Input.IsActionJustPressed("WieldItem")) 
            WieldItem();
    }

    private void WieldItem()
    {
        foreach (var item in GetItems())
        {
            item.ToggleWield(this);
        }
    }

    private void DropItem()
    {
        foreach (var item in GetItems())
        {
            item.Drop();
        }
    }

    private List<Item> GetItems()
    {
        List<Item> items = new ();
        
        foreach (var child in GetChildren())
        {
            if (child is not Item item)
                continue;
            
            items.Add(item);
        }

        return items;
    }

    private void HandleMovement()
    {
        var InputVector = Vector2.Zero;

        InputVector.X = Input.GetActionStrength("MoveRight") - Input.GetActionStrength("MoveLeft");
        InputVector.Y = Input.GetActionStrength("MoveDown") - Input.GetActionStrength("MoveUp");

        InputVector = InputVector.Normalized();

        Velocity = Velocity.Lerp(InputVector * Speed, Acceleration);

        MoveAndSlide();
    }
}