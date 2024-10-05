using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;
    public TrailRenderer trailRenderer;
    public GameObject[] ballModels;
    public ParticleSystem collisionEffect;
    public ParticleSystem winEffect;

    [Header("Settings")]
    public float speed = 5f;
    public int minSwipeRecognition = 500;
    public float hitObstacleDistanceTreshold = 0.5f;

    [Header("Sounds")]
    public AudioClip solveSound;
    public AudioClip collisionSound;
    public float solvePitchVariation = 0.2f;

    private bool isTraveling;
    private bool isFrozen;
    private Vector3 travelDirection;
    private Vector3 nextCollisionPosition;
    private Vector2 swipePosLastFrame;
    private Vector2 swipePosCurrentFrame;
    private Vector2 currentSwipe;
    private Color solveColor;

    private int chosenModelIndex;

    public static Ball singleton;

    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        // setup effects
        winEffect.gameObject.SetActive(true);
        winEffect.Stop();
        collisionEffect.gameObject.SetActive(true);
        collisionEffect.Stop();

        // get random colors
        solveColor = Random.ColorHSV(0.5f, 1f);

        // select a random ball model
        chosenModelIndex = Random.Range(0, ballModels.Length);
        var ballModel = GetSelectedBallModel();

        // apply colors to model
        var ballModelRenderer = ballModel.GetComponent<MeshRenderer>();
        ballModelRenderer.material.color = solveColor;
        trailRenderer.material.color = solveColor;

        // update the ball model
        for (int i = 0; i < ballModels.Length; i++)
        {
            // only enable the chosen model, disable the other models
            bool isChosenModel = i == chosenModelIndex;
            ballModels[i].gameObject.SetActive(isChosenModel);
        }
    }

    private GameObject GetSelectedBallModel()
    {
        return ballModels[chosenModelIndex];
    }

    private void FixedUpdate()
    {
        // handle frozen when level won
        if (isFrozen)
            return;

        // move
        if (isTraveling)
        {
            rb.velocity = speed * travelDirection;
        }

        // handle coloring tiles
        Collider[] hitColliders = Physics.OverlapSphere(transform.position - (Vector3.up / 2), 0.05f);
        foreach (Collider collider in hitColliders)
        {
            if (collider.TryGetComponent(out GroundTile tile) &&
                !tile.isColored)
            {
                tile.ChangeColor(solveColor);

                // play solve effects
                AudioManager.singleton.PlaySound2DOneShot(solveSound, pitchVariation: solvePitchVariation);
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

                // play obstacle hit effects
                AudioManager.singleton.PlaySound2DOneShot(collisionSound);
                collisionEffect.Play();
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

    public void Freeze()
    {
        isFrozen = true;
    }

    public void PlayWinEffect()
    {
        winEffect.Play();
    }
}