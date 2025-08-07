using Godot;
using SupportCrew.Equipment;

public partial class TestCharacter : CharacterBody2D
{
	[Export]
	public float Speed { get; set; } = 200.0f;

	[Export]
	public PlayerResource PlayerResource { get; set; }

	[Export]
	public bool IsPaused { get; set; }

	[Export]
	public bool CanMove { get; set; } = true;

	public delegate void ActivationHandler(GodotObject collider, TestCharacter character);
	public event ActivationHandler ActivationEvent;

	private AnimatedSprite2D _playerAnimatedSprite;
	private RayCast2D _playerInteraction;
	private ItemSprite _heldItemSprite;
	private Item _heldItem;

	public override void _Ready()
	{
		_playerAnimatedSprite = GetNode<AnimatedSprite2D>("PlayerAnimatedSprite");
		_playerInteraction = GetNode<RayCast2D>("PlayerInteraction");
		_heldItemSprite = GetNode<ItemSprite>("HeldItemSprite");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (IsPaused)
		{
			return;
		}

		HandleMovement();
		HandleInteraction();
		HandleActivation();
	}

	public void InvokeActivationEvent(GodotObject collider)
	{
		// TODO is there a better way to fake an activation from Computer?
		ActivationEvent?.Invoke(collider, this);
	}

	private void HandleMovement()
	{
		if (!CanMove)
		{
			return;
		}

		// Get input direction
		Vector2 inputDirection = Input.GetVector(PlayerResource.Left, PlayerResource.Right, PlayerResource.Up, PlayerResource.Down);
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
		if (!Input.IsActionJustPressed(PlayerResource.Interact))
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
		if (!Input.IsActionJustPressed(PlayerResource.Activate))
		{
			return;
		}

		GodotObject collider = _playerInteraction.GetCollider();
		ActivationEvent?.Invoke(collider, this);
	}

	private void SetHeldItem(Item item)
	{
		_heldItem = item;
		_heldItemSprite.Texture = _heldItem?.ItemResource.ItemTexture;
	}
}
