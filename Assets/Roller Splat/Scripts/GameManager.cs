using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GroundTile[] levelGroundTiles;

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

    private void Start()
    {
        SetupNewLevel();
    }

    private void SetupNewLevel()
    {
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
            LoadNextLevel();
        }
    }

    private void LoadNextLevel()
    {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        var nextSceneIndex = currentSceneIndex + 1;

        // check if the next scene index doesn't exceed the last scene index
        var nextSceneExists = nextSceneIndex <= SceneManager.sceneCountInBuildSettings - 1;

        // load the next scene if any, otherwise go back to the first scene
        if (nextSceneExists)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}
