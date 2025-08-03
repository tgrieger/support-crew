using Godot;
using SupportCrew.Equipment;

[Tool]
public partial class ItemCrate : StaticBody2D, IRetrievable
{
	[Export]
	public PackedScene Item { get; set; }

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
	


	public Item RetrieveItem()
	{
		Item item = Item.Instantiate<Item>();
		item.ItemResource = ItemResource;
		return item;
	}
}
