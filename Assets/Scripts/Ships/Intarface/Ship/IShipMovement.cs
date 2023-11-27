
namespace Ship.Interface
{
    public interface IShipMovement
    {
        public void SetMovementStats(ShipMovementStats shipMovementStats);
        public void ShipMove();
        public void ShipRotate();
    }
}
