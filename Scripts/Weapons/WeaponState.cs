using Godot;
using System;

public partial class WeaponState 
{
    public WeaponData Data { get; }

    public int TotalAmmo { get; set; }
    public int AmmoInMagazine { get; set; }
    public float ShootTimer { get; set; }

    public WeaponState(WeaponData data)
    {
        Data = data;
        TotalAmmo = 40;
        AmmoInMagazine = data.magazineSize;
        ShootTimer = 0f;
    }

}
