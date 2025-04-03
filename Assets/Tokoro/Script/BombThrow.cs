using System.Collections;
using UnityEngine;

public class BombThrower : MonoBehaviour
{
    public GameObject bombPrefab;
    public Transform throwPoint;
    public int maxBombs = 1;       // 現在の最大設置数
    private int currentBombs = 0;  // 現在設置されている爆弾の数
    public int maxAllowedBombs = 5; // 最大許可されるボム数

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            ThrowBomb();
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
        else
        {
            Debug.Log("ボムの最大数に達しました！");
        }
    }

    IEnumerator BombCooldown()
    {
        yield return new WaitForSeconds(3f);
        currentBombs--;
    }

    public void IncreaseBombCapacity()
    {
        if (maxBombs < maxAllowedBombs) // 最大制限を超えないようにする
        {
            maxBombs++;
            Debug.Log("ボムの最大設置数が増加: " + maxBombs);
        }
        else
        {
            Debug.Log("これ以上ボムの最大数を増やせません！");
        }
    }
}
