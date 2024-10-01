using UnityEngine;

public class GroundTile : MonoBehaviour
{
    [Header("References")]
    public MeshRenderer meshRenderer;

    public bool isColored = false;

    public void ChangeColor(Color color)
    {
        // color the tile
        meshRenderer.material.color = color;
        isColored = true;

        // check if game is complete
        GameManager.singleton.CheckLevelComplete();
    }
}
