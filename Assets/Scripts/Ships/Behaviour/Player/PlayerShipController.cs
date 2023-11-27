using Manager;
using Ship.Damageable;
using UnityEngine;

namespace PlayerShip
{
    public class PlayerShipController : MonoBehaviour
    {
        #region SERIALIZE_FIELDS
        [Header("Scriptable")]
        [SerializeField] private ShipSkinScriptable shipSkinScriptable = null;
        [SerializeField] private ShipStatsScriptable shipStatsScriptable = null;

        [Header("Scripts")]
        [SerializeField] private PlayerMovement playerMovementScript = null;
        [SerializeField] private PlayerCannonRotation playerCannonRotationScript = null;
        [SerializeField] private ShipDamageable playerShipDamageableScript = null;
        [SerializeField] private PlayerAttack playerAttackScript = null;
        #endregion

        #region UNITY_METHODS
        private void Start()
        {
            SetShip();
            SubscribeToEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }
        #endregion

        #region PUBLIC_METHODS
        private void SetShip()
        {
            playerMovementScript.SetMovementStats(shipStatsScriptable.GetShipMovementStats());
            playerCannonRotationScript.SetCannonStats(shipStatsScriptable.GetShipCannonStats());
            playerShipDamageableScript.SetDamageableStats(shipStatsScriptable.GetShipDamageableStats());
            playerAttackScript.SetAttackStats(shipStatsScriptable.GetShipCannonBallStats());
            playerShipDamageableScript.SetShipSkin(shipSkinScriptable);
        }
        #endregion

        #region PRIVATE_METHODS
        private void SubscribeToEvents()
        {
            if (playerShipDamageableScript != null)
            {
                playerShipDamageableScript.Died += GameManager.Instance.PlayerDied;
            }
        }

        private void UnsubscribeFromEvents()
        {
            if (playerShipDamageableScript != null)
            {
                playerShipDamageableScript.Died -= GameManager.Instance.PlayerDied;
            }
        }

        #endregion
    }
}
