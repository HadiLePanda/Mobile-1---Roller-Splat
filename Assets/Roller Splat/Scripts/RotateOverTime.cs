using UnityEngine;

public class RotateOverTime : MonoBehaviour
{
    [Header("Settings")]
    public float rotationSpeed = 50f;

    private void Update()
    {
        // calculate rotation amount
        float rotationAmount = rotationSpeed * Time.deltaTime;

        // rotate the object around the Y axis
        transform.Rotate(0, rotationAmount, 0);
    }
}
