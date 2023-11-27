using Constants;
using Manager;
using Ship.Interface;
using System;
using System.Collections;
using UnityEngine;

namespace Ship.Damageable
{
    public class ShipDamageable : MonoBehaviour, IShipDamageable
    {
        #region SERIALIZE_FIELDS
        [Header("Components")]
        [SerializeField] private Collider2D collider2d = null;

        [Header("SpriteRenderer")]
        [SerializeField] private SpriteRenderer spriteRendererShip = null;
        [SerializeField] private SpriteRenderer spriteRendererFlag = null;
        [SerializeField] private SpriteRenderer spriteRendererCannon = null;
        [SerializeField] private SpriteRenderer spriteRendererLifeBar = null;
        [SerializeField] private SpriteRenderer spriteRendererLife = null;

        [Header("ParticleSystems")]
        [SerializeField] private ParticleSystem piratesHitFx = null;
        [SerializeField] private ParticleSystem explosionHitFx = null;
        [SerializeField] private ParticleSystem flameHitFx = null;
        [SerializeField] private ParticleSystem dieExplosionFx = null;
        #endregion

        #region PRIVATE_VARIABLES
        private Coroutine coroutineShipDisappear = null;
        private Sprite[] shipSprites = null;
        private Sprite[] flagSprites = null;
        private int shipLife = 0;
        private int maximumLife = 0;
        private int startFlamesIndex = 2;
        private float fadeDuration = 2f;
        private float hitLife = 0;
        private bool isDead = false;
        private const int firstSpriteIndex = 0;
        private const int lastIndexSpriteAlive = 2;
        private const float maxSizeLifeBar = 1;
        #endregion

        #region PUBLIC_ACTIONS
        public Action Died = null;
        public Action ReturnToPool = null;
        #endregion

        #region PUBLIC_METHODS
        public void SetDamageableStats(ShipDamageableStats shipDamageableStats)
        {
            shipLife = shipDamageableStats.ShipLife;
            maximumLife = shipLife;
            ResetStates();
        }

        public void SetShipSkin(ShipSkinScriptable shipSkin)
        {
            shipSprites = shipSkin.ShipSkin;
            flagSprites = shipSkin.FlagSkin;
            SetSpriteShip(firstSpriteIndex);
            SetColorSpriteRenderers(Color.white);
        }
        #endregion

        #region PRIVATE_METHODS
        private void TakeDamageFX(Vector2 pos)
        {
            if (GameManager.Instance.CheckMinigameFinished()) return;

            if(!isDead)
                piratesHitFx.Play();
            
            explosionHitFx.transform.position = pos;
            explosionHitFx.Play();

            if (shipLife <= startFlamesIndex)
                flameHitFx.Play();
        }

        private void ResetStates()
        {
            EnableCollider(true);
            ResetLife();
            isDead = false;
           
        }

        private void ResetLife()
        {
            Vector2 size = spriteRendererLife.size;
            size.x = maxSizeLifeBar;
            spriteRendererLife.size = size;
            hitLife = maxSizeLifeBar / shipLife;
        }
        #endregion

        #region INTERFACE_METHODS
        public void TakeDamage(Vector2 pos)
        {
            if (isDead) return;

            shipLife--;

            TakeDamageShipSprite();
            TakeDamageFX(pos);
            ReduceLifeBar();

            if (shipLife <= 0)
                Die();
        }

        public void Die()
        {
            isDead = true;
            shipLife = 0;
            gameObject.tag = ConstantStrings.DiedTag;
            dieExplosionFx.Play();
            flameHitFx.Stop();
            SetSpriteShip(shipSprites.Length - 1);
            ReduceLifeBar();
            Died?.Invoke();
            
            if (coroutineShipDisappear != null) StopCoroutine(coroutineShipDisappear);
            coroutineShipDisappear = StartCoroutine(ShipDisappear());
        }
        #endregion

        #region PRIVATE_METHODS
        private void SetSpriteShip(int indexSprite)
        {
            spriteRendererShip.sprite = shipSprites[indexSprite];
            spriteRendererFlag.sprite = flagSprites[indexSprite];
        }

        private void TakeDamageShipSprite()
        {
            int spriteIndex = CalculateSpriteIndex();
            SetSpriteShip(spriteIndex);
        }

        private int CalculateSpriteIndex()
        {
            float lostLife = maxSizeLifeBar - ((float)shipLife / maximumLife);
            return (int)Math.Round(lostLife * (shipSprites.Length - lastIndexSpriteAlive));
        }


        private void SetColorSpriteRenderers(Color color)
        {
            spriteRendererShip.color = color;
            spriteRendererFlag.color = color;
            spriteRendererCannon.color = color;
            spriteRendererLifeBar.color = color;
            spriteRendererLife.color = color;
        }

        private void EnableCollider(bool enable)
        {
            collider2d.enabled = enable;
        }

        private void ReduceLifeBar()
        {
            Vector2 size = spriteRendererLife.size;

            if (shipLife <= 0)
            {
                size.x = 0;
            }
            else
            {
                size.x -= hitLife;
            }

            spriteRendererLife.size = size;
        }
        #endregion

        #region IENUMERATOR_METHODS
        private IEnumerator ShipDisappear()
        {
            float currentTime = 0f;
            Color color = spriteRendererShip.color;

            while (currentTime < fadeDuration)
            {
                float alpha = Mathf.Lerp(1f, 0f, currentTime / fadeDuration);
                color.a = alpha;
                SetColorSpriteRenderers(color);
                currentTime += Time.deltaTime;
                yield return null;
            }

            color.a = 0;
            SetColorSpriteRenderers(color);
            EnableCollider(false);
            ReturnToPool?.Invoke();
        }
    }
        #endregion
}

