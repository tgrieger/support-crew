using Godot;

[Tool]
public partial class ItemJoiner : StaticBody2D
{
	private Item _itemSlot1;
	private Item _itemSlot2;
	private Item _outputItemSlot;

	[Export]
	public PackedScene Item { get; set; }

	[Export]
	public ItemResource ItemResource1
	{
		get => GetNode<ItemSprite>("ItemSprite1").ItemResource;
		set
		{
			ItemSprite node = GetNode<ItemSprite>("ItemSprite1");
			if (node != null)
			{
				node.ItemResource = value;
			}
		}
	}

	[Export]
	public ItemResource ItemResource2
	{
		get => GetNode<ItemSprite>("ItemSprite2").ItemResource;
		set
		{
			ItemSprite node = GetNode<ItemSprite>("ItemSprite2");
			if (node != null)
			{
				node.ItemResource = value;
			}
		}
	}

	[Export]
	public ItemResource OutputItemResource
	{
		get => GetNode<ItemSprite>("OutputItemSprite").ItemResource;
		set
		{
			ItemSprite node = GetNode<ItemSprite>("OutputItemSprite");
			if (node != null)
			{
				node.ItemResource = value;
			}
		}
	}

	// TODO remove once recipes exist
	[Export]
	public ItemResource TempOutputResource { get; set; }

	public bool AddItem(Item item)
	{
		if (_itemSlot1 is null)
		{
			_itemSlot1 = item;
			ItemResource1 = _itemSlot1.ItemResource;
			return true;
		}

		if (_itemSlot2 is null)
		{
			_itemSlot2 = item;
			ItemResource2 = _itemSlot2.ItemResource;
			return true;
		}

		return false;
	}

	public Item RetrieveItem()
	{
		Item returnItem = null;
		if (_itemSlot2 is not null)
		{
			returnItem = _itemSlot2;
			_itemSlot2 = null;
			ItemResource2 = null;
		}
		else if (_itemSlot1 is not null)
		{
			returnItem = _itemSlot1;
			_itemSlot1 = null;
			ItemResource1 = null;
		}
		else if (_outputItemSlot is not null)
		{
			returnItem = _outputItemSlot;
			_outputItemSlot = null;
			OutputItemResource = null;
		}

		return returnItem;
	}

	public void JoinItems()
	{
		// TODO lookup recipe
		if (_itemSlot1 is null || _itemSlot2 is null)
		{
			return;
		}

		_itemSlot1 = null;
		ItemResource1 = null;
		_itemSlot2 = null;
		ItemResource2 = null;

		_outputItemSlot = Item.Instantiate<Item>();
		_outputItemSlot.ItemResource = TempOutputResource;
		OutputItemResource = _outputItemSlot.ItemResource;
	}
}
