using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tanks/Projectiles/New Tank Projectile Set", fileName = "New Tank Projectile Set")]
public class TankProjectileSet : ScriptableObject {

    private static TankProjectileSet instance;
    public bool updateProjectiles = true;
    public TankProjectile[] projectiles;

    [RuntimeInitializeOnLoadMethod]
    public static void Init()
    {
        TankProjectileSet[] sets = Resources.LoadAll<TankProjectileSet>("");
        for (int i = 0; i < sets.Length; i++)
        {
            CoroutineServer.StartCoroutine(sets[i].UpdateProjectiles());
        }
    }

    public IEnumerator UpdateProjectiles()
    {
        while (updateProjectiles) {
            for (int i = 0; i < projectiles.Length; i++)
            {
                projectiles[i].UpdateProjectile(Time.deltaTime);
            }
            yield return new WaitForEndOfFrame();
        }
    }

}
