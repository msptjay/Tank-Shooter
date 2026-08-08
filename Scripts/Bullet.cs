using Godot;
using System;

public partial class Bullet : CharacterBody3D
{

    public float speed = 50.0f;

public override void _Process(double delta)
    {
        Position += Transform.Basis * new Vector3(0,0, speed) * (float) delta;
    }
}
