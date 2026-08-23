using Godot;
using System;

public partial class Bullet : CharacterBody3D
{

    public float speed = 50.0f;
    public  float bulletDuration = 1.0f;
    public float timeLeft;


public override void _Ready()
    {
        timeLeft = bulletDuration;
    }
public override void _Process(double delta)
    {
        if (timeLeft <= 0)
        {
            On_Timer_timeout();
        }
        else
        {
            timeLeft -= (float)delta;
        }
    
        Position += Transform.Basis * new Vector3(0,0, speed) * (float) delta;
        
    }

public void On_Timer_timeout()
    {
        QueueFree();
    }
}
