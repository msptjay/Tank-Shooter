using Godot;
using System;

public partial class WeaponManager : Node
{
	enum WeaponType {Unarmed, Knife, Pistol, MachineGun}
	private WeaponType _CurrentWeaponType = WeaponType.Unarmed;
	private PickupManager pickupManager;
	[ExportGroup("Weapons")]
	[Export]private Pistol pistol;
	[Export]private MachineGun machineGun;
	[Export]private PackedScene _bullet { get ; set;}
	[ExportGroup("Important Weapon stuff")]
	[Export]private WeaponData currentWeapon;
	public WeaponData CurrentWeapon => currentWeapon;
	private WeaponData weaponData;

	
private Marker3D _Muzzle;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		currentWeapon = weaponData;
		weaponData = GD.Load<WeaponData>("res://Resources/WeaponData.tres");
		_bullet = GD.Load<PackedScene>("res://Scenes/Weapons/Bullet.tscn"); // bullet var = the Bullet node that is loaded in said directory, packed scene loads the scene into memory so it can be instantiated later on when the player shoots.
		_Muzzle = GetNode<Marker3D>("Area3D/POS");
		PickupManager pickupManager = GetParent().GetNode<PickupManager>("PickupManager");

		if (_bullet == null)
        {
            GD.PrintErr("Failed to load bullet scene!");
        }
	}

public void currentGun()
	{
		if(_CurrentWeaponType == WeaponType.Unarmed)
		{
			currentWeapon = null;
		}
		
		if (_CurrentWeaponType == WeaponType.Pistol && pistol != null)
		{
			
			WeaponData PistolWeaponData = GD.Load<WeaponData>("res://Scripts/Weapons/Pistol.tres");
			currentWeapon = PistolWeaponData;
			
			if(PistolWeaponData is WeaponData weaponData)
			{
				GD.Print("Pistol Weapon Name: " + weaponData._weaponName);
				GD.Print("Pistol Total Ammo: " + weaponData._totalAmmo);
				GD.Print("Pistol Magazine Size: " + weaponData._magazineSize);
				GD.Print("Pistol Max Ammo: " + weaponData._maxAmmo);
				GD.Print("Pistol Ammo in Magazine: " + weaponData._ammoInMagazine);
				GD.Print("Pistol Shoot Cooldown: " + weaponData._shootCooldown);
				GD.Print("Pistol Shoot Timer: " + weaponData._shootTimer);
			}
			else
			{
				GD.PrintErr("Failed to load Pistol WeaponData resource!");
			}
		}
		else if (_CurrentWeaponType == WeaponType.MachineGun)
		{
			// currentWeapon = machineGun;
		}
		else
		{
			currentWeapon = null;
		}
	}
	
	public void AddAmmo(int amount)
	{
    	currentWeapon._totalAmmo += amount;

    	if (currentWeapon._totalAmmo > weaponData._maxAmmo)
        	currentWeapon._totalAmmo = weaponData._maxAmmo;
	}
	public void Reload()
	{
		if(currentWeapon._totalAmmo > 0 && currentWeapon._ammoInMagazine < currentWeapon._magazineSize)
		{
		int bulletsToReload = currentWeapon._magazineSize - currentWeapon._ammoInMagazine;
		if (currentWeapon._totalAmmo >= bulletsToReload)
		{
			currentWeapon._totalAmmo -= bulletsToReload;
			currentWeapon._ammoInMagazine = currentWeapon._magazineSize;
		}
		else
		{
			currentWeapon._ammoInMagazine += currentWeapon._totalAmmo;
			currentWeapon._totalAmmo = 0;
		}
		}
	}

	public void Shoot()
	{
		
	if (currentWeapon._shootTimer > 0)
        return;

    if (currentWeapon._ammoInMagazine <= 0)
        return;
	if (!SpawnBullet())
        return;

    currentWeapon._ammoInMagazine--;

    SpawnBullet();

    currentWeapon._shootTimer = currentWeapon._shootCooldown;
		
	}
	private bool SpawnBullet()
	{
		if (_bullet == null)
       	 return false;

   		 if (_Muzzle == null)
      	  return false;
		var bulletInstance = _bullet.Instantiate<Bullet>();
		GetTree().Root.AddChild(bulletInstance);
		bulletInstance.GlobalPosition = _Muzzle.GlobalPosition;
		bulletInstance.GlobalRotation = _Muzzle.GlobalRotation;
		GD.Print("Shooting!");
		return true;
	}
	


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		 if (currentWeapon._shootTimer > 0)
            currentWeapon._shootTimer -= (float)delta;
		 if (currentWeapon._shootTimer < 0)
            currentWeapon._shootTimer = 0;

		if(Input.IsActionPressed("WeaponSlot1") && _CurrentWeaponType != WeaponType.Unarmed)
		{
			_CurrentWeaponType = WeaponType.Unarmed;
			currentGun();
		}
		if(Input.IsActionPressed("WeaponSlot2") && _CurrentWeaponType != WeaponType.Pistol)
		{
			_CurrentWeaponType = WeaponType.Pistol;
			currentGun();
		}
	}
	

	
}
