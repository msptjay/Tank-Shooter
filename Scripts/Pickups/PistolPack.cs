using Godot;
using System;

public partial class PistolPack : Area3D
{
	public bool monitoring = true;
	public PickupManager pickup;
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	private void OnBodyEntered(Node3D body)
	{
		if (body is Player player)
    	{
        PickupManager pickupManager = player.GetNode<PickupManager>("PickupManager");
		pickupManager.Pickup(this);

        GD.Print("Player touched pistol pack!");
    	}
	}
}
