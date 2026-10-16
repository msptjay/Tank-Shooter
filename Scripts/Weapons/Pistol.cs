using Godot;
using System;

public partial class Pistol : Node3D
{

private WeaponData weaponData;


public override void _Ready()
	{

		weaponData = GetNode<WeaponData>("res://Scenes/WeaponData.tres");
		if (weaponData == null)
		{
			GD.PrintErr("WeaponData is not assigned!");
			return;
		}

		weaponData._totalAmmo = 10;
		weaponData._magazineSize = 6;
		weaponData._maxAmmo = 60;
		weaponData._ammoInMagazine = 6;
		weaponData._shootCooldown = 0.5f;
		weaponData._shootTimer = 1f;
	}


}
	

