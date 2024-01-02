/********************************************
 * Title    : Pirate Rulette Animation Controller
 * Ver      : 0.01
 * Date     : 2022.02.03
 * Coder    : OHM
 *******************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ifland.main.minigame
{
    /// <summary>
    /// 애니메이션 클립 오브젝트 
    /// </summary>
    [System.Serializable]
    public class AnimClipObject
    {
        public string name; 
        public AnimationClip animationClip;
    }

    /// <summary>
    /// 해적 룰렛 게임 애니메이션 컨트롤러 
    /// </summary>
    public class PirateRulletAnimationController : MonoBehaviour, IAnimationHandler
    {
        private Animator anim = null;
        private PirateRulletObjectManager mObjManager;

        public AnimClipObject[] animClips;

        private GameFlowEvent mGameFlowEvent;

        private Coroutine animationInvokeCO = null; //애니메이션 코루틴 관리

        private void Awake()
        {
            if (anim == null)
                anim = GetComponent<Animator>();
            if (mObjManager == null)
            {
                mObjManager = GetComponent<PirateRulletObjectManager>();
            }
            mObjManager.mTurnAnimEvent.AddListener(StartAnimation);
        }

        private void OnDestroy()
        {
            Release();
        }

        /// <summary>
        /// 애니메이션 시작 함수 
        /// </summary>
        /// <param name="isBoom"></param>
        public void StartAnimation(bool isBoom)
        {
            AnimClipObject animClipObject;
            if (isBoom)
            {
                animClipObject = animClips[0];
            }
            else
            {
                animClipObject = animClips[1];
            }

            if (animationInvokeCO != null)
                StopCoroutine(animationInvokeCO);
            animationInvokeCO = StartCoroutine(CheckInvoke(isBoom, animClipObject));
        }

        private void Release()
        {
            if (animationInvokeCO != null)
            {
                StopCoroutine(animationInvokeCO);
                animationInvokeCO = null;
            }
        }
        /// <summary>
        /// 애니메이션 종료 함수 
        /// </summary>
        public void FinishAnimation()
        {
            Debug.Log(">>PickleBarrelAnimationController :: FinishAnimation");
            mObjManager.OnEnabledControl(true);

        }

        IEnumerator CheckInvoke(bool isBoom, AnimClipObject animClipObject)
        {
            anim.SetTrigger(animClipObject.name);
            Invoke("FinishAnimation", animClipObject.animationClip.length);

            if (isBoom)
            {
                yield return new WaitUntil(() => !IsInvoking("FinishAnimation"));

                Debug.Log(">>PickleBarrelAnimationController :: FINISH");
                mObjManager.mFlowEvent.Invoke(GameStatus.FINISH);
            }

        }

    }
}


  
