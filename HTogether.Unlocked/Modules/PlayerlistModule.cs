using HTogether.Utils;
using ImGuiNET;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HTogether.Modules;

[HModule]
public class PlayerlistModule() : Module("Playlist", Tabs.Playerlist)
{
	private List<PlayerContainer> players = [];

	private class PlayerContainer
	{
		public string Name;
		public float Distance;

		public PlayerObjectController Player;

		public bool Selected;

		public PlayerContainer(PlayerObjectController player)
		{
			Player = player;
			UpdateData();
		}

		public void UpdateData()
		{
			Name = Player.PlayerName;
			Distance = Vector3.Distance(LobbyController.Instance.LocalplayerController.transform.position, Player.transform.position);
		}
	}

	public override void RenderGUIElements()
	{
		if (ImGui.BeginTable("playerTable", 2, ImGuiTableFlags.Borders | ImGuiTableFlags.NoBordersInBody))
		{
			ImGui.TableSetupColumn("Name");
			ImGui.TableSetupColumn("Distance");

			ImGui.TableHeadersRow();

			foreach (PlayerContainer item in players)
			{
				ImGui.TableNextRow();
				ImGui.TableNextColumn();

				ImGui.Selectable(item.Player.PlayerName, ref item.Selected, ImGuiSelectableFlags.SpanAllColumns);

				ImGui.TableNextColumn();

				ImGui.Text($"{item.Distance:F2}");
			}

			ImGui.EndTable();
		}

		if (ImGui.Button("Teleport to player"))
		{
			UnityMainThreadDispatcher.Enqueue(() =>
			{
				Vector3 newPos = players.Where(p => p.Selected).FirstOrDefault().Player.transform.position;
				LobbyController.Instance.LocalplayerController.GetComponent<FirstPersonTransform>().coroutineActivator(newPos, 0f);
			});
		}

	}

	public override void Update()
	{
		if (HTogether.Instance.Renderer.CurrentTabId != Tabs.Playerlist)
			return;

		players.RemoveAll(container => !LobbyController.Instance.Manager.GamePlayers.Contains(container.Player));

		foreach (PlayerObjectController player in LobbyController.Instance.Manager.GamePlayers)
		{
			IEnumerable<PlayerContainer> result = players.Where(container => container.Player.PlayerSteamID == player.PlayerSteamID);

			if (result.Any())
			{
				result.First().UpdateData();
			}
			else
			{
				players.Add(new PlayerContainer(player));
			}
		}
	}

}
