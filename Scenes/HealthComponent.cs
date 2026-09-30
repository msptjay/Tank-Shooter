using Godot;
using System;

public partial class HealthComponent : Node3D
{
	// Called when the node enters the scene tree for the first time.
	private HudComponent Hud;

	private int health;
	private int max_Health = 100;
	public int Health => health;
	public override void _Ready()
	{
		Hud = GetParent().GetNode<HudComponent>("HudComponent");
		health = max_Health ;
	}


	public void TakeDamage(int Attack)
	{
		health -= Attack;


		if(health <= 0)
		{
			GetParent().QueueFree();
		}
	}
	public void HealthIncrease(int healthBonus)
	{
		health += healthBonus; // adds 25 health to the player's health count when they collide with the health pack
			Hud.UpdateHealthUI();
			if (health > max_Health) // if the player's health count exceeds the max health, set it to max health
			{
				health = max_Health;
				Hud.UpdateHealthUI();
			}
	
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
