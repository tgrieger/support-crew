using Godot;

public partial class TestCharacter : CharacterBody2D
{
	[Export]
	public float Speed { get; set; } = 200.0f;

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
		HandleMovement();
		HandleInteraction();
		HandleJoining();
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
			if (collider is not ItemJoiner itemJoiner || !itemJoiner.AddItem(_heldItem))
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
			case ItemCrate itemCrate:
				Item newItem = itemCrate.GetItem();
				SetHeldItem(newItem);
				break;
			case ItemJoiner itemJoiner:
				Item retrievedItem = itemJoiner.RetrieveItem();
				SetHeldItem(retrievedItem);
				break;
		}
	}

	private void HandleJoining()
	{
		if (!Input.IsActionJustPressed("join"))
		{
			return;
		}

		GodotObject collider = _playerInteraction.GetCollider();
		if (collider is not ItemJoiner itemJoiner)
		{
			return;
		}

		itemJoiner.JoinItems();
	}

	private void SetHeldItem(Item item)
	{
		_heldItem = item;
		_heldItemSprite.Texture = _heldItem?.ItemResource.ItemTexture;
	}
}
