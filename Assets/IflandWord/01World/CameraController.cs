/********************************************
 * Title    : Camera Controller 
 * Ver      : 0.01
 * Date     : 2022.02.03
 * Coder    : OHM
 *******************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ifland.main
{
    /// <summary>
    /// 카메라 위치 이동 및 회전 제어 
    /// </summary>
    public class CameraController : MonoBehaviour
    {

        [SerializeField] private GameObject target; 
        [SerializeField] private float speed; 


        public Transform cameraTransform; //카메라 위치 
        public float cameraSensitivity;

        [HideInInspector]
        public UnityAction<bool, MiniGame> CheckMiniGameStartEvent; // miniGame이 시작하고 끝났음을 전달받는 Event
  
        private Animator objectAnimator;
        private void Awake()
        {
            Utils.cameraOffset = this.transform.position; //카메라 시작 위치 저장
            CameraPosReset(); //카메라 벡터 및 위치 세팅 
            CheckMiniGameStartEvent += SetCameraMiniGameVersion; //미니게임 상태 이벤트 구독 
        }

        private void Start()
        {
        

        }
        private void OnDestroy()
        {
            CheckMiniGameStartEvent -= SetCameraMiniGameVersion; //미니게임 상태 이벤트 구독 취소
        }

        /// <summary>
        /// 카메라 위치 초기화 함수 
        /// </summary>
        public void CameraPosReset()
        {
            this.transform.eulerAngles = new Vector3(30, 0, 0); // 카메라 회전 값 리셋
            this.transform.position = Utils.cameraOffset; 
        }

        /// <summary>
        /// 미니게임 시작 또는 종료시 실행되는 함수 
        /// [미니게임 시작] : 미니게임에 등록되어있는 위치 값으로 Camera 이동한다.
        /// [미니게임 종료] : 카메라 고정값으로 리셋
        /// </summary>
        /// <param name="isStart">미니게임 시작 여부</param>
        /// <param name="miniGame"> 미니게임 정보 </param>
        private void SetCameraMiniGameVersion(bool isStart, MiniGame miniGame = null) 
        {
            if (isStart)
            {
                this.transform.position = miniGame.point.transform.position;
                this.transform.rotation = miniGame.point.transform.rotation;
                Debug.Log(">>CameraController :: SetCameraMiniGameVersion / isStart " + isStart + "/miniGame " + miniGame.name);
            }
            else
            {
                CameraPosReset(); //카메라 플레이어 쪽으로 리셋 
                Debug.Log(">>CameraController :: SetCameraMiniGameVersion / isStart " + isStart + "/miniGame  FINISH ");
            }
        }

      

    }
}

