using Godot;
using System;
using SupportCrew.Equipment;

public partial class TestLevel : Node2D
{
	private HealthControl _healthControl;

	[Export]
	public PackedScene PlayerScene { get; set; }

	[Export]
	public PlayerResource PlayerOneResource { get; set; }

	[Export]
	public PlayerResource PlayerTwoResource { get; set; }

	[Export]
	public PlayerResource PlayerThreeResource { get; set; }

	[Export]
	public PlayerResource PlayerFourResource { get; set; }

	[Export]
	public AbilityResource[] AbilityResources { get; set; }

	[Export]
	public Ability[] Abilities { get; set; }

	public override void _Ready()
	{
		_healthControl = GetNode<HealthControl>("HealthControl");

		PlayerResource[] playerResources = [ PlayerOneResource, PlayerTwoResource, PlayerThreeResource, PlayerFourResource ];
		for (int i = 0; i < GameManager.Instance.NumberOfPlayers; i++)
		{
			TestCharacter player = PlayerScene.Instantiate<TestCharacter>();
			player.PlayerResource = playerResources[i];
			player.GlobalPosition = new Vector2(128 * (i + 1), 128 * (i + 1));
			player.ActivationEvent += HandleActivationEvent;
			AddChild(player);
		}

		Spawner otherScript = GetNode<Spawner>("SpawnControl");
		otherScript.RandomSpawner();

		for (int i = 0; i < Math.Min(AbilityResources.Length, Abilities.Length); i++)
		{
			Abilities[i].AbilityResource = AbilityResources[i];
			Abilities[i].OnFailure += _healthControl.HandleFailureEvent;
			_healthControl.OnPause += Abilities[i].OnPause;
			_healthControl.OnResume += Abilities[i].OnResume;
		}
	}

	private void HandleActivationEvent(GodotObject collider, TestCharacter character)
	{
		if (collider is not IActivatable activatable)
		{
			return;
		}

		if (!activatable.CanActivate(character))
		{
			return;
		}

		// TODO need a way to differentiate between equipment that completes objectives and the others?
		// We don't need to loop through abilities because the joiner will never complete an objective
		if (activatable is ItemJoiner)
		{
			activatable.Activate();
			return;
		}

		foreach (Ability ability in Abilities)
		{
			switch (activatable)
			{
				case Launcher launcher when ability.CanCompleteEvent(launcher.ItemResource.Name):
				case Laser when ability.CanCompleteEvent("Laser Beam"):
				case Computer when ability.CanCompleteEvent("Computer"):
					activatable.Activate();
					ability.CompleteEvent();
					return;
			}
		}
	}
}
