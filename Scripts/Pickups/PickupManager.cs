using Godot;
using System;

public partial class PickupManager : Node
{
	private Player player;
	private Pistol pistol;
	private HudComponent Hud;
	private HealthComponent healthComponent;
	private PackedScene AmmoPack { get; set; }
	private PackedScene HealthPack { get; set; }
	private PackedScene PistolPack { get; set; }


public override void _Ready()
{

healthComponent = GetParent().GetNode<HealthComponent>("HealthComponent");
Hud = GetParent().GetNode<HudComponent>("HudComponent");
AmmoPack = GD.Load<PackedScene>("res://Scenes/Pickups/AmmoPack.tscn");
HealthPack = GD.Load<PackedScene>("res://Scenes/Pickups/HealthPack.tscn");
PistolPack = GD.Load<PackedScene>("res://Scenes/Pickups/PistolPack.tscn");
}  

// public void ItemPickup(Area3D body)
// 	{
// 		if(body is AmmoPack ammoPack)
// 		{
// 			if (pistol != null)
//     		{
//         		pistol.AddAmmo(10);
// 				Hud.UpdateAmmoUI();
//     		}
// 		}
// 		if (body is HealthPack healthPack)
// 		{
// 			healthComponent.HealthIncrease(25);
// 		}
		

// 	}


public void WeaponPickup(Area3D body)
	{
		if(body is PistolPack pistolPack)
		{
			// if (pistol != null)
       		//  return;

    		// if (PistolScene == null)
    		// {
      	 	// GD.PrintErr("PistolScene could not be loaded!");
      	 	//  return;
    		// }

    		// pistol = PistolScene.Instantiate<Pistol>();

    		// gunSpawnPOS.AddChild(pistol);

    		// pistol.GlobalTransform = gunSpawnPOS.GlobalTransform;

   			//  GD.Print("Picked up pistol!");
			//  UpdateAmmoUI();
		}
		if(body is MachineGunPack machinegunPack)
		{
			// if (pistol != null)
       		//  return;

    		// if (PistolScene == null)
    		// {
      	 	// GD.PrintErr("PistolScene could not be loaded!");
      	 	//  return;
    		// }

    		// pistol = PistolScene.Instantiate<Pistol>();

    		// gunSpawnPOS.AddChild(pistol);

    		// pistol.GlobalTransform = gunSpawnPOS.GlobalTransform;

   			//  GD.Print("Picked up pistol!");
			//  UpdateAmmoUI();
		}
	}
	// public void Pickup(Area3D body)
	// {
		
	// 	if (body is AmmoPack ammoPack)
	// 	{
	// 		if (pistol != null)
    // 		{
    //     		pistol.AddAmmo(10);
	// 			Hud.UpdateAmmoUI();
    // 		}
			 
	// 	}

	// 	// if (body is HealthPack healthPack)
	// 	// {
	// 	// 	_Health += 25; // adds 25 health to the player's health count when they collide with the health pack
	// 	// 	HealthLabel.Text = $"Health: " + (_Health);
	// 	// 	if (_Health > _MaxHealth) // if the player's health count exceeds the max health, set it to max health
	// 	// 	{
	// 	// 		_Health = _MaxHealth;
	// 	// 		HealthLabel.Text = $"Health: " + (_Health);
	// 	// 	}
			 
	// 	//}
	// 	if (body is PistolPack)
	// 		{
   	// 		if (pistol != null)
    //    		 return;

    // 		if (PistolScene == null)
    // 		{
    //   	 	GD.PrintErr("PistolScene could not be loaded!");
    //   	 	 return;
    // 		}

    // 		pistol = PistolScene.Instantiate<Pistol>();

    // 		gunSpawnPOS.AddChild(pistol);

    // 		pistol.GlobalTransform = gunSpawnPOS.GlobalTransform;

   	// 		 GD.Print("Picked up pistol!");
	// 		 Hud.UpdateAmmoUI();
	// 		}
	// 	if (body is MachineGunPack)
	// 		{
	// 			if(machineGun != null)
	// 			return;

	// 			if (MachineGunScene == null)
	// 			{
	// 				GD.PrintErr("Can't load in machine gun scene");
	// 				return;
	// 			}
	// 			machineGun = MachineGunScene.Instantiate<MachineGun>();
	// 			machineSpawnPOS.AddChild(machineGun);
	// 			machineGun.GlobalTransform = machineSpawnPOS.GlobalTransform;
	// 			Hud.UpdateAmmoUI();
	// 		}
		
	// }


}
