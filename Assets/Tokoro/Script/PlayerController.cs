using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public GameObject bombPrefab;
    public Transform throwPoint;
    public Transform cameraTransform; // カメラの Transform を取得

    private Rigidbody rb;
    private int maxBombs = 1;
    private int currentBombs = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
        if (Input.GetButtonDown("Fire1"))
        {
            ThrowBomb();
        }
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal"); // A/D または ←/→
        float v = Input.GetAxis("Vertical");   // W/S または ↑/↓

        // カメラのY軸の回転だけを考慮（X軸の傾きなどは無視）
        Vector3 forward = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z).normalized;
        Vector3 right = new Vector3(cameraTransform.right.x, 0, cameraTransform.right.z).normalized;

        // カメラの向きを基準に移動方向を決定
        Vector3 moveDirection = forward * v + right * h;

        if (moveDirection.magnitude > 0.1f)
        {
            // プレイヤーの移動
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.deltaTime);

            // プレイヤーの向きをカメラの水平方向に合わせる
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            rb.MoveRotation(targetRotation);
        }
    }

    void ThrowBomb()
    {
        if (currentBombs < maxBombs)
        {
            GameObject bomb = Instantiate(bombPrefab, throwPoint.position, throwPoint.rotation);
            Rigidbody bombRb = bomb.GetComponent<Rigidbody>();
            bombRb.AddForce(transform.forward * 10.0f, ForceMode.Impulse);

            currentBombs++;
            StartCoroutine(BombCooldown());
        }
    }

    IEnumerator BombCooldown()
    {
        yield return new WaitForSeconds(3f);
        currentBombs--;
    }

    public void IncreaseBombCapacity()
    {
        maxBombs++;
        Debug.Log("ボムの最大設置数が増加: " + maxBombs);
    }

    public IEnumerator SpeedBoost(float duration)
    {
        moveSpeed *= 1.5f;
        Debug.Log("スピードアップ！");
        yield return new WaitForSeconds(duration);
        moveSpeed /= 1.5f;
        Debug.Log("スピードが元に戻った");
    }
}
