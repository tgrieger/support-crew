using Godot;
using SupportCrew.Equipment;

[Tool]
public partial class LaserBatteryAttachment : StaticBody2D, IAddable, IRetrievable
{
	private double _durabilityPercentage;
	private Durability _durability;

	public Item ItemSlot { get; private set; }

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

	[Export]
	public double DurabilityPercentage
	{
		get => _durabilityPercentage;
		set
		{
			_durabilityPercentage = value;
			if (_durability is not null)
			{
				_durability.DurabilityPercentage = value;
			}

			if (ItemSlot is not null)
			{
				ItemSlot.DurabilityPercentage = value;
			}
		}
	}

	public override void _Ready()
	{
		_durability = GetNode<Durability>("Durability");
		_durability.Visible = ItemResource?.HasDurability ?? false;
		_durability.DurabilityPercentage = _durabilityPercentage;
	}

	public bool AddItem(Item item)
	{
		if (item.ItemResource.Name != "Battery")
		{
			return false;
		}

		if (ItemSlot is not null)
		{
			return false;
		}

		ItemSlot = item;
		ItemResource = ItemSlot.ItemResource;
		_durability.Visible = true;
		DurabilityPercentage = ItemSlot.DurabilityPercentage;

		return true;
	}

	public Item RetrieveItem()
	{
		Item returnItem = ItemSlot;
		ItemSlot = null;
		ItemResource = null;
		_durability.Visible = false;

		return returnItem;
	}
}
