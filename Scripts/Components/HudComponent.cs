using Godot;
using System;

public partial class HudComponent : Control
{

	private HealthComponent healthComponent;
	private Player player;

	[ExportGroup("UI")]
	private ProgressBar StaminaBar;
	private ProgressBar ShootCooldownBar;
	private Label HealthLabel;

	[ExportGroup("Labels")]
	private Label AmmoCountLabel;
	private Label AmmoTotalLabel;
	private Label CanShootLabel;
	private ProgressBar ShootingBar;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		healthComponent = GetParent().GetNode<HealthComponent>("HealthComponent");
		player = GetParent<Player>();


		CanShootLabel = GetNode<Label>("VBoxContainer/Bool");
		ShootingBar = GetNode<ProgressBar>("VBoxContainer/ShootBar"); // grabs the node for the shooting bar from the inspector and assigns it to the ShootingBar variable
		AmmoTotalLabel= GetNode<Label>("VBoxContainer/AmmoTotalLabel"); // grabs the node for the ammo count bar from the inspector and assigns it to the AmmoCountBar variable
		AmmoCountLabel = GetNode<Label>("VBoxContainer/AmmoCountLabel"); // grabs the node for the ammo count bar from the inspector and assigns it to the AmmoCountBar variable
		StaminaBar = GetNode<ProgressBar>("VBoxContainer/StaminaBar"); //Grabs the Node for the stamina bar from the inspector and assigns it to the StaminaBar variable
		HealthLabel = GetNode<Label>("VBoxContainer/HealthLabel"); // grabs the node for the Health bar from the inspector and assigns it to the HealthBar variable
		HealthLabel.Text = $"Health: " + healthComponent.Health;

		AmmoCountLabel.Text = $"Weapon: 0/0";
		AmmoTotalLabel.Text = $"Total Ammo: 0/0";
	}


	public void UpdateAmmoUI()
	{

	
			// if (pistol != null)
			// {
			// 	ShootingBar.Value = pistol.ShootTimer / pistol.ShootCooldown * 100;
			// 	AmmoCountLabel.Text = $"Weapon: " + pistol.AmmoInMagazine + "/" + pistol.MagazineSize;
			// 	AmmoTotalLabel.Text = $"Total Ammo: " + pistol.TotalAmmo + "/" + pistol.MaxAmmo;
			// }
			// else
			// {
			// 	AmmoCountLabel.Text = $"Weapon: 0/0";
			// 	AmmoTotalLabel.Text = $"Total Ammo: 0/0";
			// }
		
		
	}

	public void UpdateHealthUI()
	{
		HealthLabel.Text = $"Health: " + healthComponent.Health;
	}

	
	public override void _Process(double delta)
	{
		if(player.Draining)
		{
			StaminaBar.Modulate = new Color(0, 225, 0);
		}

		if (player.Exhaustion)
		{
			StaminaBar.Modulate = new Color(225, 0, 0); // Change the color of the stamina bar to red when exhausted

		}
		else
		{
			StaminaBar.Modulate = new Color(0, 225, 0);
		}
	
		

		if (player.MaxStamina >= player.Stamina)
		{
		StaminaBar.Value = player.Stamina / player.MaxStamina * 100;
			
		}
		
	}
}
