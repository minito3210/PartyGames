using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public GameObject bombPrefab;
    public Transform throwPoint;

    private Rigidbody rb;
    private int maxBombs = 1; // 最大設置可能数
    private int currentBombs = 0; // 設置中のボム数

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
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(h, 0, v).normalized;

        if (moveDirection != Vector3.zero)
        {
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.deltaTime);
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            rb.MoveRotation(targetRotation);
        }
    }

    void ThrowBomb()
    {
        if (currentBombs < maxBombs) // ボムの制限を追加
        {
            GameObject bomb = Instantiate(bombPrefab, throwPoint.position, throwPoint.rotation);
            Rigidbody bombRb = bomb.GetComponent<Rigidbody>();
            bombRb.AddForce(transform.forward * 10.0f, ForceMode.Impulse);

            currentBombs++;
            StartCoroutine(BombCooldown()); // ボムが爆発したらカウントを減らす
        }
    }

    IEnumerator BombCooldown()
    {
        yield return new WaitForSeconds(3f); // 爆弾の爆発時間
        currentBombs--; // ボムが消えたら数を減らす
    }

    public void IncreaseBombCapacity()
    {
        maxBombs++;
        Debug.Log("ボムの最大設置数が増加: " + maxBombs);
    }

    public IEnumerator SpeedBoost(float duration)
    {
        moveSpeed *= 1.5f; // 速度を1.5倍に
        Debug.Log("スピードアップ！");
        yield return new WaitForSeconds(duration);
        moveSpeed /= 1.5f; // 元に戻す
        Debug.Log("スピードが元に戻った");
    }
}
