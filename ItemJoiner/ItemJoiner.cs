using Godot;

[Tool]
public partial class ItemJoiner : StaticBody2D
{
	[Export]
	public PackedScene Item { get; set; }

	[Export]
	public ItemResource ItemResource
	{
		get => GetNode<ItemSprite>("ItemSprite").ItemResource;
		set
		{
			ItemSprite node = GetNode<ItemSprite>("ItemSprite");
			if (node != null)
			{
				node.ItemResource = value;
			}
		}
	}

	public Item JoinItems()
	{
		// TODO lookup resource to make
		Item item = Item.Instantiate<Item>();
		item.ItemResource = ItemResource;
		return item;
	}
}
