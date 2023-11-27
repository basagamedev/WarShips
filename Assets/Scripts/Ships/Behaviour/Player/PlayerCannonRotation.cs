using UnityEngine;

namespace PlayerShip
{
    public class PlayerCannonRotation : MonoBehaviour
    {
        #region SERIALIZE_FIELDS
        [Header("Cannon")]
        [SerializeField] private Transform cannonRoot = null;
        #endregion

        #region PRIVATE_FIELDS
        private Camera mainCamera;
        private float rotationSpeed = 0;
        #endregion

        #region UNITY_METHODS
        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            CannonRotate();
        }
        #endregion

        #region PUBLIC_METHODS
        public void SetCannonStats(ShipCannonStats shipCannonStats)
        {
            rotationSpeed = shipCannonStats.ShipCannonRotationSpeed;
        }
        #endregion

        #region INTERFACE_METHODS
        public void CannonRotate()
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = mainCamera.nearClipPlane;
            Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

            Vector3 direction = mouseWorldPosition - cannonRoot.position;
            direction.z = 0;

            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);

            cannonRoot.rotation = Quaternion.Lerp(cannonRoot.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        #endregion
    }
}
