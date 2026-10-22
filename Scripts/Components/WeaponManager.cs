using Godot;
using System;

public partial class WeaponManager : Node
{
	enum WeaponType {Unarmed, Knife, Pistol, MachineGun}
	private WeaponType _CurrentWeaponType = WeaponType.Unarmed;
	private PickupManager pickupManager;
	
	 [ExportGroup("Weapon Nodes")]
    [Export] private PackedScene bulletScene;
    // [Export] private Marker3D muzzle;
	
	[ExportGroup("Weapon Data")]
	[Export] private WeaponData pistolData;
	[Export] private WeaponData machineGunData;
	
	private WeaponState pistolState;
	private WeaponState machineGunState;

	private WeaponState currentWeapon;
	public bool HasWeapon => currentWeapon != null;
	public WeaponData CurrentWeapon => currentWeapon?.Data;
	public int AmmoInMagazine => currentWeapon?.AmmoInMagazine ?? 0;
	public int TotalAmmo => currentWeapon?.TotalAmmo ?? 0;
	public float ShootTimer => currentWeapon?.ShootTimer ?? 0f;
	private bool hasPistol = false;
	private bool hasMachineGun = false;

	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		pistolData = GD.Load<WeaponData>("res://Resources/PistolData.tres");

   		machineGunData = GD.Load<WeaponData>("res://Resources/MachineGunData.tres" );
		if (pistolData != null)
        pistolState = new WeaponState(pistolData);

		if (machineGunData != null)
			machineGunState = new WeaponState(machineGunData);
		if (bulletScene == null)
            GD.PrintErr("Bullet scene is not assigned!");

        if (muzzle == null)
            GD.PrintErr("Muzzle is not assigned!");
		// currentWeapon = weaponData;
		
		bulletScene = GD.Load<PackedScene>("res://Scenes/Weapons/Bullet.tscn");
		
		pickupManager = GetParent().GetNode<PickupManager>("PickupManager");
	}
	private Marker3D muzzle;

	public void SetMuzzle(Marker3D newMuzzle)
	{
		muzzle = newMuzzle;
	}
		public bool PickupPistol()
		{		
		if (pistolState == null || hasPistol)
			return false;

			hasPistol = true;
			_CurrentWeaponType = WeaponType.Pistol;
			UpdateCurrentWeapon();
			GD.Print("Pistol picked up!");
			return true;
		}

		public bool PickupMachineGun()
		{
		if (machineGunState == null || hasMachineGun)
			return false;

		hasMachineGun = true;
		_CurrentWeaponType = WeaponType.MachineGun;
		UpdateCurrentWeapon();

		GD.Print("Machine gun picked up!");
		return true;
		}

	private void UpdateCurrentWeapon()
	{
		switch (_CurrentWeaponType)
		{
			case WeaponType.Pistol:
				currentWeapon = pistolState;
				break;

			case WeaponType.MachineGun:
				currentWeapon = machineGunState;
				break;

			default:
				currentWeapon = null;
				break;
		}

		if (currentWeapon != null)
			GD.Print($"Equipped: {currentWeapon.Data.weaponName}");
		else
			GD.Print("Unarmed");
	}
	public void AddAmmo(int amount)
	{
   		if (currentWeapon == null || amount <= 0)
       	 return;
   		currentWeapon.TotalAmmo = Mathf.Min(currentWeapon.TotalAmmo + amount,currentWeapon.Data.MaxAmmo);
	}
	public void Reload()
	{	
		if(currentWeapon == null)
		return;

		 if (currentWeapon.TotalAmmo <= 0)
            return;

        int spaceInMagazine = currentWeapon.Data.magazineSize - currentWeapon.AmmoInMagazine;

        if (spaceInMagazine <= 0)
            return;

        int bulletsToReload = Mathf.Min(spaceInMagazine,currentWeapon.TotalAmmo);

        currentWeapon.AmmoInMagazine += bulletsToReload;
        currentWeapon.TotalAmmo -= bulletsToReload;
	}

	public void Shoot()
	{
	if (currentWeapon == null)
	return;
	if (currentWeapon.ShootTimer > 0)
        return;

    if (currentWeapon.AmmoInMagazine <= 0)
        return;
	if (!SpawnBullet())
        return;

    currentWeapon.AmmoInMagazine--;

    currentWeapon.ShootTimer = currentWeapon.Data.shootCooldown;
		
	}
	private bool SpawnBullet()
	{
		if (bulletScene == null)
       	 return false;

   		 if (muzzle == null)
      	  return false;
		var bulletInstance = bulletScene.Instantiate<Bullet>();
		GetTree().Root.AddChild(bulletInstance);
		bulletInstance.GlobalPosition = muzzle.GlobalPosition;
		bulletInstance.GlobalRotation = muzzle.GlobalRotation;
		GD.Print("Shooting!");
		return true;
	}
	


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		if (currentWeapon != null)
		{			
		currentWeapon.ShootTimer = Mathf.Max(0f,currentWeapon.ShootTimer - (float)delta);
		}

		if (Input.IsActionJustPressed("WeaponSlot1"))
		{
		_CurrentWeaponType = WeaponType.Unarmed;
		UpdateCurrentWeapon();
		}

		if (Input.IsActionJustPressed("WeaponSlot2") && hasPistol)
		{
		_CurrentWeaponType = WeaponType.Pistol;
		UpdateCurrentWeapon();
		}

		if (Input.IsActionJustPressed("WeaponSlot3") && hasMachineGun)
		{
		_CurrentWeaponType = WeaponType.MachineGun;
		UpdateCurrentWeapon();
		}
	}
	

	
}
