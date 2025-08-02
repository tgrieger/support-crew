using Godot;
using SupportCrew.Equipment;

[Tool]
public partial class Charger : StaticBody2D, IAddable, IRetrievable
{
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

		return true;
	}

	public Item RetrieveItem()
	{
		Item returnItem = _itemSlot;
		_itemSlot = null;
		ItemResource = null;

		return returnItem;
	}
}
