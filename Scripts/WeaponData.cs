using Godot;
using System;
using System.ComponentModel;

[GlobalClass]
public partial class WeaponData : Resource
{
    [Export]
  	public int _totalAmmo  {get; set;}
    [Export]
    public int _magazineSize  {get; set;}	
	[Export]
	public int _maxAmmo  {get; set;}
	[Export]
	public int _ammoInMagazine  {get; set;}
	[Export]
	public float _shootCooldown  {get; set;}
	[Export]
	public float _shootTimer  {get; set;}

    public WeaponData(int totalAmmo, int magazineSize, int maxAmmo, int ammoInMagazine, float shootCooldown, float shootTimer)
    {
        _totalAmmo = totalAmmo;
        _magazineSize = magazineSize;
        _maxAmmo = maxAmmo;
        _ammoInMagazine = ammoInMagazine;
        _shootCooldown = shootCooldown;
        _shootTimer = shootTimer;

    }
  
}
