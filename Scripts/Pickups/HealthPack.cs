using Godot;
using System;

public partial class HealthPack : Area3D
{
	public bool monitoring = true;
	// Called when the node enters the scene tree for the first time.
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
			player.HealthIncrease(25);
			GD.Print("Picked up health pack!");
			QueueFree();
		}
		
	}

}
