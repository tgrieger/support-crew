using Godot;
using SupportCrew.Equipment;

[Tool]
public partial class LaserBatteryAttachment : StaticBody2D, IAddable, IRetrievable
{
	private double _durabilityPercentage;
	private Durability _durability;
	private Item _itemSlot;

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

			if (_itemSlot is not null)
			{
				_itemSlot.DurabilityPercentage = value;
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

		if (_itemSlot is not null)
		{
			return false;
		}

		_itemSlot = item;
		ItemResource = _itemSlot.ItemResource;
		_durability.Visible = true;
		DurabilityPercentage = _itemSlot.DurabilityPercentage;

		return true;
	}

	public Item RetrieveItem()
	{
		Item returnItem = _itemSlot;
		_itemSlot = null;
		ItemResource = null;
		_durability.Visible = false;

		return returnItem;
	}
}
