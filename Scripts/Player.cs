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
	[ExportGroup("Stamina")]
	[Export]
	private float _Stamina = 100.0f;
	[Export]
	private float _MaxStamina = 100.0f;
	[Export]
	private float _StaminaRegenRate = 15.0f;
	[Export]
	private bool _Exhaustion = false;

	private int _Health;
	private int _MaxHealth = 100;

	[ExportGroup("Move Bools")]
	private bool _IsMoving;
	private bool _IsTurning;
	private ProgressBar StaminaBar;
	private ProgressBar HealthBar;
	private int Score = 0;
	
public override void _Ready()
	{
		// Grabs a referance for the labels that are children to the player node
		StaminaBar = GetNode<ProgressBar>("StaminaBar");
		HealthBar = GetNode<ProgressBar>("HealthBar");
		_Stamina = _MaxStamina;
		_Health = _MaxHealth;
	}
public void UpdateState()
	{
		// if the input for said movement is pressed then the state will be set to that movement, if not it will be set to idle
		 if (Input.IsActionPressed("Run_Forward") && !_Exhaustion)
		{
			_CurrentState = MovementState.Running;
		}
		else if (Input.IsActionPressed("Move_Forward") || Input.IsActionPressed("Move_Backward"))
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

	private void HandleRunning()
	{
		float forwardDirection = Input.GetAxis("Run_Forward", "Move_Backward");
		float run_velocity = forwardDirection * _RunSpeed;
		Velocity = Transform.Basis.Z * run_velocity;

	}

	private void StaminaDrain(float delta)
	{
		_Stamina -= 25.0f * (float)delta;
		if (_Stamina <= 0)
		{
			_Exhaustion = true;
			_CurrentState = MovementState.Idle;
			GD.Print("Out of stamina!");
		}
	}
	private void StaminaRegen(float delta)
	{
		if (_Stamina < _MaxStamina && _CurrentState != MovementState.Running)
		{
			_Stamina += _StaminaRegenRate * (float)delta;
			if (_Stamina >= 25.0f && _Exhaustion)
			{
				_Exhaustion = false;
				GD.Print("Stamina regenerated!");
			}
		}
	}
	private void TakeDamage(int damage)
	{
		_Health -= damage;
		if (_Health <= 0)
		{
			_Health = 0;
			GD.Print("Player is dead!");
			// You can add additional logic here for when the player dies, such as respawning or ending the game.
		}
		HealthBar.Value = (float)_Health / _MaxHealth * 100;
	}
    public override void _Process(double delta)
    {
		StaminaBar.Value = _Stamina / _MaxStamina * 100;
		if (_CurrentState != MovementState.Running)
		{
			StaminaRegen((float)delta);
		}
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

			// if enum state is set then it will call the apripriate function
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
