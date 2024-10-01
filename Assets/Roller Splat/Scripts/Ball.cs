using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;
    public MeshRenderer meshRenderer;

    [Header("Settings")]
    public float speed = 5f;
    public int minSwipeRecognition = 500;
    public float hitObstacleDistanceTreshold = 0.5f;

    private bool isTraveling;
    private Vector3 travelDirection;
    private Vector3 nextCollisionPosition;
    private Vector2 swipePosLastFrame;
    private Vector2 swipePosCurrentFrame;
    private Vector2 currentSwipe;
    private Color solveColor;

    private void Start()
    {
        // get colors
        solveColor = Random.ColorHSV(0.5f, 1);
        meshRenderer.material.color = solveColor;
    }

    private void FixedUpdate()
    {
        // move
        if (isTraveling)
        {
            rb.velocity = speed * travelDirection;
        }

        // handle coloring tiles
        Collider[] hitCollidres = Physics.OverlapSphere(transform.position - (Vector3.up / 2), 0.05f);
        foreach (Collider collider in hitCollidres)
        {
            if (collider.TryGetComponent(out GroundTile tile) &&
                !tile.isColored)
            {
                tile.ChangeColor(solveColor);
            }
        }

        // when we hit an obstacle, stop moving
        if (nextCollisionPosition != Vector3.zero)
        {
            if (Vector3.Distance(transform.position, nextCollisionPosition) < hitObstacleDistanceTreshold)
            {
                isTraveling = false;
                travelDirection = Vector3.zero;
                nextCollisionPosition = Vector3.zero;
            }
        }

        if (isTraveling)
            return;

        HandleSwipeInput();
    }

    private void HandleSwipeInput()
    {
        // pressing down
        if (Input.GetMouseButton(0))
        {
            // store current frame swipe value
            swipePosCurrentFrame = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            if (swipePosLastFrame != Vector2.zero)
            {
                currentSwipe = swipePosCurrentFrame - swipePosLastFrame;

                // make sure swipe is detected
                if (currentSwipe.sqrMagnitude < minSwipeRecognition)
                {
                    return;
                }

                currentSwipe.Normalize();

                // up & down
                if (currentSwipe.x > -0.5f && currentSwipe.x < 0.5)
                {
                    TravelTowards(currentSwipe.y > 0 ? Vector3.forward : Vector3.back);
                }

                // left & right
                if (currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    TravelTowards(currentSwipe.x > 0 ? Vector3.right : Vector3.left);
                }
            }

            // store last frame swipe value
            swipePosLastFrame = swipePosCurrentFrame;
        }

        // released input
        if (Input.GetMouseButtonUp(0))
        {
            // reset swipe values
            swipePosLastFrame = Vector2.zero;
            currentSwipe = Vector2.zero;
        }
    }

    private void TravelTowards(Vector3 direction)
    {
        travelDirection = direction;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, 100f))
        {
            nextCollisionPosition = hit.point;
        }

        isTraveling = true;
    }
}