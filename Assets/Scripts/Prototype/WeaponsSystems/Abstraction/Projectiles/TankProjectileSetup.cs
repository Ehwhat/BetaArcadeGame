using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tanks/Singletons/Tank Projectile Setup")]
public class TankProjectileSetup : ScriptableObject {

    public List<TankProjectile> _tankProjectiles;

    public void Setup()
    {
        TankProjectile.poolHolder = new GameObject("Projectile Pool Holder").transform;
        for (int i = 0; i < _tankProjectiles.Count; i++)
        {
            _tankProjectiles[i].OnInit();
            CoroutineServer.StartCoroutine(UpdateProjectileEnumerator(_tankProjectiles[i]));
        }
    }

    private static IEnumerator UpdateProjectileEnumerator(TankProjectile projectile)
    {
        while (true)
        {
            projectile.UpdateProjectile(Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

}
