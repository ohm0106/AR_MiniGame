/********************************************
 * Title    : BacketBall Mini Game Goal post Object 
 * Ver      : 0.01
 * Date     : 2022.02.03
 * Coder    : OHM
 *******************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ifland.main.minigame.BASKETBALL
{
    



    public class GoalPost : MonoBehaviour
    {
        //Target Component에서 값 설정 
        public float rangeX; //Target Object가 움직일 x좌표 범위
        public float rangeY; //Target Object가 움직일 y좌표 범위


        private bool isTrigger = false; //true : collider에 물체가 감지 될 때 , false : 감지된 물체가 collider에서 exit 될 때

        private BasketBallObjectManager mBasketBallObjectManager = null; 

        private Coroutine FeverCorutine = null; 

        private bool isFever = false; //true : Fevertime != NONETYPE, false : Fevertime == NONTYPE 

        private Vector3 offset;
        Vector3 targetPos;

        public ParticleSystem particleSys;

        /// <summary>
        /// 코루틴 type enum
        /// </summary>
        private enum CorutineType : int
        {
            MOVE_ROTATION = 0,
            MOVE_X = 1,
            MOVE_Y = 2
           
        }


        private void Awake()
        {
            //BasketBallObjectManager 값 참조 
            if (mBasketBallObjectManager == null)
                mBasketBallObjectManager = FindObjectOfType<BasketBallObjectManager>();

            mBasketBallObjectManager.SendFeverTypeEvent += SetFeverTime;
            offset = this.transform.localPosition;

        }

        /// <summary>
        /// FEVERTIME 일 시 각 type 에 맞춰 courutine 실행
        /// </summary>
        /// <param name="_feverType"> 현재 FeverTime Type </param>
        /// <param name="_stage"> 현재 Stage </param>
        private void SetFeverTime(FeverType _feverType, int _stage)
        {
            isFever = _feverType != FeverType.NONE ? true : false;
            Debug.Log(">>Target :: SetFeverTime " + _feverType + " "+ _stage);
            //현재 Fever Time 일 시 Target 오브젝트 움직임 처리 Corutine Start
            if (isFever)
            {
                float ramainder =  _stage % 3;
                if (FeverCorutine != null)
                {
                    StopCoroutine(FeverCorutine);
                    FeverCorutine = null;
                }

                CorutineType curType = (CorutineType)ramainder;

                switch (curType)
                {
                    
                    case CorutineType.MOVE_X:
                        FeverCorutine = StartCoroutine(MoveTargetXCo());
                        break;
                    case CorutineType.MOVE_Y:
                        FeverCorutine = StartCoroutine(MoveTargetYCo());
                        break;
                    case CorutineType.MOVE_ROTATION:
                        FeverCorutine = StartCoroutine(MoveTargetRotCo());
                        break;
                }

                Debug.Log(">>Target :: SetFeverTime " + curType + " / " + FeverCorutine);
            }
            //현재 Fever time 아닐 시 초기화
            else
            {
                isFever = false;

                if (FeverCorutine != null)
                {
                    StopCoroutine(FeverCorutine);
                    FeverCorutine = null;
                }

                //target 위치 초기화 
                this.transform.localPosition = offset;
                this.transform.localRotation = Quaternion.Euler(Vector3.zero);
            }
        }

        private void OnDestroy()
        {
            //참조 제거 및 이벤트 구독 취소 
            if (mBasketBallObjectManager != null)
            {
                mBasketBallObjectManager.SendFeverTypeEvent -= SetFeverTime;
                mBasketBallObjectManager = null;
            }
            //FeverCorutine이 진행 중이라면 corutine stop 후 null 처리 
            if (FeverCorutine != null)
            {
                StopCoroutine(FeverCorutine);
                FeverCorutine = null;
            }
        }

        /// <summary>
        /// Target 오브젝트 x좌표 좌우 움직임 제어 함수  
        /// </summary>
        /// <param name="_isFever"></param>
        /// <returns></returns>
        private IEnumerator MoveTargetXCo()
        {
            float num = 0.01f;
            bool isPlus = true;
            while (isFever)
            {
                if (this.transform.localPosition.x >= rangeX)
                {
                    isPlus = false;
                    //num = -0.01f;
                }
                else if (this.transform.localPosition.x <= (-1f) * rangeX)
                {
                    isPlus = true;
                    //num = 0.01f;
                }

                num = isPlus ? 0.01f : -0.01f;
                targetPos = new Vector3(num, 0, 0);
                // target 선형 보간으로 targetPos만큼 이동 처리 
                this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, this.transform.localPosition + targetPos, 1.0f);
                yield return new WaitForSeconds(0.1f);
            }
          
        }

        /// <summary>
        /// Target 오브젝트 y좌표 상하 움직임 제어 함수 
        /// </summary>
        /// <param name="_isFever"></param>
        /// <returns></returns>
        private IEnumerator MoveTargetYCo()
        {
            float num = 0.01f;
            bool isPlus = true;
            while (isFever)
            {
                if (this.transform.localPosition.y >= rangeY)
                {
                    isPlus = false;
                }
                else if (this.transform.localPosition.y <= 0f)
                {
                    isPlus = true;
                }
                num = isPlus ? 0.01f : -0.01f;

                targetPos = new Vector3(0, num, 0);
                // target 선형 보간으로 targetPos만큼 이동 처리 
                this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, this.transform.localPosition + targetPos, 1.0f);
                yield return new WaitForSeconds(0.1f);
            }
        }
        /// <summary>
        /// Target 오브젝트 Rotation 움직임 함수 
        /// </summary>
        /// <returns></returns>
        private IEnumerator MoveTargetRotCo() 
        {
            while (isFever)
            {
                this.transform.Rotate(Vector3.forward * Time.deltaTime * 500f);
                yield return new WaitForSeconds(0.1f);
            }

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Ball" && !isTrigger)
            {
                isTrigger = true;
                Ball ballsc = other.gameObject.GetComponent<Ball>();

                int plus = (int)ballsc.CurGrade + 1;
                mBasketBallObjectManager.GetScoreEvent(true, 50 * plus );
                particleSys.Play(); //Particle System Play 
                Debug.Log(">> Target : OnTriggerEnter / Score : " + Utils.Score);
            }
        }


        private void OnTriggerExit(Collider other)
        {
            if (isTrigger)
                isTrigger = false;
        }
    }
}
