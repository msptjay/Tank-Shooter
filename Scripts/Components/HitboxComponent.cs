using Godot;
using System;
using System.ComponentModel;

public partial class HitboxComponent : Area3D


{
	[Export]
	private HealthComponent health_component;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		health_component = GetNode<HealthComponent>("HealthComponent");
	}


	public void Damage(float Attack)
	{
		if(health_component != null)
		{
			health_component.TakeDamage(10);
		}
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
