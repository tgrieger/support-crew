using Godot;
using System;

public partial class Laser : Node2D
{
	private ItemSprite _objectiveItemSprite;
	private LaserBatteryAttachment _laserBatteryAttachment;
	private LaserButton _laserButton;

	[Export] public double LaserCost { get; set; } = 50;

	public override void _Ready()
	{
		// TODO don't do this, it's bad practice
		_objectiveItemSprite = GetNode<ItemSprite>("/root/Node2D/HealthControl/ObjectiveItemSprite");

		_laserBatteryAttachment = GetNode<LaserBatteryAttachment>("LaserBatteryAttachment");
		_laserButton = GetNode<LaserButton>("LaserButton");

		_laserButton.OnLaserPushed += OnLaserPushed;
	}

	private void OnLaserPushed()
	{
		if (_laserBatteryAttachment.ItemResource is null || _laserBatteryAttachment.DurabilityPercentage < LaserCost || _objectiveItemSprite.ItemResource?.Name != "Laser Beam")
		{
			// Don't fire the laser if it's not the objective
			return;
		}

		_laserBatteryAttachment.DurabilityPercentage -= LaserCost;
		_objectiveItemSprite.ItemResource = null;
	}
}
