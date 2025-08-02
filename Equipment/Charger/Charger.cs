using Godot;
using SupportCrew.Equipment;

[Tool]
public partial class Charger : StaticBody2D, IAddable, IRetrievable
{
	private Item _itemSlot;

	[Export] public double ChargingRate { get; set; } = 50;

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

	public override void _Process(double delta)
	{
		ChargeBattery(delta);
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

		item.RemoveChild(item.Durability);
		AddChild(item.Durability);

		return true;
	}

	public Item RetrieveItem()
	{
		Item returnItem = _itemSlot;
		_itemSlot = null;
		ItemResource = null;

		RemoveChild(returnItem.Durability);
		returnItem.AddChild(returnItem.Durability);

		return returnItem;
	}

	private void ChargeBattery(double delta)
	{
		if (_itemSlot is null)
		{
			return;
		}

		if (_itemSlot.DurabilityPercentage >= 100)
		{
			return;
		}

		double amountToCharge = ChargingRate * delta;
		if (_itemSlot.DurabilityPercentage + amountToCharge >= 100)
		{
			_itemSlot.DurabilityPercentage = 100;
			return;
		}

		_itemSlot.DurabilityPercentage += amountToCharge;
	}
}
