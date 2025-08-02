using Godot;
using SupportCrew.Equipment;

public partial class TestCharacter : CharacterBody2D
{
	[Export]
	public float Speed { get; set; } = 200.0f;

	private AnimatedSprite2D _playerAnimatedSprite;
	private RayCast2D _playerInteraction;
	private ItemSprite _heldItemSprite;
	private ItemSprite _objectiveItemSprite;
	private ProgressBar _healthBar;
	private Item _heldItem;
	private bool _canMove;

	public override void _Ready()
	{
		_playerAnimatedSprite = GetNode<AnimatedSprite2D>("PlayerAnimatedSprite");
		_playerInteraction = GetNode<RayCast2D>("PlayerInteraction");
		_heldItemSprite = GetNode<ItemSprite>("HeldItemSprite");
		_healthBar = GetNode<ProgressBar>("/root/Node2D/HealthControl/HealthBar");
		// TODO don't do this, it's bad practice
		_objectiveItemSprite = GetNode<ItemSprite>("/root/Node2D/HealthControl/ObjectiveItemSprite");
		_canMove = true;
		_healthBar.ValueChanged += HealthLost;
		
	}

	public override void _PhysicsProcess(double delta)
	{
		if(_canMove == true)
		{
			HandleMovement();
			HandleInteraction();
			HandleActivation();
		}
		
	}

	private void HandleMovement()
	{
		// Get input direction
		Vector2 inputDirection = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (inputDirection == Vector2.Zero)
		{
			return;
		}

		// The player is currently a circle with a radius of 32.
		// 48 is 1.5 times that so the target position is a bit beyond the player.
		_playerInteraction.TargetPosition = inputDirection * 48;

		// Set velocity based on input and speed
		Velocity = inputDirection * Speed;
		if (inputDirection.Y < 0)
		{
			_playerAnimatedSprite.Play("Up");
		}
		else if (inputDirection.Y > 0)
		{
			_playerAnimatedSprite.Play("Down");
		}
		else if (inputDirection.X == 1)
		{
			_playerAnimatedSprite.Play("Right");
		}
		else if (inputDirection.X == -1)
		{
			_playerAnimatedSprite.Play("Left");
		}

		// Move the character and handle collisions
		MoveAndSlide();
	}

	private void HandleInteraction()
	{
		if (!Input.IsActionJustPressed("interact"))
		{
			return;
		}

		GodotObject collider = _playerInteraction.GetCollider();
		if (_heldItem is not null)
		{
			if (collider is not IAddable addable || !addable.AddItem(_heldItem))
			{
				_heldItem.GlobalPosition = GlobalPosition + _playerInteraction.TargetPosition;
				GetParent().AddChild(_heldItem);
			}

			SetHeldItem(null);
			_heldItemSprite.ItemResource = null;

			return;
		}

		switch (collider)
		{
			case Item item:
				SetHeldItem(item);
				item.GetParent().RemoveChild(item);
				_heldItemSprite.ItemResource = _heldItem?.ItemResource;
				break;
			case IRetrievable retrievable:
				Item newItem = retrievable.RetrieveItem();
				SetHeldItem(newItem);
				break;
		}
	}

	private void HandleActivation()
	{
		if (!Input.IsActionJustPressed("activate"))
		{
			return;
		}

		GodotObject collider = _playerInteraction.GetCollider();
		if (collider is not IActivatable activatable)
		{
			return;
		}

		if (activatable is Launcher launcher)
		{
			if (launcher.ItemResource != _objectiveItemSprite.ItemResource)
			{
				// Don't allow launching things that aren't the objective
				return;
			}

			_objectiveItemSprite.ItemResource = null;
		}

		activatable.Activate();
	}
	
	public void HealthLost(double newValue)
	{
		if (newValue < 5)
		{
			_canMove = false;
		}
		
	}

	private void SetHeldItem(Item item)
	{
		_heldItem = item;
		_heldItemSprite.Texture = _heldItem?.ItemResource.ItemTexture;
	}
}
