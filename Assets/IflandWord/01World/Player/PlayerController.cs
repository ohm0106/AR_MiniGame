/********************************************
 * Title    : 플레이어 컨트롤러 
 * Ver      : 0.01
 * Date     : 2022.02.03
 * Coder    : OHM
 *******************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace ifland.main
{
    /// <summary>
    /// virtual 조이스틱 상태 enum
    /// </summary>
    public enum LeverCondition : int
    {
        UP = 0,
        DOWN = 180,
        RIGHT = 90,
        LEFT = 270,
    }

    /// <summary>
    /// Player 컨트롤 제어 클래스 
    /// </summary>
    [System.Obsolete]
    public class PlayerController : MonoBehaviour, IAnimationController
    {
        public float applySpeed; // Player speed
        private Rigidbody rigidbody; 

        private bool isJoystickMove;

        private Animator anim; // Player 객체의 캐릭터 render
        public GameObject character;

        [HideInInspector]
        public UnityAction<Vector2> PlayerMoveEvent;

        public bool isControltouch = true;

        private void Start()
        {
            this.rigidbody = GetComponent<Rigidbody>();
            this.anim = GetComponentInChildren<Animator>();
            PlayerMoveEvent += Move;
          
        }

       
        void Update()
        {
            rigidbody.velocity = Vector3.zero; //플레이어의 이동속도를 일정하게 하기 위해 Rigidbody Velocity를 0으로 고정 
        }

        private void OnDestroy()
        {
            PlayerMoveEvent -= Move; 
        }

        /// <summary>
        /// 플레이어 이동 함수 
        /// </summary>
        /// <param name="inputDirection">UI JoyStick 이동 Vector</param>
        public void Move(Vector2 inputDirection)
        {

            Vector2 moveInput = inputDirection;

            //조이스틱의 움직임이 있다면 true 없다면 false
            this.isJoystickMove = moveInput.sqrMagnitude != 0;

            Debug.Log(">>PlayController :: Move " + isJoystickMove);

            //isJoystickMove 값에 따라 Player 애니메이션 제어 처리
            PlayBoolAnimation(AnimationType.RUN, "isRun", this.isJoystickMove);

          
            if (isJoystickMove)
            {
                //캐릭터 오브젝트 회전
                this.character.transform.rotation = Quaternion.LookRotation(new Vector3(moveInput.x, 0, moveInput.y).normalized);

                //Player 이동 : 캐릭터 오브젝트의 forward 기준으로 움직이기 
                this.rigidbody.MovePosition(this.transform.position + this.character.transform.forward * Time.deltaTime * this.applySpeed);

                Debug.Log(">>PlayerController :: Move / player position " + this.transform.transform + "/Player child forward/" + this.character.transform.forward);
            }

        }

        /// <summary>
        /// UI 버튼 클릭 Event 함수
        /// </summary>
        public void Jump()
        {
            PlayTriggerAnimation(AnimationType.JUMP, "doJump");
        }


        #region IAnimationController

        public void PlayTriggerAnimation( AnimationType animType, string name)
        {
           /* this.anim.SetTrigger(name);*/
        }

        public void PlayBoolAnimation( AnimationType animType, string name, bool isPlay)
        {
            /*this.anim.SetBool(name, isPlay);*/
        }

        public void PlayIntAnimation( AnimationType animType, string name, int num)
        {
            /*this.anim.SetInteger(name, num);*/
        }


        #endregion

    
    }

}

