using Godot;
using System;
using System.ComponentModel;

[GlobalClass]
public partial class WeaponData : Resource 
{

    [ExportGroup("Weapon")]
    [Export]public string weaponName = "Weapon Name";
    [ExportGroup("Ammo")]
    [Export]public int magazineSize  {get; set;}	
    [Export] public int MaxAmmo { get; set; } = 120;

	[Export]public float shootCooldown  {get; set;}
	
  

 

    
}
