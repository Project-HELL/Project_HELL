using UnityEngine;

public class TrackingMonster : MonoBehaviour
{
    public GameObject player;
    public float moveSpeed = 7f;
    public float delayBeforeMove = 0.5f;
    public float yCoordinateTolerance = 0.2f;
    public float xCoordinateDistance = 2f;
    public float pursuitDuration = 5f;

    private bool isPursuing = false;
    private bool isDelayed = false;
    private float pursuitTimeRemaining = 0f;
    private Renderer monsterRenderer;
    private Vector3 initialScale;
    private Animator animator;

    void Start()
    {
        monsterRenderer = GetComponent<Renderer>();
        monsterRenderer.material.color = new Color(1, 1, 1, 0.5f);
        player = GameObject.FindWithTag("Player");
        initialScale = transform.localScale;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isPursuing && Mathf.Abs(player.transform.position.y - transform.position.y) <= yCoordinateTolerance
            && Mathf.Abs(player.transform.position.x - transform.position.x) <= xCoordinateDistance)
        {
            isPursuing = true;
            monsterRenderer.material.color = new Color(1, 1, 1, 1f);
            Debug.Log("Monster has become opaque and will start pursuing after 0.5 seconds.");
        }

        if (isPursuing)
        {
            if (!isDelayed)
            {
                delayBeforeMove -= Time.deltaTime;
                if (delayBeforeMove <= 0)
                {
                    isDelayed = true;
                    pursuitTimeRemaining = pursuitDuration;
                    monsterRenderer.material.color = new Color(1, 0, 0, 1f);
                    Debug.Log("Monster is now in pursuit mode!");
                    animator.SetBool("isPursuing", true);
                }
            }
            else
            {
                bool isPlayerInDetectionRange = Mathf.Abs(player.transform.position.x - transform.position.x) <= xCoordinateDistance &&
                                                 Mathf.Abs(player.transform.position.y - transform.position.y) <= yCoordinateTolerance;

                if (isPlayerInDetectionRange)
                {
                    pursuitTimeRemaining = pursuitDuration;
                }

                if (pursuitTimeRemaining > 0)
                {
                    pursuitTimeRemaining -= Time.deltaTime;
                    Vector3 direction = (player.transform.position - transform.position).normalized;
                    transform.position += direction * moveSpeed * Time.deltaTime;
                    FlipMonster(direction);
                }
                else
                {
                    ResetMonster();
                }
            }
        }
    }

    void FlipMonster(Vector3 direction)
    {
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(initialScale.x), initialScale.y, initialScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(initialScale.x), initialScale.y, initialScale.z);
        }
    }

    void ResetMonster()
    {
        isPursuing = false;
        isDelayed = false;
        delayBeforeMove = 0.5f;
        monsterRenderer.material.color = new Color(1, 1, 1, 0.5f);
        transform.localScale = initialScale;
        animator.SetBool("isPursuing", false);
        Debug.Log("Monster has reset to its original state.");
    }
}