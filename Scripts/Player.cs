using Godot;
using System;

public partial class Player : CharacterBody3D
{
	enum MovementState {Idle, Walking, Turning, Running}
	private MovementState _CurrentState = MovementState.Idle;

	[ExportGroup("Movement")]
	[Export]
	private float _TurnSpeed = 50.0f;
	[Export]
	private float _WalkSpeed = 10.0f;
	[Export]
	private float _RunSpeed = 50.0f;

	[ExportGroup("Move Bools")]
	private bool _IsMoving;
	private bool _IsTurning;
	private Label ScoreLabel;
	private Label StaminaLabel;
	private int Score = 0;
	private float _Stamina = 100.0f;
public override void _Ready()
	{
		ScoreLabel = GetNode<Label>("ScoreLabel");
		StaminaLabel = GetNode<Label>("StaminaLabel");
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

		else if (Input.IsActionPressed("Run_Forward") && _Stamina > 0)
		{
			_CurrentState = MovementState.Running;
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

	private void HandleRunning()
	{
		
		float forwardDirection = Input.GetAxis("Run_Forward", "Move_Backward");
		float run_velocity = forwardDirection * _RunSpeed;
		Velocity = Transform.Basis.Z * run_velocity;
	}

	private void StaminaDrain(float delta)
	{
		_Stamina -= 10.0f * (float)delta;
		if (_Stamina <= 0)
		{
			_Stamina = 0;
			_CurrentState = MovementState.Idle;
			GD.Print("Out of stamina!");
		}
	}
    public override void _Process(double delta)
    {
        ScoreLabel.Text = "Score: " + Score.ToString();
		StaminaLabel.Text = "Stamina: " + _Stamina.ToString("F1");
		if (_CurrentState == MovementState.Running)
		{
			StaminaDrain((float)delta);
		}
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
				 Velocity = Vector3.Zero;
				GD.Print("Turning");
                break;

            case MovementState.Idle:
                Velocity = Vector3.Zero;
				GD.Print("Idle");
                break;

            case MovementState.Running:
                HandleRunning();
				GD.Print("Running");
                break;
        }
        MoveAndSlide();
	}
}
