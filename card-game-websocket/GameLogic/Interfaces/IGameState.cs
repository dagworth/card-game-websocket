public interface IGameState {
    void StartState();
    bool CanPlayCard(CardEntity card);
    void EndTurn();
}