using Constants;
using PoolSystem;
using Ship.Attack;
using UnityEngine;

namespace PlayerShip
{
    public class PlayerAttack : MonoBehaviour
    {
        #region SERIALIZE_FIELDS
        [Header("Key Shoot")]
        [SerializeField] private KeyCode keySingleShoot = KeyCode.Mouse0;
        [SerializeField] private KeyCode keyTripleShoot = KeyCode.Mouse1;

        [Header("Variables")]
        [SerializeField] private float waitToSingleShoot = 0.5f;
        [SerializeField] private float waitToSideShoot = 1f;

        [Header("Components")]
        [SerializeField] private Animator cannonAnimator = null;
        [SerializeField] private Transform singleCannon = null;
        [SerializeField] private Transform[] leftSideCannons = null;
        [SerializeField] private Transform[] rightSideCannons = null;
        #endregion

        #region PRIVATE_FIELDS
        private float delayToSingleShoot =  0.5f;
        private float delayToSideShoot =  1f;
      
        private float cannonBallSpeed = 0;
        #endregion

        #region UNITY_METHODS
        private void Update()
        {
            if (CanSingleShoot() && Input.GetKeyDown(keySingleShoot))
            {
                SingleShoot();
            }

            if (CanSideShoot() && Input.GetKeyDown(keyTripleShoot))
            {
                SideShoot();
            }
        }
        #endregion

        #region PUBLIC_METHODS
        public void SetAttackStats(ShipCannonBallStats shipCannonStats)
        {
            cannonBallSpeed = shipCannonStats.ShipCannonBallSpeed;
        }
        #endregion

        #region PRIVATE_METHODS
        private bool CanSingleShoot()
        {
            delayToSingleShoot += Time.deltaTime;
            return delayToSingleShoot > waitToSingleShoot;
        }

        private bool CanSideShoot()
        {
            delayToSideShoot += Time.deltaTime;
            return delayToSideShoot > waitToSideShoot;
        }

        private void SingleShoot()
        {
            delayToSingleShoot = 0;

            BallShoot(singleCannon);
            cannonAnimator.CrossFade(ConstantStrings.ShootCannonAnimation, 0);
        }

        private void SideShoot()
        {
            delayToSideShoot = 0;

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 mousePositionRelativeToShip = transform.InverseTransformPoint(mousePosition);

            Transform[] cannons = mousePositionRelativeToShip.x < 0 ? leftSideCannons : rightSideCannons;

            for (int i = 0; i < cannons.Length; i++)
            {
                BallShoot(cannons[i]);
            }
        }

        private void BallShoot(Transform rootTransform)
        {
            CannonBall pooledCannonBall = GenericObjectPool.Instance.GetPooledObject<CannonBall>();
            pooledCannonBall.SetCannonBall(rootTransform, cannonBallSpeed, ConstantStrings.EnemyTag);
        }
        #endregion
    }
}
