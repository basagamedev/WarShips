using Constants;
using PoolSystem;
using Ship.Interface;
using System.Collections;
using UnityEngine;

namespace Ship.Attack
{
    public class CannonBall : MonoBehaviour
    {
        #region SERIALIZE_FIELDS
        [Header("Components")]
        [SerializeField] private Rigidbody2D rigidBody2D = null;

        [Header("Variables")]
        [SerializeField] private float ballSpeed = 5f;
        #endregion

        #region PRIVATE_FIELDS
        private float timeReturnObjectToPool = 5f;
        private string tagToHit = "";
        private Coroutine coroutineBackToPool = null;
        #endregion

        #region PUBLIC_METHODS
        public void SetCannonBall(Transform cannon, float ballSpeed, string tagToHit)
        {
            this.transform.position = cannon.position;
            this.transform.rotation = cannon.rotation;
            this.ballSpeed = ballSpeed;
            this.tagToHit = tagToHit;
            this.gameObject.SetActive(true);
        }
        #endregion

        #region UNITY_METHODS
        private void OnEnable()
        {
            StopAutomaticallyReturnObjectToPool();
            coroutineBackToPool = StartCoroutine(AutomaticallyReturnObjectToPool());
        }

        private void OnDisable()
        {
            StopAutomaticallyReturnObjectToPool();
        }

        private void FixedUpdate()
        {
            rigidBody2D.velocity = transform.up * ballSpeed * Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(tagToHit))
            {
                HitEnemy(collision);
            }
        }
        #endregion

        #region PRIVATE_METHODS
        private void HitEnemy(Collider2D collision)
        {
            IShipDamageable shipStatus = collision.GetComponent<IShipDamageable>();

            if (shipStatus != null)
                shipStatus.TakeDamage(this.transform.position);

            ReturnObjectToPool();
        }

        private void StopAutomaticallyReturnObjectToPool()
        {
            if (coroutineBackToPool != null) StopCoroutine(coroutineBackToPool);
        }

        private void ReturnObjectToPool()
        {
            GenericObjectPool.Instance.ReturnObjectToPool(this);
            ResetVelocity();
        }

        private void ResetVelocity()
        {
            rigidBody2D.velocity = Vector2.zero;
        }
        #endregion

        #region IENUMERATOR_METHODS
        private IEnumerator AutomaticallyReturnObjectToPool()
        {
            yield return new WaitForSeconds(timeReturnObjectToPool);
            ReturnObjectToPool();
            coroutineBackToPool = null;
        }
        #endregion
    }
}