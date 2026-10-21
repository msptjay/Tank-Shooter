using Godot;
using System;

public partial class Player : CharacterBody3D
{
	enum MovementState {Idle, Walking, Turning, Running}
	enum AttackState {Idle, Shooting, Stabbing, Reloading}
	private AttackState _CurrentAttackState = AttackState.Idle;
	private MovementState _CurrentState = MovementState.Idle;
	private HudComponent Hud;

	 [Export] 
	 private float camera_Tilt = Mathf.DegToRad(75);

	[ExportGroup("Movement")]
	[Export]
	private float _TurnSpeed = 50.0f;
	[Export]
	private float _WalkSpeed = 10.0f;
	[Export]
	private float _RunSpeed = 20.0f;
	[ExportGroup("Player Stats")]
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
	private bool draining;

	public bool Draining => draining;

	public float Stamina => _Stamina;
	public float MaxStamina => _MaxStamina;
	public bool Exhaustion => _Exhaustion;


	private WeaponManager WeaponManager;

	
	[ExportGroup("Player Bools")]
	[Export]private bool _IsTurning;

	[Export]private bool IsMoving;
	[Export]private bool _Flipped = false;


	

	private int Score = 0;

	// [ExportGroup("Pistol Stats")]

	
	// private PackedScene PistolScene;
	// private PackedScene MachineGunScene;

	// private Pistol pistol;
	// private MachineGun machineGun;

	// private PackedScene Ammo { get; set; }
	// private PackedScene HealthP { get; set; }
	// private PackedScene PistolPack { get; set; }


	 public Marker3D gunSpawnPOS;
	
	
	

	
public override void _Ready()
	{

		// MachineGunScene = GD.Load<PackedScene>("res://Scenes/Weapons/MachineGun.tscn");
		// PistolScene = GD.Load<PackedScene>("res://Scenes/Weapons/Pistol.tscn"); // Pistol var = the Pistol node that is loaded in said directory, packed scene loads the scene into memory so it can be instantiated later on when the player picks up a pistol.
		 gunSpawnPOS = GetNode<Marker3D>("GunSpawnPOS");
		// machineSpawnPOS = gunSpawnPOS;

		WeaponManager = GetNode<WeaponManager>("WeaponManager");
		Hud = GetNode<HudComponent>("HudComponent");
		
		_Stamina = _MaxStamina; //Whatever the max stamina is set to in the inspector will be the starting stamina for the player
		

	}
	
		
	public void UpdateMovement()
	{
		
		if (Input.IsActionPressed("Aiming") || Input.IsActionPressed("Shooting"))
		{
			IsMoving = false;
			_CurrentState = MovementState.Idle;
			Velocity = Vector3.Zero;
			return;
		}


		// if the input for said movement is pressed then the state will be set to that movement, if not it will be set to idle
		if (Input.IsActionPressed("Run_Forward") && !_Exhaustion && Input.IsActionPressed("Move_Forward"))
		{
			_CurrentState = MovementState.Running;
		
		}

		// If they are not doing the above, it will check if they do this next.
		else if (Input.IsActionPressed("Move_Forward") && _CurrentAttackState != AttackState.Shooting || Input.IsActionPressed("Move_Backward"))
		{
			_CurrentState = MovementState.Walking;
			
			
             // if the above if statement is true, they will then do this if conditions are met.
			if (Input.IsActionPressed("Move_Backward") && _CurrentState == MovementState.Walking && Input.IsActionPressed("Flip") && !_Flipped)
			{
				HandleFlip();
			}
		} 
		else
		{
			_CurrentState = MovementState.Idle;
		
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

	public void UpdateAttack()
	{
		if (WeaponManager.CurrentWeapon == null)
   	 	{
        _CurrentAttackState = AttackState.Idle;
        return;
		}

		if (Input.IsActionJustPressed("Reload"))
		{
			
		}

	
    if (Input.IsActionPressed("Aiming") && Input.IsActionJustPressed("Shooting") && !IsMoving)
    {
        _CurrentAttackState = AttackState.Shooting;
    }
    else
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
		draining = true;
		// StaminaBar.Modulate = new Color(0, 225, 0); // Change the color of the stamina bar to green when not exhausted
		_Stamina -= 25.0f * delta;
   		 _Stamina = Mathf.Max(_Stamina, 0);

		if (_Stamina <= 0)
		{
			_Exhaustion = true;
			_CurrentState = MovementState.Idle;
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
	

    public override void _Process(double delta)
    {

		if (_CurrentState != MovementState.Running && _Stamina < _MaxStamina)
		{
			StaminaRegen((float)delta);
		}
		if (_CurrentState == MovementState.Running)
		{
			StaminaDrain((float)delta);

			
		}
		else
		draining = false;


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
		
		
		
	}

	public override void _PhysicsProcess(double delta)
	{
		UpdateMovement();
		UpdateAttack();
		if (!Input.IsActionPressed("Aiming") && !Input.IsActionPressed("Shooting") && (Input.IsActionPressed("Turn_Right") || Input.IsActionPressed("Turn_Left")))
		{
			HandleTurning((float)delta);
			Velocity = Vector3.Zero;
		}

        switch (_CurrentState)
        {
			// if enum state is set then it will call the apripriate function
            case MovementState.Walking:
                HandleForward();
				//GD.Print("Walking");
                break;

            case MovementState.Idle:
                Velocity = Vector3.Zero;
				//GD.Print("Idle");
                break;

            case MovementState.Running:
                HandleRunning();
				//GD.Print("Running");
                break;

			case MovementState.Turning:
				HandleTurning((float)delta);
				//GD.Print("Turning");
				 break;

        }
       MoveAndSlide();
      
	  if (WeaponManager.CurrentWeapon != null)
		switch (_CurrentAttackState)
		{
			
			case AttackState.Shooting:
				// pistol.Shoot();
				// Hud.UpdateAmmoUI();
				break;
		}
	}
}
