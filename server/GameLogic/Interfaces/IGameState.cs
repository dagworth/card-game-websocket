namespace server.GameLogic.Interfaces;

using server.GameLogic.Entities;

public interface IGameState {
    void StartState();
    bool CanPlayCard(CardEntity card);
    void EndTurn();
}