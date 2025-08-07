namespace SupportCrew.Equipment;

public interface IActivatable
{
	bool CanActivate(TestCharacter character);
	void Activate();
}
