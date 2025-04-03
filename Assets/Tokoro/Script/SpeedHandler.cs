using System.Collections;
using UnityEngine;

public class SpeedHandler : MonoBehaviour
{
    public float speedMultiplier = 1.5f;
    private float defaultSpeed;
    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        defaultSpeed = playerController.moveSpeed;
    }

    public void ActivateSpeedBoost(float duration)
    {
        StartCoroutine(SpeedBoost(duration));
    }

    private IEnumerator SpeedBoost(float duration)
    {
        playerController.moveSpeed *= speedMultiplier;
        Debug.Log("�X�s�[�h�A�b�v�I");
        yield return new WaitForSeconds(duration);
        playerController.moveSpeed = defaultSpeed;
        Debug.Log("�X�s�[�h�����ɖ߂���");
    }
}
