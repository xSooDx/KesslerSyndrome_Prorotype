using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using SoodUtils;

namespace Shatter
{
    public class Shatterable2D : MonoBehaviour
    {
        [AssetsOnly]
        public Shatterable2D shatterObject;

        public int shatterCount = 2;
        public float shatterRadius = 0.05f;
        public UnityEvent<ShatterData2D> onShatter;

        public bool shatterOnCollision = true;
        public float minShaterForce = 1f;
        private Rigidbody2D rb2D;
        
        public LayerMask noShatterMask;

        [HideInInspector] public bool canShatter = true;

        // ToDo Move to a better place
        public static int noCollisionLayer = 31;

        Vector2 velocity;

        void Awake()
        {
            rb2D = GetComponent<Rigidbody2D>();
            gameObject.name += $"{Random.Range(1, 100)}";
        }

        private void FixedUpdate()
        {
            velocity = rb2D.velocity;
        }

        public IEnumerator Shatter(ShatterData2D shatterData)
        {
            if (!canShatter) yield break;
            // ToDo: Object Pooling
            this.gameObject.SetActive(false);

            if (shatterObject == null) yield break;

            yield return null;

            onShatter.Invoke(shatterData);
            Shatterable2D[] shatterPieces = new Shatterable2D[shatterCount];
            LayerMask[] oldMasks = new LayerMask[shatterCount];
            for (int i = 0; i < shatterCount; i++)
            {
                Vector2 spawnVariance = Random.insideUnitCircle * shatterRadius;
                Shatterable2D shatter = Instantiate(shatterObject, transform.position + (Vector3)spawnVariance, Quaternion.identity);

                if (shatter.rb2D != null)
                {
                    shatter.rb2D.velocity = (Vector2.one + (Random.insideUnitCircle * 0.1f)) * shatterData.velocity;

                    shatter.canShatter = false;
                    oldMasks[i] = shatter.gameObject.layer;
                    shatter.gameObject.layer = noCollisionLayer;

                    shatterPieces[i] = shatter;
                }
            }

            for (int i = 0; i < shatterCount; i++)
            {
                yield return new WaitForSeconds(Random.Range(0.25f, 0.5f));
                shatterPieces[i].canShatter = true;
                shatterPieces[i].gameObject.layer = oldMasks[i];
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if((noShatterMask & (1 << other.gameObject.layer)) > 0) return;
            if (!shatterOnCollision) return;
            if (other.relativeVelocity.magnitude / Time.fixedDeltaTime < minShaterForce) return;

            SoodHelper.StartCoroutineGlobaly(Shatter(new ShatterData2D(this.velocity)));
        }
    }
}

[System.Serializable]
public struct ShatterData2D
{
    public Vector2 velocity;

    public ShatterData2D(Vector2 velocity)
    {
        this.velocity = velocity;
    }
}