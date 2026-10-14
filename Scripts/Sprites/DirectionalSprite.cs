using Godot;

namespace LarpZ.Scripts.Sprites;

[GlobalClass]
public partial class DirectionalSprite : Sprite2D
{
    [Export] private bool IsPlayer;
    
    private CharacterBody2D parent;
    public AnimationPlayer AnimationPlayer;

    public override void _Ready()
    {
        base._Ready();

        if (GetParent() is CharacterBody2D character)
            parent = character;
        else
            GD.PrintErr("DirectionalSprite expected CharacterBody2D as parent but was " + GetParent().GetType() + "!");

        if (GetChild(0) is AnimationPlayer animationPlayer)
            AnimationPlayer = animationPlayer;
        else
            GD.PrintErr("DirectionalSprite expected AnimationPlayer as first child but was " + GetChild(0).GetType() + "!");
        
        Play("South");
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        
        UpdateDirection();
    }

    private void UpdateDirection()
    {
        var InputVector = Vector2.Zero;

        if (IsPlayer)
        {
            InputVector.X = Input.GetActionStrength("MoveRight") - Input.GetActionStrength("MoveLeft");
            InputVector.Y = Input.GetActionStrength("MoveDown") - Input.GetActionStrength("MoveUp");
        }
        else
        {
            InputVector = parent.Velocity;
        }

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
        if (AnimationPlayer.CurrentAnimation == direction)
            return;
        
        AnimationPlayer.Play(direction);
    }
}