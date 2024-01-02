/********************************************
 * Title    : Screen Click or Touch Manager
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
    /// 실행되는 platform에 따른 화면 터치 및 클릭 제어
    /// # 1. Virtual 3D Button click 모션 및 클릭 정보 전달  
    /// </summary>
    public class ClickManager : MonoBehaviour
    {
        Animator buttonAnimator = null;
       
        private bool isClickDown;
        private GameObject obj;
        public UnityAction<bool> RayCastingClickEvent; //Casting Event
        public UnityAction<bool> StartControlEvent;


        //클릭[터치] 좌표 방향으로 오브젝트 collider 체크를 위한 ray 생성 
        private Ray ray;
        private RaycastHit hit;

        private void Awake()
        {
            Utils.isExecuteMinigame = false; 
        }

        private void Update()
        {

#if UNITY_EDITOR
            Click();
#elif UNITY_ANDROID
            Touch();
#endif

        }

        /// <summary>
        /// 마우스 입력 처리 함수
        /// </summary>
        private void Click()
        {
            //마우스 입력 좌표로 직선 물리 좌표 지정  
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Mouse Click Down 이거나 미니게임이 실행 중일 때 한번 실행
            if (Input.GetMouseButtonDown(0) && Utils.isExecuteMinigame)
            {
                //씬에 ray을 투영해 casting 성공 시 
                if (Physics.Raycast(ray, out hit, 1000f))
                {
                    /*   Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.red);*/
                    //casting된 Object 정보(hit)를 obj 변수에 참조 
                    obj = hit.collider.gameObject;

                    //casting된 object가 button이라면 애니메이션 실행 
                    if (obj.tag == "Button" && !isClickDown)
                    {
                        isClickDown = true;
                        RayCastingClickEvent.Invoke(isClickDown);
                        buttonAnimator = obj.GetComponent<Animator>();
                        buttonAnimator.SetBool("isPushUp", false);
                        buttonAnimator.SetTrigger("doPush");
                        Debug.Log(">>ClickManager :: RayCast is Success! Casting Obj Tag : " + obj.tag);
                    }

                }
                else
                {
                    Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.red);
                    if (obj != null)
                        obj = null;
                }

            }
            if (Input.GetMouseButtonUp(0))
            {
                if (isClickDown)
                {

                    isClickDown = false;
                    RayCastingClickEvent.Invoke(isClickDown);
                    buttonAnimator.SetBool("isPushUp", true);
                    Debug.Log(">>ClickManager :: MouseButtonUp ");
                }
            }
        }
        /// <summary>
        /// 화면 터치 처리 함수 
        /// </summary>
        private void Touch()
        {
            foreach(Touch touch in Input.touches)
            {
                //화면 터치 시작 
                if (touch.phase == TouchPhase.Began && Utils.isExecuteMinigame)
                {
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);

                    if (Physics.Raycast(ray, out hit, 1000))
                    {
                        //casting된 Object 정보(hit)를 obj 변수에 참조 
                        obj = hit.collider.gameObject;

                        //casting된 object 가 button이라면 애니메이션 실행  
                        if (obj.tag == "Button" && !isClickDown)
                        {
                            isClickDown = true;
                            RayCastingClickEvent.Invoke(isClickDown);
                            buttonAnimator = obj.GetComponent<Animator>();
                            buttonAnimator.SetBool("isPushUp", false);
                            buttonAnimator.SetTrigger("doPush");
                            Debug.Log(">>ClickManager :: RayCast is Success! Casting Obj Tag : " + obj.tag);
                        }

                    }
                    else
                    {
                        Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.red);
                        if (obj != null)
                            obj = null;
                    }
                }
                //화면 터치 종료  
                else if ((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && Utils.isExecuteMinigame)
                {
                    if (isClickDown)
                    {
                        isClickDown = false;
                        RayCastingClickEvent.Invoke(isClickDown);
                        buttonAnimator.SetBool("isPushUp", true);
                        Debug.Log(">>ClickManager :: MouseButtonUp ");
                    }
                }

                
            }
        }
    }
}

