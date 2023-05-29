using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public int maxAmmo = 30; // max number of bullets per clip
    public int totalAmmo = 120; // total ammo the player has
    public float reloadTime = 2f; // reload time in seconds
    public GameObject bulletPrefab; // Prefab for the bullet
    public float bulletSpeed = 50f; // speed of bullet
    public float fireRate = 10f; // Bullets per second
    public Transform bulletSpawnPoint; // The point from which bullets are fired


    private float nextFireTime = 0f; // Time when the gun can fire again
    private int currentAmmo; // number of bullets currently in the clip
    private bool isReloading = false;

    private void Start()
    {
        // Fill the clip when game starts
        currentAmmo = maxAmmo;
    }

    public void Shoot(NPC target)
    {
        // Check if the gun can shoot
        if (!CanShoot())
            return;

        // Update the next fire time
        nextFireTime = Time.time + 1f / fireRate;

        // Shoot a bullet
        currentAmmo--;

        GameObject bulletObject = BulletPool.Instance.GetBullet();
        bulletObject.transform.position = bulletSpawnPoint.position;

        // Calculate direction to target
        Vector3 directionToTarget;
        if (target != null)
        {
            directionToTarget = (target.transform.position - bulletSpawnPoint.position).normalized;
        }
        else
        {
            directionToTarget = bulletSpawnPoint.forward;
        }
        
        bulletObject.transform.rotation = Quaternion.LookRotation(directionToTarget);
        bulletObject.SetActive(true);
        bulletObject.GetComponent<Rigidbody>().velocity = directionToTarget * bulletSpeed;

        // Start reloading if out of ammo
        if (currentAmmo <= 0)
            StartCoroutine(Reload());
    }


    private IEnumerator Reload()
    {
        if (totalAmmo <= 0)
            yield break;

        isReloading = true;

        yield return new WaitForSeconds(reloadTime);

        int ammoToReload = Mathf.Min(maxAmmo, totalAmmo);
        totalAmmo -= ammoToReload;
        currentAmmo += ammoToReload;
        
        isReloading = false;
    }

    public bool CanShoot()
    {
        // Check if it's time to fire again
        return Time.time >= nextFireTime && !isReloading && currentAmmo > 0;
    }
}
