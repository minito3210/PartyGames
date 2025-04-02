using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCreateManager : MonoBehaviour
{
   [Header("ステージオブジェクト"), SerializeField]
   private GameObject m_stage;

   [Header("回転速度"), SerializeField]
   private float rotationSpeed = 30f; // 回転速度（度/秒）

   // Start is called before the first frame update
   void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      if (m_stage != null)
      {
         // 中心オブジェクトの周りを回転
         transform.RotateAround(m_stage.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
      }
   }
}
