using server.GameLogic.Entities;

public abstract class CardEffect {
    //maybe add List<int> targets for OnPlay for targetting before play card
    public virtual void OnPlay(GameEntity game, PlayerEntity owner, CardEntity card) {}
    public virtual void OnSpawn(GameEntity game, PlayerEntity owner, CardEntity card) {}
    public virtual void OnDeath(GameEntity game, PlayerEntity owner, CardEntity card) {}
    public virtual void OnDraw(GameEntity game, PlayerEntity owner, CardEntity card) {}
    public virtual void OnAttack(GameEntity game, PlayerEntity owner, CardEntity card) {}
    public virtual void CustomEffect(GameEntity game, PlayerEntity owner, CardEntity card) {}

}