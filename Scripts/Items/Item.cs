using Godot;

namespace LarpZ.Scripts.Items;

[GlobalClass]
public partial class Item : Sprite2D
{
    [Signal]
    public delegate void ItemGotPickedUpEventHandler(Node2D PickUper);
    
    [Signal]
    public delegate void ItemGotDroppedEventHandler();
    
    [Signal]
    public delegate void ItemGotWieldedEventHandler(Node2D Wielder);
    
    [Signal]
    public delegate void ItemGotUnWieldedEventHandler(Node2D Wielder);

    [Export] public Texture2D Icon;
    
    public bool Held;
    public bool Wielded;

    private Node OwningEntity;
    private Area2D PickupArea;

    public override void _Ready()
    {
        base._Ready();

        var pickupArea = new Area2D();
        var shape = new RectangleShape2D();
        var collisonShape = new CollisionShape2D();
        
        shape.Size = new Vector2(Icon.GetWidth(), Icon.GetHeight());

        collisonShape.Shape = shape;
        
        AddChild(pickupArea);
        pickupArea.AddChild(collisonShape);

        pickupArea.InputEvent += OnClicked;
    }

    private void OnClicked(Node viewport, InputEvent args, long shapeIdx)
    {
        if (args is not InputEventMouseButton mouseEvent)
            return;
        
        if (mouseEvent.ButtonIndex != MouseButton.Left)
            return;
        
        var Player = GetTree().GetNodesInGroup("Player");
        
        foreach (var player in Player)
        {
            if (player is not Node2D player2d)
                continue;
            
            Pickup(player2d);
            break;
        }
    }

    public virtual void ToggleWield(Node2D Wielder)
    {
        if (!Held)
            return;
        
        Wielded = !Wielded;

        if (Wielded)
            EmitSignal(SignalName.ItemGotWielded, Wielder);
        else
            EmitSignal(SignalName.ItemGotUnWielded, Wielder);
    }

    public virtual void Pickup(Node2D pickUper)
    {
        EmitSignal(SignalName.ItemGotPickedUp, pickUper);

        SelfModulate = new Color(1, 1, 1, 0);
        Held = true;

        Reparent(pickUper);
        GlobalPosition = pickUper.GlobalPosition;
    }

    public virtual void Drop()
    {
        EmitSignal(SignalName.ItemGotDropped);
        
        Reparent(GetParent().GetParent());

        Held = false;
        Wielded = false;
        SelfModulate = new Color(1, 1, 1, 1);
    }
}