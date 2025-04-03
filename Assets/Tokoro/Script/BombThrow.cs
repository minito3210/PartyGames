using System.Collections;
using UnityEngine;

public class BombThrower : MonoBehaviour
{
    public GameObject bombPrefab;
    public Transform throwPoint;
    public int maxBombs = 1;       // ���݂̍ő�ݒu��
    private int currentBombs = 0;  // ���ݐݒu����Ă��锚�e�̐�
    public int maxAllowedBombs = 5; // �ő勖�����{����

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
            Debug.Log("�{���̍ő吔�ɒB���܂����I");
        }
    }

    IEnumerator BombCooldown()
    {
        yield return new WaitForSeconds(3f);
        currentBombs--;
    }

    public void IncreaseBombCapacity()
    {
        if (maxBombs < maxAllowedBombs) // �ő吧���𒴂��Ȃ��悤�ɂ���
        {
            maxBombs++;
            Debug.Log("�{���̍ő�ݒu��������: " + maxBombs);
        }
        else
        {
            Debug.Log("����ȏ�{���̍ő吔�𑝂₹�܂���I");
        }
    }
}
