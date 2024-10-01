using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GroundTile[] levelGroundTiles;

    public static GameManager singleton;

    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        levelGroundTiles = FindObjectsOfType<GroundTile>();
    }
}
