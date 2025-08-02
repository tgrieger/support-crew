using Godot;

[Tool]
public partial class Item : StaticBody2D
{
	private double _durabilityPercentage;

	public Durability Durability { get; private set; }

	[Export]
	public double DurabilityPercentage
	{
		get => _durabilityPercentage;
		set
		{
			_durabilityPercentage = value;
			if (Durability is null)
			{
				return;
			}

			Durability.DurabilityPercentage = value;
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
		Durability = GetNode<Durability>("Durability");
		Durability.Visible = ItemResource?.HasDurability ?? false;
		Durability.DurabilityPercentage = _durabilityPercentage;
	}
}
