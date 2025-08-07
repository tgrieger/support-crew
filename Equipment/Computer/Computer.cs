using Godot;
using SupportCrew.Equipment;

public partial class Computer : StaticBody2D, IActivatable
{
	private readonly object _lock = new();
	private Sprite2D _arrowSprite;
	private TestCharacter _playerUsing;
	private Texture2D[] _arrows;
	private Texture2D[] _inputs = new Texture2D[6];
	private int _inputsIndex;

	[Export]
	public Texture2D UpArrow { get; set; }

	[Export]
	public Texture2D RightArrow { get; set; }

	[Export]
	public Texture2D DownArrow { get; set; }

	[Export]
	public Texture2D LeftArrow { get; set; }

	public override void _Ready()
	{
		_arrowSprite = GetNode<Sprite2D>("ArrowSprite");
		_arrows = [UpArrow, RightArrow, DownArrow, LeftArrow];
	}

	public override void _Process(double delta)
	{
		if (_playerUsing is null)
		{
			return;
		}

		bool isUpPressed = Input.IsActionJustPressed(_playerUsing.PlayerResource.Up);
		bool isRightPressed = Input.IsActionJustPressed(_playerUsing.PlayerResource.Right);
		bool isDownPressed = Input.IsActionJustPressed(_playerUsing.PlayerResource.Down);
		bool isLeftPressed = Input.IsActionJustPressed(_playerUsing.PlayerResource.Left);

		if (!isUpPressed && !isRightPressed && !isDownPressed && !isLeftPressed)
		{
			// No input
			return;
		}

		// TODO improve this so you can't just spam inputs to pass
		if (isUpPressed && _inputs[_inputsIndex] == UpArrow
			|| isRightPressed && _inputs[_inputsIndex] == RightArrow
			|| isDownPressed && _inputs[_inputsIndex] == DownArrow
			|| isLeftPressed && _inputs[_inputsIndex] == LeftArrow)
		{
			IncrementInputsIndex();
		}
	}

	public bool CanActivate(TestCharacter character)
	{
		lock (_lock)
		{
			// TODO I wonder if there is a better way to do this with Input handling?
			if (_playerUsing is not null && _playerUsing != character)
			{
				return false;
			}

			_playerUsing = character;
		}

		_playerUsing.CanMove = !_playerUsing.CanMove;
		if (_playerUsing.CanMove)
		{
			// Player exiting either because they quit or they were successful with the input.
			_arrowSprite.Texture = null;
			_playerUsing = null;
			if (_inputsIndex >= _inputs.Length)
			{
				// Success!
				return true;
			}
		}
		else
		{
			SetArrows();
			_arrowSprite.Texture = _inputs[_inputsIndex];
		}

		return false;
	}

	public void Activate()
	{
	}

	private void SetArrows()
	{
		// TODO setting them so they aren't ever the same one after the other
		// This is to avoid it being hard to tell if an input was taken and it's the same arrow
		// vs an input being missed/incorrect.
		_inputsIndex = 0;
		int lastIndex = GD.RandRange(0, _arrows.Length - 1);
		_inputs[0] = _arrows[lastIndex];
		for (int i = 1; i < _inputs.Length; i++)
		{
			// Per the above comment, get a random number one less than what we want and
			// then increase the current index by that much and mod it by our total.
			// That way we'll never get the same number twice.
			lastIndex = (GD.RandRange(1, _arrows.Length - 1) + lastIndex) % _arrows.Length;
			_inputs[i] = _arrows[lastIndex];
		}
	}

	private void IncrementInputsIndex()
	{
		_inputsIndex++;
		if (_inputsIndex >= _inputs.Length)
		{
			_playerUsing.InvokeActivationEvent(this);
		}
		else
		{
			_arrowSprite.Texture = _inputs[_inputsIndex];
		}
	}
}
