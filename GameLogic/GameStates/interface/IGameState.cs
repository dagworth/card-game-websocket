public interface IGameState {
    void StartState();
    bool CanPlayCard(CardStatus card);
    void EndTurn();
}