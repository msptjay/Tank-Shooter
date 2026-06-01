using Godot;
using System;

public partial class CharacterBody3d : CharacterBody3D
{
	enum MovementState {Idle, Walking, Turning}
	private MovementState _CurrentState = MovementState.Idle;

	[Export]
	private float _TurnSpeed = 50.0f;
	private float _WalkSpeed = 10.0f;
	private float _RunSpeed = 50.0f;

	private bool _IsMoving;
	private bool _IsTurning;

	private Label ScoreLabel;
	private int Score = 0;

public override void _Ready()
	{
		ScoreLabel = GetNode<Label>("ScoreLabel");
	}
public void UpdateState()
	{

        if (Input.IsActionPressed("Move_Forward") || Input.IsActionPressed("Move_Backward"))
		{
			_CurrentState = MovementState.Walking;
		}
		else if (Input.IsActionPressed("Turn_Right") || Input.IsActionPressed("Turn_Left"))
		{
			_CurrentState = MovementState.Turning;
		}
		else
		{
			_CurrentState = MovementState.Idle;	
		}
        
	}
	private void HandleTurning(float delta)
	{
		float turnDirection = Input.GetAxis("Turn_Right", "Turn_Left");
		RotationDegrees = new Vector3(RotationDegrees.X, RotationDegrees.Y + (turnDirection * _TurnSpeed * delta), RotationDegrees.Z);
	}
	private void HandleForward()
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
		
		UpdateState();
        switch (_CurrentState)

        {
            case MovementState.Walking:
                HandleForward();
				GD.Print("Walking");
                break;

            case MovementState.Turning:
                HandleTurning((float)delta);
				GD.Print("Turning");
                break;

            case MovementState.Idle:
                Velocity = Vector3.Zero;
				GD.Print("Idle");
                break;
        }

        MoveAndSlide();
	}
}
