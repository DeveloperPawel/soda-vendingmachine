
namespace Player
{
    public interface IPlayerSOConsumer
    {
        void Consume(PlayerCoinUpdateEvent playerCoinUpdateEvent);
        void Consume(PlayerCoinSuccessEvent playerCoinSuccessEvent);
    }
}
