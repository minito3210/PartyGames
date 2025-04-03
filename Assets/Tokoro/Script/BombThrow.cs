using System.Collections;
using UnityEngine;

public class BombThrower : MonoBehaviour
{
    public GameObject bombPrefab;
    public Transform throwPoint;
    public float throwForce = 10.0f;
    private int maxBombs = 1;
    private int currentBombs = 0;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            ThrowBomb();
        }
    }

    public void ThrowBomb()
    {
        if (currentBombs < maxBombs)
        {
            GameObject bomb = Instantiate(bombPrefab, throwPoint.position, throwPoint.rotation);
            Rigidbody bombRb = bomb.GetComponent<Rigidbody>();

            if (bombRb != null)
            {
                bombRb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
            }

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
        Debug.Log("É{ÉÄÇÃç≈ëÂê›íuêîÇ™ëùâ¡: " + maxBombs);
    }
}
