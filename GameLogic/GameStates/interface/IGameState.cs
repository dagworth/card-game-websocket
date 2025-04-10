public interface IGameState {
    void StateStarted();
    bool CanPlayCard(CardStatus card);
    void EndTurn();
}