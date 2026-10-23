using Godot;
using System;

public partial class PickupManager : Node
{
	private Player player;
    private HudComponent hud;
    private HealthComponent healthComponent;
    private WeaponManager weaponManager;
	[ExportGroup("Pickups")]
	private PackedScene AmmoPack { get; set; }
	private PackedScene HealthPack { get; set; }
	private PackedScene PistolPack { get; set; }
	private PackedScene pistolScene;
	private Node3D pistolModel;

    public override void _Ready()
    {
        player = GetParent<Player>();
        hud = GetParent().GetNode<HudComponent>("HudComponent");
        healthComponent = GetParent().GetNode<HealthComponent>("HealthComponent");
        weaponManager = GetParent().GetNode<WeaponManager>("WeaponManager");
		AmmoPack = GD.Load<PackedScene>("res://Scenes/Pickups/AmmoPack.tscn");
		HealthPack = GD.Load<PackedScene>("res://Scenes/Pickups/HealthPack.tscn");
		PistolPack = GD.Load<PackedScene>("res://Scenes/Pickups/PistolPack.tscn");
		pistolScene = GD.Load<PackedScene>("res://Scenes/Weapons/Pistol.tscn");
    }

    public void Pickup(Area3D body)
    {
        // Ammo pickup
        if (body is AmmoPack)
        {
            if (!weaponManager.HasWeapon)
            {
                GD.Print("Pick up a weapon first!");
                return;
            }

            weaponManager.AddAmmo(10);
            hud.UpdateAmmoUI();

            GD.Print("Picked up 10 ammo!");
            body.QueueFree();
            return;
        }

        // Health pickup
        if (body is HealthPack)
        {
            healthComponent.HealthIncrease(25);
			hud.UpdateHealthUI();

			body.QueueFree();
				return;
        }

        // Pistol pickup
        if (body is PistolPack)
		{
			if (weaponManager.PickupPistol())
			{
				weaponManager.SpawnPistol();
				hud.UpdateAmmoUI();
				body.QueueFree();
			}

    	return;
		}

        // Machine gun pickup
        if (body is MachineGunPack)
        {
            if (weaponManager.PickupMachineGun())
            {
                hud.UpdateAmmoUI();
                body.QueueFree();
            }
        }
    }

}
