using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public Transform launchPoint;
    public GameObject projectilePrefab;

    public void FireProjectile()
    {
        GameObject projectile =
            Instantiate(projectilePrefab, launchPoint.position, projectilePrefab.transform.rotation);
        Vector3 originalScale = projectile.transform.localScale;

        // flip the projectile's direction based on the direction the character is facing at the time of launch
        projectile.transform.localScale = new Vector3(originalScale.x * transform.localScale.x > 0 ? 1 : -1,
            originalScale.y, originalScale.z);
    }
}