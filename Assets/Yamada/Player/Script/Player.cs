using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : NetworkBehaviour
{
   [Header("プレイヤーオブジェクト"), SerializeField]
   private GameObject m_playerObject;
   [Header("カメラ"), SerializeField]
   private Camera m_camera;
   [Header("移動速度"), SerializeField]
   private float m_speed;
   [Header("ジャンプ力"), SerializeField]
   private float m_jumpPower;

   Rigidbody m_rigidbody;

   private int m_jumpNum = 2;

   public bool m_isGameOver { get; private set; }

   //サーバー側で使う移動・ジャンプ状態
   private Vector3 m_moveDirection = Vector3.zero;
   private bool m_jumpRequested = false;

   // Start is called before the first frame update
   void Start()
   {
      AudioListener listener = m_camera.GetComponent<AudioListener>();

      // 自分のプレイヤーだけカメラを有効にする
      if (IsOwner && m_camera != null)
      {
         m_camera.enabled = true;
         listener.enabled = true;
      }
      else
      {
         if (m_camera != null)
         {
            m_camera.enabled = false;
            listener.enabled = false;
         }
      }
      m_rigidbody = GetComponent<Rigidbody>();
   }

   // Update is called once per frame
   void Update()
   {
      if (IsOwner)
      {
         HandleInput();

         if (m_playerObject.transform.position.y <= -3.5f)
         {
            Debug.Log("場外に落ちた");
            m_isGameOver = true;
            // 一定時間後に削除
            Destroy(m_playerObject, 2.0f);
         }
      }

      if (IsServer) //自分のPlayerObjectだけ処理
      {
         ServerUpdate();
      }
   }


      //入力処理(クライアントでのみ）
      private void HandleInput()
   {
      Vector3 moveDir = Vector3.zero;
      Transform cameraTransform = m_camera.transform;
      Vector3 forward = cameraTransform.forward;
      Vector3 right = cameraTransform.right;
      forward.y = 0;
      right.y = 0;
      forward.Normalize();
      right.Normalize();

      if (Input.GetKey(KeyCode.W)) moveDir += forward;
      if (Input.GetKey(KeyCode.S)) moveDir -= forward;
      if (Input.GetKey(KeyCode.A)) moveDir -= right;
      if (Input.GetKey(KeyCode.D)) moveDir += right;

      bool jump = false;

      if (m_jumpNum > 0 && Input.GetKeyDown(KeyCode.Space))
      {
         jump = true;
      }

      //サーバーに入力情報を送信
      SendInputToServerRpc(moveDir, jump);
   }

   //クライアント→サーバー　入力送信
   [ServerRpc]
   private void SendInputToServerRpc(Vector3 moveDir, bool jump)
   {
      m_moveDirection = moveDir.normalized;
      if (jump && m_jumpNum > 0)
      {
         m_jumpRequested = true;
         m_jumpNum -= 1;
      }
   }

   //サーバー側の物理処理
   private void ServerUpdate()
   {
      if(m_moveDirection != Vector3.zero)
      {
         Quaternion targetRotation = Quaternion.LookRotation(m_moveDirection);
         m_playerObject.transform.rotation = Quaternion.Slerp(m_playerObject.transform.rotation, targetRotation, Time.deltaTime * 10.0f);

         transform.position = transform.position + m_moveDirection * m_speed;
      }

      if(m_jumpRequested && m_jumpNum >= 0)
      {
         float jumpPower = m_jumpPower;
         if (m_jumpNum == 0)
         {
            jumpPower *= 0.5f;
         }
         m_rigidbody.AddForce(Vector3.up * jumpPower);
         m_jumpRequested = false;
      }
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.CompareTag("Laser"))
      {
         Debug.Log("レーザーに当たった");
         m_isGameOver = true;
      }
   }

   private void OnCollisionEnter(Collision collision)
   {
      if (!IsServer) return; // サーバー側でのみジャンプ数をリセット

      if (collision.gameObject.CompareTag("Ground"))
      {
         m_jumpNum = 2;
      }
   }
}
