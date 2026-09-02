using Godot;
using System;

public partial class Player : CharacterBody3D
{
	enum MovementState {Idle, Walking, Turning, Running, Shooting}
	enum AttackState {Idle, Shooting, Stabbing}
	private MovementState _CurrentState = MovementState.Idle;
	private AttackState _CurrentAttackState = AttackState.Idle;

	 [Export] 
	 private float camera_Tilt = Mathf.DegToRad(75);

	[ExportGroup("Movement")]
	[Export]
	private float _TurnSpeed = 50.0f;
	[Export]
	private float _WalkSpeed = 10.0f;
	[Export]
	private float _RunSpeed = 20.0f;
	[ExportGroup("Player")]
	[Export]
	private float _Stamina = 100.0f;
	[Export]
	private float _MaxStamina = 100.0f;
	[Export]
	private float _StaminaRegenRate = 15.0f;
	[Export]
	private bool _Exhaustion = false;
	[Export]
	private float _flipCooldown = 3.0f;

	private int _Health;
	private int _MaxHealth = 100;

	[ExportGroup("Player Bools")]
	private bool _IsMoving;
	private bool _IsTurning;
	private bool canShoot;
	private bool JustShot = false;
	private bool IsMoving;
	private bool _Flipped = false;
	private ProgressBar StaminaBar;
	private ProgressBar HealthBar;
	private ProgressBar ShootingBar;
	private int Score = 0;


	private float _shootCooldown = 2.0f;
	private float _shootTimer = 1.0f;
	private PackedScene bullet { get ; set;}
	
	private Node3D _pos;



public override void _Ready()
	{
	


		StaminaBar = GetNode<ProgressBar>("StaminaBar"); //Grabs the Node for the stamina bar from the inspector and assigns it to the StaminaBar variable
		HealthBar = GetNode<ProgressBar>("HealthBar"); // grabs the node for the Health bar from the inspector and assigns it to the HealthBar variable
		ShootingBar = GetNode<ProgressBar>("ShootBar"); // grabs the node for the shooting bar from the inspector and assigns it to the ShootingBar variable

		_Stamina = _MaxStamina; //Whatever the max stamina is set to in the inspector will be the starting stamina for the player
		_Health = _MaxHealth; // whatever the max health is set to in the inspector will be the starting health for the player
		HealthBar.Value = (float)_Health / _MaxHealth * 100; 
		canShoot = true;
		_shootTimer = _shootCooldown;
		bullet = GD.Load<PackedScene>("res://Scenes/Bullet.tscn"); // bullet var = the Bullet node that is loaded in said directory, packed scene loads the scene into memory so it can be instantiated later on when the player shoots.
		_pos = GetNode<Node3D>("Gun/POS"); // Grabs the node for the position of the bullet to spawn from, this is a child node of the gun that is attached to the player, it is used to determine where the bullet will spawn when shooting.

		// if no bullet var is found ("NULL") then it will print an error to the console, this is to help with debugging if the bullet scene is not assigned in the inspector.
		if (bullet == null)
        {
            GD.PrintErr("Failed to load bullet scene!");
        }
		 if (_pos == null)
        {
            GD.PrintErr("_pos is not assigned! Drag a Node3D into the '_pos' slot in the Inspector.");
        }

	}


public void UpdateMovement()
	{
		
		//The player will only "Run forward" if they are not exhausted, and if they are already pressing the move forward button, and if they are not currently shooting.


		// if the input for said movement is pressed then the state will be set to that movement, if not it will be set to idle
		if (Input.IsActionPressed("Run_Forward") && !_Exhaustion && Input.IsActionPressed("Move_Forward") && _CurrentAttackState != AttackState.Shooting)
		{
			_CurrentState = MovementState.Running;
		}

		// If they are not doing the above, it will check if they do this next.
		else if (Input.IsActionPressed("Move_Forward") && _CurrentAttackState != AttackState.Shooting || Input.IsActionPressed("Move_Backward") && _CurrentAttackState != AttackState.Shooting)
		{
			_CurrentState = MovementState.Walking;

			
             // if the above if statement is true, they will then do this if conditions are met.
			if (Input.IsActionPressed("Move_Backward") && _CurrentState == MovementState.Walking && Input.IsActionPressed("Flip") && !_Flipped)
			{
				HandleFlip();
			}
		} 
		
		
	}

public void UpdateAttack()
	{
		
		if (Input.IsActionPressed("Shoot") && _CurrentAttackState != AttackState.Shooting)
		{
			_CurrentAttackState = AttackState.Shooting;
		}
		else if (!Input.IsActionPressed("Shoot") && _CurrentAttackState == AttackState.Shooting)
		{
			_CurrentAttackState = AttackState.Idle;
		}
	}

