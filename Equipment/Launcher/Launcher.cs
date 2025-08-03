using Godot;
using SupportCrew.Equipment;

[Tool]
public partial class Launcher : StaticBody2D, IAddable, IRetrievable, IActivatable
{
	[Signal]
	public delegate void ObjectiveCompletedEventHandler();
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
		if (_itemSlot is null)
		{
			return null;
		}

		Item returnItem = _itemSlot;
		_itemSlot = null;
		ItemResource = null;
		return returnItem;
	}

	public void Activate()
	{
		if (_itemSlot is null)
		{
			return;
		}

		_itemSlot.QueueFree();
		_itemSlot = null;
		ItemResource = null;
		EmitSignal(SignalName.ObjectiveCompleted);
	}
}
