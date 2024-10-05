using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager singleton;

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
    }

    // VFX
    public void SpawnEffect(ParticleSystem effect, Vector3 position)
    {
        if (effect == null)
            return;

        // spawn effect
        Instantiate(effect.gameObject, position, Quaternion.identity);
    }
}
