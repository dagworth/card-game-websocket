using Godot;
using System;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Text.Json;

using shared;
using System.Threading;

public partial class MessageHandler : Node {
	//returns int if its the id and only then
	private static Label logs;
	public static int ExecuteMessage(string message) {
		logs.Text += $"\n{message}";
		ServerEvent data = JsonSerializer.Deserialize<ServerEvent>(message);
		if (data is InformId a) {
			return a.PlayerId;
		}

		if (data is TargetOptions b) {
			List<int> targets = b.Targets; //just for reference
			return 0;
		}

		if (data is GameUpdate c) {
			foreach (ClientUpdater updater in c.Events) {
				if(updater is CardLocationUpdater cardlocation) {
					//There is a location change for a card
					if(cardlocation.Prev == CardLocations.Hand) {
						HandCardManager.Instance.removeHandCard(cardlocation.CardId);
					} else if (cardlocation.Prev == CardLocations.Board) {
						BoardCardManager.Instance.removeBoardCard(cardlocation.CardId);
					}

					if(cardlocation.Now == CardLocations.Hand) {
						HandCardManager.Instance.addHandCard(CardHandler.GetCard(cardlocation.CardId));
					} else if (cardlocation.Now == CardLocations.Board) {
						GD.Print(ClientHandler.plr_id + " " + cardlocation.CardId);
						BoardCardManager.Instance.addBoardCard(CardHandler.GetCard(cardlocation.CardId));
					}

				} else if (updater is StatUpdater stat) {
					//There is a buff that was applied to a card
					CardHandler.GetCard(stat.CardId).UpdateCard(stat.Buff, stat.Inverse);

				} else if (updater is NewCardUpdater newcard) {
					GD.Print("do new card: " + newcard.card.Id);
					//There is a new card that needs to be made or updated
					CardEntity card = CardHandler.GetCard(newcard.card.Id);
					if (card == null) {
						GD.Print("new card" + newcard.card.Id);
						CardHandler.AddCard(newcard.card);
					}  else {
						GD.Print("update card" + newcard.card.Id);
						card.UpdateCard(newcard.card);
					}

				} else if (updater is AttackActionUpdater attack) {
					//there is an attack that happened
				} else if (updater is DamageUpdater dmg) {
					//there is a card that took damage
				} else if (updater is TurnUpdater turn) {
					//there is a turn change
				} else if (updater is ToggleAttackUpdater togatk) {
					//a player toggled attack
					AttackManager.Instance.ToggleAttack(togatk.CardId, togatk.Status);
				}
			}
			return 0;
		}



		GD.Print($"updater not handled {message}");
		return 0; //never should get here
	}

	public override void _Ready() {
		logs = GetTree().Root.GetNode<Label>("Main/UI/logs");
	}

	public static void SendEndTurn() {
		EndTurnRequest clone = new();
		clone.PlayerId = ClientHandler.plr_id;
		ClientHandler.SendMessage(JsonSerializer.Serialize<ClientRequest>(clone));
	}

	public static void SendJoinQueue() {
		JoinQueueRequest clone = new();
		clone.PlayerId = ClientHandler.plr_id;
		ClientHandler.SendMessage(JsonSerializer.Serialize<ClientRequest>(clone));
	}

	public static void PlayCard(int card_id) {
		PlayCardRequest clone = new();
		clone.PlayerId = ClientHandler.plr_id;
		clone.CardId = card_id;
		ClientHandler.SendMessage(JsonSerializer.Serialize<ClientRequest>(clone));
	}

	public static void ToggleAttack(int card_id) {
		ToggleAttackRequest clone = new();
		clone.PlayerId = ClientHandler.plr_id;
		clone.UnitAttacking = card_id;
		ClientHandler.SendMessage(JsonSerializer.Serialize<ClientRequest>(clone));
	}
}
