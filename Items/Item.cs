using Godot;

[Tool]
public partial class Item : StaticBody2D
{
	private double _durabilityPercentage;
	private Durability _durability;

	[Export]
	public double DurabilityPercentage
	{
		get => _durabilityPercentage;
		set
		{
			_durabilityPercentage = value;
			if (_durability is null)
			{
				return;
			}

			_durability.DurabilityPercentage = value;
		}
	}

	[Export]
	public ItemResource ItemResource
	{
		get => GetNode<ItemSprite>("ItemSprite").ItemResource;
		set
		{
			ItemSprite node = GetNodeOrNull<ItemSprite>("ItemSprite");
			if (node != null)
			{
				node.ItemResource = value;
			}
		}
	}

	public override void _Ready()
	{
		_durability = GetNode<Durability>("Durability");
		_durability.Visible = ItemResource?.HasDurability ?? false;
		_durability.DurabilityPercentage = _durabilityPercentage;
	}
}
