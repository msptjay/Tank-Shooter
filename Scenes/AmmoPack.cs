using Godot;
using System;

public partial class AmmoPack : Area3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public void OnBodyEntered(CharacterBody3D body)
	{
		if (body is Player player)
		{
			player.Pickup(this);
			QueueFree(); // removes the ammo pack from the scene after it has been collected
		}
	}
}
