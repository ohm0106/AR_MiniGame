/********************************************
 * Title    : BasketBall 3D Object Physics control and object list Management
 * Ver      : 0.01
 * Date     : 2022.02.03
 * Coder    : OHM
 *******************************************/
/********************************************
 * Title    : BasketBall 3D Object Physics control and list manager
 * Ver      : 0.01
 * Date     : 2022.02.03
 * Coder    : OHM
 *******************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ifland.main.minigame.BASKETBALL
{

    /// <summary>
    /// 생성된 농구공 3d 오브젝트의 리스트 관리 및 농구공 물리 제어 클래스 
    /// </summary>
    public class BasketBallController : MonoBehaviour
    {

        public GameObject ballPrefab; 

        private List<GameObject> balls = new List<GameObject>(); //생성된 공 리스트

        private GameObject ball;
        public Transform pos;

        [SerializeField]
        private Material[] materials;


        private void Awake()
        {
            
        }

        private void OnDestroy()
        {
            //생성된 ball 오브젝트 제거 
            if (balls != null)
            {
                foreach (var ball in balls)
                {
                    Destroy(ball);
                }
                balls = null;
            }
        }

        /// <summary>
        /// ball 3d object 생성 함수 
        /// </summary>
        /// <param name="count"> ball 생성 갯수 </param>
        /// <param name="pos">생성될 좌표</param>
        public void CreateBall(int count, Transform parent)
        {
          
           Debug.Log(">> BacketBallController :: CreateBall " + count + parent);
           for (int index = 0; index < count; index++)
            {
                GameObject instanceBall = Instantiate(ballPrefab, parent);
                //Ball 객체에 this 참조 
                Ball ballsc = instanceBall.GetComponent<Ball>();
                ballsc.Controller = this;
                balls.Add(instanceBall);
                Debug.Log(">> BacketBallController :: CreateBall " + index + " / " + count + " / " + balls[index]);
           }
        }

        /// <summary>
        /// Ball 설정 reset 함수 
        /// </summary>
        public void ResetBall()
        {
            if(balls != null)
            {
                foreach(var obj in balls)
                {
                    Ball ballsc = obj.GetComponent<Ball>();
                    ballsc.Count = 0;
                }
            }
        }

        /// <summary>
        /// Ball 오브젝트 Active Set 함수 
        /// </summary>
        /// <param name="isActive"></param>
        public void SetActiveObject(bool isActive)
        {
            foreach(var obj in balls)
            {
                if(obj.activeSelf != isActive)
                    obj.SetActive(isActive);
            }
        }


        /// <summary>
        /// Collider에 casting 된 공 오브젝트 발사 함수 
        /// </summary>
        /// <param name="force">Default value = 3f</param>
        public void ControlForce(float force = 3f)
        {
            if (ball == null)
                return;
            ball.transform.position = pos.position;
            Rigidbody rigid = ball.GetComponent<Rigidbody>();
            rigid.AddForce(new Vector3(0f, 1f, 0.6f) * force, ForceMode.Impulse); //Force range : 3f ~ 6f
            ball = null;
        }


     
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "Ball" && ball != other.gameObject)
            {
                ball = other.gameObject; // collider에 잡힌 오브젝트의 정보 저장 
            }
        }


        /// <summary>
        /// Ball mesh renderer를 현재 Grade에 맞는 Matetrial로 변경 함수 
        /// </summary>
        /// <param name="grade">Ball 현재 grade</param>
        /// <param name="mesh">Ball meshRenderer</param>
        public void ChangeMaterial(BallGrade grade , MeshRenderer mesh)
        {

            /*
               material = materials[(int)ball];
           */

            switch (grade)
            {
                case BallGrade.BASIC:
                    mesh.material = materials[0];
                    break;
                case BallGrade.SLIVER:
                    mesh.material = materials[1];
                    break;
                case BallGrade.GOLD:
                    mesh.material = materials[2];
                    break;
                case BallGrade.DIAMOND:
                    mesh.material = materials[3]; 
                    break;
            }

            Debug.Log(">>BasketBallController :: ChangeBallMaterial / grade " + grade + "/mesh Material" + mesh.material);
           

        }
       
    }
}
