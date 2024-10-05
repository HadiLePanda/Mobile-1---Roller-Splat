using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Sounds")]
    public AudioClip levelWinSound;
    public AudioClip gameStartSound;
    public AudioClip gameCompleteSound;

    [Header("Settings")]
    public float winSequenceTime = 2f;
    public float mainMenuMusicVolume = 0.1f;
    public float gameplayMusicVolume = 0.2f;

    private GroundTile[] levelGroundTiles;

    private Coroutine winRoutine;

    public static GameManager singleton;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetupNewLevel();
    }

    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (singleton != this)
        {
            Destroy(gameObject);
        }
    }

    private void SetupNewLevel()
    {
        // setup for main menu
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            AudioManager.singleton.SetMusicVolume(mainMenuMusicVolume);
        }

        levelGroundTiles = FindObjectsOfType<GroundTile>();
    }

    public void CheckLevelComplete()
    {
        // check if all tiles have been colored
        bool hasColoredAllTiles = true;
        for (int i = 0; i < levelGroundTiles.Length; i++)
        {
            if (levelGroundTiles[i].isColored == false)
            {
                hasColoredAllTiles = false;
                break;
            }
        }

        // load next level if this level is complete
        if (hasColoredAllTiles)
        {
            // trigger win sequence
            if (winRoutine != null)
                StopCoroutine(winRoutine);
            winRoutine = StartCoroutine(WinSequence());
        }
    }

    private IEnumerator WinSequence()
    {
        // freeze gameplay
        Ball ball = Ball.singleton;
        ball.Freeze();

        // play win effects
        AudioManager.singleton.PlaySound2DOneShot(levelWinSound);   // win sound
        ball.PlayWinEffect();                                       // player confettis
        Camera.main.GetComponent<GameCamera>().PlayWinEffect();     // camera confettis

        // if game is finished, play game won sound
        bool wasLastLevel = !NextSceneExists();
        if (wasLastLevel)
        {
            // stop music
            AudioManager.singleton.StopMusic();

            // play game complete sound
            AudioManager.singleton.PlaySound2DOneShot(gameCompleteSound);
        }

        // wait for win sequence
        var sequenceTime = wasLastLevel ? winSequenceTime + 1f : winSequenceTime;
        yield return new WaitForSeconds(sequenceTime);

        // next level
        LoadNextLevel();
    }

    private bool NextSceneExists()
    {
        // check if the next scene index doesn't exceed the last scene index
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        var nextSceneIndex = currentSceneIndex + 1;
        var lastSceneIndex = SceneManager.sceneCountInBuildSettings - 1;

        return nextSceneIndex <= lastSceneIndex;
    }

    public void LoadNextLevel()
    {
        // load the next scene if any, otherwise go back to the first scene
        if (NextSceneExists())
        {
            var nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(nextSceneIndex);

            // update music volume
            AudioManager.singleton.SetMusicVolume(gameplayMusicVolume);
        }
        else
        {
            // load main menu
            SceneManager.LoadScene(0);

            // update music volume
            AudioManager.singleton.SetMusicVolume(mainMenuMusicVolume);
            AudioManager.singleton.PlayMusic();
        }
    }
}
