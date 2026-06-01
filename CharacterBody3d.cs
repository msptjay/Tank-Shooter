using Godot;
using System;

public partial class CharacterBody3d : CharacterBody3D
{
	[Export]
	private float _TurnSpeed = 50.0f;
	private float _WalkSpeed = 20.0f;
	private float _RunSpeed = 150.0f;

	private Label ScoreLabel;
	private int Score = 0;


public override void _Ready()
	{
		ScoreLabel = GetNode<Label>("ScoreLabel");
	}

	private void _HandleTurn(float delta)
	{
		float yRotDegrees = RotationDegrees.Y;
		float turnDirection = Input.GetAxis("Turn_Right", "Turn_Left");
		RotationDegrees = new Vector3(RotationDegrees.X, RotationDegrees.Y + (turnDirection * _TurnSpeed * delta), RotationDegrees.Z);
	}
	private void _HandleForward(float delta)
	{
		float forwardDirection = Input.GetAxis("Move_Forward","Move_Backward");
		float walk_velocity = forwardDirection * _WalkSpeed;
		Velocity = Transform.Basis.Z * walk_velocity;
	
	}
    public override void _Process(double delta)
    {
        ScoreLabel.Text = "Score: " + Score.ToString();
    }

	public override void _PhysicsProcess(double delta)
	{
		
		_HandleTurn((float)delta);
		_HandleForward((float)delta);
		MoveAndSlide();
	
	}
}
