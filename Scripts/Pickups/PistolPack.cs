using Godot;
using System;

public partial class PistolPack : Area3D
{
	public bool monitoring = true;
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
			player.Pickup(this);
			GD.Print("Picked up pistol pack!");
			QueueFree();
		}
		
	}
}
