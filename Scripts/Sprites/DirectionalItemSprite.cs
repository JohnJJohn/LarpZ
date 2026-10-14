using Godot;
using GodotUtilities;
using LarpZ.Scripts.Items;

namespace LarpZ.Scripts.Sprites;

[GlobalClass]
public partial class DirectionalItemSprite : Sprite2D
{
    private AnimationPlayer AnimationPlayer;
    private Item Item;

    public override void _Ready()
    {
        base._Ready();

        if (GetParent() is Item item)
        {
            Item = item;
            
            if (!Item.Held)
                Hide();
        }
        else
        {
            GD.PrintErr("Parent of: " + Name + " was not an item!");
        }
        
        if (GetChild(0) is AnimationPlayer animationPlayer)
            AnimationPlayer = animationPlayer;
        else
            GD.PrintErr("DirectionalItemSprite expected AnimationPlayer as first child but was " + GetChild(0).GetType() + "!");

        Item.ItemGotPickedUp += OnPickup;
        Item.ItemGotDropped += OnDropped;
        Item.ItemGotWielded += OnWielded;
        Item.ItemGotUnWielded += OnUnwielded;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        
        UpdateDirection();
    }
    
    private void OnWielded(Node2D wielder)
    {
        foreach (var child in wielder.GetChildrenOfType<DirectionalSprite>())
        {
            Play(child.AnimationPlayer.CurrentAnimation);
            break;
        }
    }
    
    private void OnUnwielded(Node2D wielder)
    {
        RefreshItem(wielder);
    }
    
    private void OnPickup(Node2D pickUper)
    {
        Show();

        RefreshItem(pickUper);
    }

    private void RefreshItem(Node2D toCheck)
    {
        foreach (var child in toCheck.GetChildrenOfType<DirectionalSprite>())
        {
            GD.Print(child.AnimationPlayer.CurrentAnimation);
            Play(child.AnimationPlayer.CurrentAnimation);
            break;
        }
    }
    
    private void OnDropped()
    {
        Hide();
    }
    
    private void UpdateDirection()
    {
        if (!Item.Held)
            return;
        
        var InputVector = Vector2.Zero;
        
        InputVector.X = Input.GetActionStrength("MoveRight") - Input.GetActionStrength("MoveLeft");
        InputVector.Y = Input.GetActionStrength("MoveDown") - Input.GetActionStrength("MoveUp");

        InputVector = InputVector.Normalized();
        
        if (InputVector.Y < -0.5)
            Play("North");
        if (InputVector.Y > 0.5)
            Play("South");
        if (InputVector.X < -0.5)
            Play("West");
        if (InputVector.X > 0.5)
            Play("East");
    }

    private void Play(string direction)
    {
        var animation = GetPrefix() + direction;
        
        if (AnimationPlayer.CurrentAnimation == animation)
            return;
        
        AnimationPlayer.Play(animation);
    }

    private string GetPrefix()
    {
        if (Item.Wielded)
            return "Wielded";
        
        return "Held";
    }
}