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
	private PackedScene PistolScene;
	private WeaponManager WeaponManager;
	private HudComponent hud;

public override void _Ready()
{
PistolScene = GD.Load<PackedScene>("res://Scenes/Weapons/Pistol.tscn");
healthComponent = GetParent().GetNode<HealthComponent>("HealthComponent");
Hud = GetParent().GetNode<HudComponent>("HudComponent");
AmmoPack = GD.Load<PackedScene>("res://Scenes/Pickups/AmmoPack.tscn");
HealthPack = GD.Load<PackedScene>("res://Scenes/Pickups/HealthPack.tscn");
PistolPack = GD.Load<PackedScene>("res://Scenes/Pickups/PistolPack.tscn");
WeaponManager = GetParent().GetNode<WeaponManager>("WeaponManager");
}  

public void ItemPickup(Area3D body)
	{
		if(body is AmmoPack ammoPack)
		{
			if (pistol != null)
    		{
        		WeaponManager.AddAmmo(10);
				Hud.UpdateAmmoUI();
    		}
		}
		if (body is HealthPack healthPack)
		{
			healthComponent.HealthIncrease(25);
		}
		

	}


public void WeaponPickup(Area3D body)
	{
		if(body is PistolPack pistolPack)
		{
			if (pistol != null)
       		 return;

    		if (pistol == null)
    		{
      	 	GD.PrintErr("PistolScene could not be loaded!");
      	 	 return;
    		}

    		pistol = PistolScene.Instantiate<Pistol>();

    		player.gunSpawnPOS.AddChild(pistol);

    		pistol.GlobalTransform = player.gunSpawnPOS.GlobalTransform;

   			 GD.Print("Picked up pistol!");
			hud.UpdateAmmoUI();
		}
		if(body is MachineGunPack machinegunPack)
		{
			if (pistol != null)
       		 return;

    		if (PistolScene == null)
    		{
      	 	GD.PrintErr("PistolScene could not be loaded!");
      	 	 return;
    		}

    		pistol = PistolScene.Instantiate<Pistol>();

    		player.gunSpawnPOS.AddChild(pistol);

    		pistol.GlobalTransform = player.gunSpawnPOS.GlobalTransform;

   			 GD.Print("Picked up pistol!");
			 hud.UpdateAmmoUI();
		}
	}
	public void Pickup(Area3D body)
	{
		
		if (body is AmmoPack ammoPack)
		{
			if (pistol != null)
    		{
        		WeaponManager.AddAmmo(10);
				Hud.UpdateAmmoUI();
    		}
			 
		}

		if (body is HealthPack healthPack)
		{
			healthComponent.HealthIncrease(25);			 
		}
		
	}


}
