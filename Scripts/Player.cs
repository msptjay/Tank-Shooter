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

	private int _Health;
	private int _MaxHealth = 100;

	[ExportGroup("Player Bools")]
	private bool _IsMoving;
	private bool _IsTurning;
	// private bool canShoot;
	// private bool JustShot = false;
	private bool IsMoving;
	private bool _Flipped = false;

	[ExportGroup("UI")]
	private ProgressBar StaminaBar;
	private Label HealthLabel;
	// private ProgressBar ShootingBar;
	// private Label AmmoCountLabel;
	// private Label AmmoTotalLabel;
	// private Label CanShootLabel;

	private int Score = 0;

	[ExportGroup("Pistol Stats")]

	// private int TotalPistolBullets;	
	// private int StartingPistolBullets = 6;
	// private int MaxPistolBullets = 60;
	// private int AmmoClipSizePistol = 6;
	// private int AmmoInClipPistol;
	// private float _shootCooldown = 1.0f;
	// private float _shootTimer = 1.0f;
	//  private Node3D _pos;
	// private Marker3D gunSpawnPOS;
	private PackedScene PistolScene;
	// private PackedScene bullet { get ; set;}

	private Pistol pistol;
	private Node3D currentGun;
	private PackedScene Ammo { get; set; }
	private PackedScene Health { get; set; }
	private PackedScene PistolPack { get; set; }


	private Label AmmoCountLabel;
	private Label AmmoTotalLabel;
	private Label CanShootLabel;
	private ProgressBar ShootingBar;
	private Marker3D gunSpawnPOS;
	
	

	
public override void _Ready()
	{



		 PistolScene = GD.Load<PackedScene>("res://Scenes/Weapons/Pistol.tscn"); // Pistol var = the Pistol node that is loaded in said directory, packed scene loads the scene into memory so it can be instantiated later on when the player picks up a pistol.
		


		gunSpawnPOS = GetNode<Marker3D>("GunSpawnPOS");
		CanShootLabel = GetNode<Label>("VBoxContainer/Bool");
		ShootingBar = GetNode<ProgressBar>("VBoxContainer/ShootBar"); // grabs the node for the shooting bar from the inspector and assigns it to the ShootingBar variable
		AmmoTotalLabel= GetNode<Label>("VBoxContainer/AmmoTotalLabel"); // grabs the node for the ammo count bar from the inspector and assigns it to the AmmoCountBar variable
		AmmoCountLabel = GetNode<Label>("VBoxContainer/AmmoCountLabel"); // grabs the node for the ammo count bar from the inspector and assigns it to the AmmoCountBar variable
		//AmmoCountLabel.Text = $"Weapon:" + (AmmoInClipPistol) + "/" + (AmmoClipSizePistol);
		//AmmoTotalLabel.Text = $"Total Ammo: " + (TotalPistolBullets) + "/" + (MaxPistolBullets);
		StaminaBar = GetNode<ProgressBar>("VBoxContainer/StaminaBar"); //Grabs the Node for the stamina bar from the inspector and assigns it to the StaminaBar variable
		HealthLabel = GetNode<Label>("VBoxContainer/HealthLabel"); // grabs the node for the Health bar from the inspector and assigns it to the HealthBar variable
		_Stamina = _MaxStamina; //Whatever the max stamina is set to in the inspector will be the starting stamina for the player
		_Health = _MaxHealth; // whatever the max health is set to in the inspector will be the starting health for the player
		HealthLabel.Text = $"Health: " + _Health;
		


		Ammo = GD.Load<PackedScene>("res://Scenes/Pickups/AmmoPack.tscn"); // Ammo var = the AmmoPack node that is loaded in said directory, packed scene loads the scene into memory so it can be instantiated later on when the player picks up ammo packs.
		Health = GD.Load<PackedScene>("res://Scenes/Pickups/HealthPack.tscn"); // Health var = the HealthPack node that is loaded in said directory, packed scene loads the scene into memory so it can be instantiated later on when the player picks up health packs.
		PistolPack = GD.Load<PackedScene>("res://Scenes/Pickups/PistolPack.tscn"); // Pistol var = the PistolPack node that is loaded in said directory, packed scene loads the scene into memory so it can be instantiated later on when the player picks up a pistol.

		
		

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
		else
		{
			_CurrentState = MovementState.Idle;
		}
		
		
	}

	public void UpdateAttack()
	{
		if (pistol == null)
    {
        _CurrentAttackState = AttackState.Idle;
        return;
    }

    if (Input.IsActionJustPressed("Reload"))
    {
        pistol.Reload();
    }

    if (Input.IsActionPressed("Shoot") &&
        Input.IsActionJustPressed("Shooting") &&
        !IsMoving)
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

	public void Pickup(Area3D body)
	{
		
		if (body is AmmoPack ammoPack)
		{
			pistol.AddAmmo(10);
			 
		}

		if (body is HealthPack healthPack)
		{
			HealthLabel.Text = $"Health: " + (_Health);
			_Health += 25; // adds 25 health to the player's health count when they collide with the health pack
			if (_Health > _MaxHealth) // if the player's health count exceeds the max health, set it to max health
			{
				HealthLabel.Text = $"Health: " + (_Health);
				_Health = _MaxHealth;
			}
			 
		}
		if (body is PistolPack)
{
   			 if (pistol != null)
       		 return;

    	if (PistolScene == null)
    	{
      	  GD.PrintErr("PistolScene could not be loaded!");
      	  return;
    	}

    pistol = PistolScene.Instantiate<Pistol>();

    gunSpawnPOS.AddChild(pistol);

    pistol.GlobalTransform = gunSpawnPOS.GlobalTransform;

    GD.Print("Picked up pistol!");
}
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
		StaminaBar.Modulate = new Color(0, 225, 0); // Change the color of the stamina bar to green when not exhausted
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
		HealthLabel.Text = $"Health: " + (_Health);
	}

	

    public override void _Process(double delta)
    {
		if (_Exhaustion)
		{
			StaminaBar.Modulate = new Color(225, 0, 0); // Change the color of the stamina bar to red when exhausted

		}
		else
		{
			StaminaBar.Modulate = new Color(0, 225, 0);
		}
		if(Input.IsActionPressed("Shoot") || Input.IsActionPressed("Reload") || Input.IsActionPressed("Shooting"))
		{
		

		if (_MaxStamina >= _Stamina)
		{
		StaminaBar.Value = _Stamina / _MaxStamina * 100;
			
		}
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
		if (_CurrentState == MovementState.Walking || _CurrentState == MovementState.Running)
		{
		IsMoving = true;
		}
		else
		{
		IsMoving = false;	
		}
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
      
	  if (pistol != null)
		switch (_CurrentAttackState)
		{
			case AttackState.Shooting:
				pistol.Shoot();
				//GD.Print("Shooting");
				break;
		}
	}
}
