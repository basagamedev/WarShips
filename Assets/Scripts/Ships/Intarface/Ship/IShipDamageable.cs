using UnityEngine;

namespace Ship.Interface
{
    public interface IShipDamageable
    {
        public void SetDamageableStats(ShipDamageableStats shipDamageableStats);
        public void SetShipSkin(ShipSkinScriptable shipSkin);
        public void Die();
        public void TakeDamage(Vector2 pos);
    }
}
