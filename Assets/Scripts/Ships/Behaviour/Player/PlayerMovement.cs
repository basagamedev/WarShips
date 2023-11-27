using Ship.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerShip
{
    public class PlayerMovement : MonoBehaviour, IShipMovement
    {
        #region SERIALIZE_FIELDS
        [Header("Components")]
        [SerializeField] private Rigidbody2D rigidBody2d = null;
        #endregion

        #region PRIVATE_FIELDS
        private Vector2 movement = Vector2.zero;
        private Vector2 currentVelocity = Vector2.zero;
        private const string horizontalAxis = "Horizontal";
        private const string verticalAxis = "Vertical";
        private float maxSpeed = 0;
        private float acceleration = 0;
        private float deceleration = 0;
        private float rotationSpeed = 0;
        #endregion

        #region UNITY_METHODS
        void Update()
        {
            ShipRotate();
        }

        void FixedUpdate()
        {
            ShipMove();
        }
        #endregion

        #region PUBLIC_METHODS
        public void SetMovementStats(ShipMovementStats shipMovementStats)
        {
            maxSpeed = shipMovementStats.MaxSpeed;
            acceleration = shipMovementStats.Acceleration;
            deceleration = shipMovementStats.Deceleration;
            rotationSpeed = shipMovementStats.RotationSpeed;
        }
        #endregion

        #region INTERFACE_METHODS
        public void ShipMove()
        {
            if (movement != Vector2.zero)
            {
                currentVelocity += (Vector2)transform.up * acceleration * Time.fixedDeltaTime;
            }
            else
            {
                currentVelocity -= currentVelocity.normalized * deceleration * Time.fixedDeltaTime;
            }

            currentVelocity = Vector2.ClampMagnitude(currentVelocity, maxSpeed);
            rigidBody2d.MovePosition(rigidBody2d.position + currentVelocity * Time.fixedDeltaTime);
        }

        public void ShipRotate()
        {
            movement.x = Input.GetAxisRaw(horizontalAxis);
            movement.y = Input.GetAxisRaw(verticalAxis);

            if (movement != Vector2.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, movement);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
        }
        #endregion
    }
}
