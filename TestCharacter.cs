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
		HandleActivation();
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
			switch (collider)
			{
				case ItemJoiner itemJoiner when itemJoiner.AddItem(_heldItem):
				case Launcher launcher when launcher.AddItem(_heldItem):
					break;
				default:
					_heldItem.GlobalPosition = GlobalPosition + _playerInteraction.TargetPosition;
					GetParent().AddChild(_heldItem);
					break;
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
				Item joinerItem = itemJoiner.RetrieveItem();
				SetHeldItem(joinerItem);
				break;
			case Launcher launcher:
				Item launcherItem = launcher.RetrieveItem();
				SetHeldItem(launcherItem);
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
		switch (collider)
		{
			case ItemJoiner itemJoiner:
				itemJoiner.JoinItems();
				break;
			case Launcher launcher:
				launcher.LaunchItem();
				break;
		}
	}

	private void SetHeldItem(Item item)
	{
		_heldItem = item;
		_heldItemSprite.Texture = _heldItem?.ItemResource.ItemTexture;
	}
}