	private void HandleTurning(float delta)
	{
		
			// IsMoving = true;
			float turnDirection = Input.GetAxis("Turn_Right", "Turn_Left");
			RotationDegrees = new Vector3(RotationDegrees.X, RotationDegrees.Y + (turnDirection * _TurnSpeed * delta), RotationDegrees.Z);
			Velocity = Vector3.Zero;
			
		
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
	private void HandleFlip()
	{
		RotationDegrees = new Vector3(RotationDegrees.X, RotationDegrees.Y + 180.0f, RotationDegrees.Z);
		_Flipped = true;

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

	

	private void ShootingStance(float delta)
	{
		canShoot = true;
		if (bullet == null || _pos == null) return;
		
			if(Input.IsActionJustPressed("Shooting") && canShoot && !IsMoving && !JustShot)
				{
					JustShot = true;
					var bulletInstance = bullet.Instantiate<Bullet>();
					GetTree().Root.AddChild(bulletInstance);
					bulletInstance.GlobalPosition = _pos.GlobalPosition;
					bulletInstance.GlobalRotation = _pos.GlobalRotation;
					GD.Print("Shooting!");
				}
			
				
				
	}
    public override void _Process(double delta)
    {
		ShootingBar.Value = _shootTimer / _shootCooldown * 100;
		if(JustShot && _CurrentAttackState == AttackState.Shooting)
		{
			_shootTimer -= (float)delta;
			if (_shootTimer <= 0)
			{
				JustShot = false;
				_shootTimer = _shootCooldown;
			}
		}


		StaminaBar.Value = _Stamina / _MaxStamina * 100;
		if (_CurrentState != MovementState.Running && _Stamina < _MaxStamina)
		{
			StaminaRegen((float)delta);
		}
		if (_CurrentState == MovementState.Running)
		{
			StaminaDrain((float)delta);
			
		}
		if(_Flipped)
		{
			_flipCooldown -= (float)delta;
			if (_flipCooldown <= 0)
			{
				_Flipped = false;
				_flipCooldown = 3.0f;
				GD.Print("Flip ready!");
			}
		}
		if (_CurrentAttackState != AttackState.Shooting)
			{
				canShoot = false;
			}
		if (_CurrentState == MovementState.Walking || _CurrentState == MovementState.Running)
		{
		IsMoving = true;
		}
		else
		{
		IsMoving = false;	
		}


    }

	public override void _PhysicsProcess(double delta)
	{
		UpdateMovement();
		UpdateAttack();
		if (Input.IsActionPressed("Turn_Right") && !Input.IsActionPressed("Shoot") || Input.IsActionPressed("Turn_Left") && !Input.IsActionPressed("Shoot"))
		{
		IsMoving = true;
        HandleTurning((float)delta);
		 Velocity = Vector3.Zero;
		}
		else
		IsMoving = false;

        switch (_CurrentState)
        {
			// if enum state is set then it will call the apripriate function
            case MovementState.Walking:
                HandleForward();
				GD.Print("Walking");
                break;

            case MovementState.Idle:
                Velocity = Vector3.Zero;
				GD.Print("Idle");
                break;

            case MovementState.Running:
                HandleRunning();
				GD.Print("Running");
                break;

			case MovementState.Turning:
				HandleTurning((float)delta);
				GD.Print("Turning");
				 break;

        }
        MoveAndSlide();

		switch (_CurrentAttackState)
		{
			case AttackState.Shooting:
				ShootingStance((float)delta);
				GD.Print("Shooting");
				break;
		}
	}
}
