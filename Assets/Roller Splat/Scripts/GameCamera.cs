using UnityEngine;

public class GameCamera : MonoBehaviour
{
    [Header("References")]
    public ParticleSystem winEffect;

    private void Start()
    {
        winEffect.gameObject.SetActive(false);
    }

    public void PlayWinEffect()
    {
        winEffect.gameObject.SetActive(true);
        winEffect.Play();
    }
}
