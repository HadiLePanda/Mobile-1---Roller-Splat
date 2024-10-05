using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    public void Play()
    {
        // load first level
        GameManager.singleton.LoadNextLevel();

        // play game start sound
        AudioManager.singleton.PlaySound2DOneShot(GameManager.singleton.gameStartSound);
    }
}
