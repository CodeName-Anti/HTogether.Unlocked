using HTogether.Rendering;
using ImGuiNET;

namespace HTogether.Modules;

[HModule]
public class MoneyUnlockedModule() : Module("Money Unlocked", (int)BaseTabID.Money)
{
	private readonly MoneyModule moneyModule = HTogether.Instance.ModuleManager.GetModule<MoneyModule>();

	public override void RenderGUIElements()
	{
		ImGui.SeparatorText("Non host");

		if (ImGui.Button("Add##Unlocked"))
		{
			GameData.Instance.CmdAlterFundsWithoutExperience(moneyModule.Amount);
		}

		ImGui.SameLine();

		if (ImGui.Button("Remove##Unlocked"))
		{
			GameData.Instance.CmdAlterFundsWithoutExperience(-moneyModule.Amount);
		}
	}

}
