using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 10; // You can set this value according to your needs
    private float bulletLifeTime = 5f; // Bullet's lifetime before it gets destroyed
    private float lifeTimer;

    private void OnEnable()
    {
        lifeTimer = bulletLifeTime;
    }

    private void Update()
    {
        lifeTimer -= Time.deltaTime;

        if (lifeTimer <= 0f)
        {
            BulletPool.Instance.ReturnBullet(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        NPC npc = other.gameObject.GetComponent<NPC>();
        if (npc != null)
        {
            Debug.Log("Hit NPC");
            npc.ReceiveDamage(damage);
            BulletPool.Instance.ReturnBullet(gameObject);
        }
    }
}
