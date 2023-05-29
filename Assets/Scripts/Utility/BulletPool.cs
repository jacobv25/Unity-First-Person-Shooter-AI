using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance { get; private set; }

    public GameObject bulletPrefab;
    public int initialSize = 100;

    private Queue<GameObject> availableBullets = new Queue<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        AddBullets(initialSize);
    }

    private void AddBullets(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject bulletObject = Instantiate(bulletPrefab);
            bulletObject.SetActive(false);
            availableBullets.Enqueue(bulletObject);
        }
    }

    public GameObject GetBullet()
    {
        if (availableBullets.Count == 0)
        {
            AddBullets(initialSize);
        }

        return availableBullets.Dequeue();
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        availableBullets.Enqueue(bullet);
    }
}
