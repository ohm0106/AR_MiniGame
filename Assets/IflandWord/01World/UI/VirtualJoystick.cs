/********************************************
 * Title    : Virtual Joystick Controller
 * Ver      : 0.01
 * Date     : 2022.02.03
 * Coder    : OHM
 *******************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ifland.main
{
    /// <summary>
    /// 가상 Joystick 버튼 제어 클래스 
    /// Drag Handler 인터페이스 상속 
    /// </summary>
    [System.Obsolete]
    public class VirtualJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField]
        private RectTransform lever; //레버 이미지
        private RectTransform rectTransform; //레버 위치

        [SerializeField, Range(10, 150)]
        private float leverRange; //레버 움직임 범위 

        private Vector2 inputDirection;
        private bool isDrag; //true : 플레이어가 Joystick을 사용하기 위해 화면을 드레그 중 일 때 

        [SerializeField]
        private PlayerController playerController; 

        public float sensiblivity = 1f; //민감도 (Default value : 1f)

        

        private void Awake()
        {
            this.rectTransform = GetComponent<RectTransform>(); 
        }

        //OnBeginDrag : 드레그를 시작 할 때
        public void OnBeginDrag(PointerEventData eventData)
        {
            ControllJoyStickLever(eventData);
            this.isDrag = true;
        }

        //OnDrag : 드레그 중일 때
        public void OnDrag(PointerEventData eventData)
        {
            ControllJoyStickLever(eventData);
        }

        //OnEndDrag : 드레그를 끝냈을 때
        public void OnEndDrag(PointerEventData eventData)
        {
            this.lever.anchoredPosition = Vector2.zero; //Lever 원위치 
            this.isDrag = false;
            this.playerController.Move(Vector2.zero); //Player move stop
        }


        /// <summary>
        /// JoyStickLever UI Controller 함수
        /// </summary>
        /// <param name="eventData">클릭 좌표</param>
        private void ControllJoyStickLever(PointerEventData eventData)
        {
            //Lever가 있어야 할 위치
            var inputPos = eventData.position - this.rectTransform.anchoredPosition; 
            var inputVector = inputPos.sqrMagnitude < this.leverRange ? inputPos : inputPos.normalized * this.leverRange; 
            this.lever.anchoredPosition = inputVector;


            this.inputDirection = inputVector / this.leverRange; //해상도의 값 정규화 
            this.inputDirection = this.inputDirection * this.sensiblivity;

            Debug.Log(">> VirtualJoystick ::ControllJoyStickLever inputDirection is " + inputDirection);
        }

        /// <summary>
        /// 조이스틱 입력값 전달 함수 
        /// </summary>
        private void InputControlVector()
        {
            playerController.PlayerMoveEvent.Invoke(this.inputDirection);
        }

        private void Update()
        {
            if (isDrag)
            {
                InputControlVector();
            }
        }


   
    }


}
