using System.Collections;
using UnityEngine;

public class EnemyBomberAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float wanderDuration = 5f;
    public float throwInterval = 6f;
    public GameObject bombPrefab;
    public Transform throwPoint;

    private float wanderTimer;
    private float throwTimer;
    private Vector3 wanderDirection;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ChooseNewDirection();
        wanderTimer = wanderDuration;
        throwTimer = throwInterval;
    }

    void Update()
    {
        Wander();
        HandleBombThrowing();
    }

    void Wander()
    {
        wanderTimer -= Time.deltaTime;
        rb.MovePosition(rb.position + wanderDirection * moveSpeed * Time.deltaTime);

        if (wanderTimer <= 0f)
        {
            ChooseNewDirection();
            wanderTimer = wanderDuration;
        }
    }

    void ChooseNewDirection()
    {
        float angle = Random.Range(0f, 360f);
        wanderDirection = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)).normalized;
    }

    void HandleBombThrowing()
    {
        throwTimer -= Time.deltaTime;
        if (throwTimer <= 0f)
        {
            ThrowBomb();
            throwTimer = throwInterval;
        }
    }

    void ThrowBomb()
    {
        GameObject bomb = Instantiate(bombPrefab, throwPoint.position, throwPoint.rotation);
        Rigidbody bombRb = bomb.GetComponent<Rigidbody>();
        Vector3 throwDir = transform.forward + Vector3.up * 0.5f;
        bombRb.AddForce(throwDir.normalized * 10f, ForceMode.Impulse);
    }
}
