namespace server.GameLogic.Interfaces;

using server.GameLogic.Entities;
using shared;

public interface IGameState {
    void StartState();
    bool CanPlayCard(CardEntity card);
    void EndTurn(EndTurnRequest req);
}