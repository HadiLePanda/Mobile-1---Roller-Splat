using UnityEngine;

public class GroundTile : MonoBehaviour
{
    [Header("References")]
    public MeshRenderer meshRenderer;

    public bool isColored = false;

    public void ChangeColor(Color color)
    {
        meshRenderer.material.color = color;
        isColored = true;
    }
}
