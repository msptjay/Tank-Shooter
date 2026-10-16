using Godot;
using System;

public partial class WeaponManager : Node
{

	public Resource _weaponData;
	private Node3D currentWeapon;

	public Node3D CurrentWeapon => currentWeapon;
	private Pistol pistol;
	private MachineGun machineGun;

	// private int _totalAmmo;
	// private int _magazineSize = 6;
	// private int _maxAmmo = 60;
	// private int _ammoInMagazine;
	// private float _shootCooldown = 0.5f;
	// private float _shootTimer = 0;

public int AmmoInMagazine => _ammoInMagazine;
public int TotalAmmo => _totalAmmo;
public int MagazineSize => _magazineSize;
public int MaxAmmo => _maxAmmo;

public float ShootTimer => _shootTimer;
public float ShootCooldown => _shootCooldown;
	

	// private Pistol pistol;

	private Marker3D _Muzzle;
	// private Marker3D gunSpawnPOS;
	private PackedScene _bullet { get ; set;}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		_WeaponData = GD.Load<WeaponData>("res://Scenes/WeaponData.tres");
		_bullet = GD.Load<PackedScene>("res://Scenes/Bullet.tscn"); // bullet var = the Bullet node that is loaded in said directory, packed scene loads the scene into memory so it can be instantiated later on when the player shoots.
		_Muzzle = GetNode<Marker3D>("Area3D/POS");
		
		_totalAmmo = 10;
		_ammoInMagazine = _magazineSize;

		if (_WeaponData is WeaponData weaponData == null)
		{
			GD.PrintErr("WeaponData is not assigned!");
			return;
		}
		else
		{
			GD.print(weaponData._totalAmmo);
			GD.print(weaponData._magazineSize);
			GD.print(weaponData._maxAmmo);
			GD.print(weaponData._ammoInMagazine);
			GD.print(weaponData._shootCooldown);
			GD.print(weaponData._shootTimer);
		}


		if (_bullet == null)
        {
            GD.PrintErr("Failed to load bullet scene!");
        }
	}

public void currentGun()
	{
		if (pistol != null)
		{
			currentWeapon = pistol;
		}
		else
		{
			currentWeapon = null;
		}
	}

	
	public void AddAmmo(int amount)
	{
    	_totalAmmo += amount;

    	if (_totalAmmo > _maxAmmo)
        	_totalAmmo = _maxAmmo;
	}
	public void Reload()
	{
		if(_totalAmmo > 0 && _ammoInMagazine < _magazineSize)
		{
		int bulletsToReload = _magazineSize - _ammoInMagazine;
		if (_totalAmmo >= bulletsToReload)
		{
			_totalAmmo -= bulletsToReload;
			_ammoInMagazine = _magazineSize;
		}
		else
		{
			_ammoInMagazine += _totalAmmo;
			_totalAmmo = 0;
		}
		}
	}

	public void Shoot()
	{
		
	if (_shootTimer > 0)
        return;

    if (_ammoInMagazine <= 0)
        return;
	if (!SpawnBullet())
        return;

    _ammoInMagazine--;

    SpawnBullet();

    _shootTimer = _shootCooldown;
		
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

		 if (_shootTimer > 0)
        _shootTimer -= (float)delta;
		 if (_shootTimer < 0)
            _shootTimer = 0;
	}
	

	
}
