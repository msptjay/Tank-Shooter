using Godot;
using System;

public partial class Gun : CharacterBody3D
{
    [Export] private float _BulletSpeed = 20.0f;
    [Export] private Node bullet;


    public override void _Ready()
    {
        bullet = GetNode<Node>("Barrel");
    }

    public void Shoot()
    {
       
    }
}
