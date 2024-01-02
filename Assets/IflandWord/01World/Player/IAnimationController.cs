/********************************************
 * Title    : Animation Controller Interface
 * Ver      : 0.01
 * Date     : 2022.02.03
 * Coder    : OHM
 *******************************************/



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ifland.main
{
    /// <summary>
    /// 애니메이션 컨트롤 인터페이스 
    /// </summary>
    public interface IAnimationController
    {
        /// <summary>
        /// Trigger Animation 실행
        /// </summary>
        /// <param name="animType">실행될 Animation</param>
        /// <param name="name">animation 파라미터</param>
        void PlayTriggerAnimation(AnimationType animType, string name);

        /// <summary>
        /// Bool Animation 실행
        /// </summary>
        /// <param name="animType">실행될 Animator</param>
        /// <param name="name">animation 파라미터</param>
        /// <param name="isPlay">animation 플레이 여부</param>
        void PlayBoolAnimation(AnimationType animType, string name, bool isPlay);

        /// <summary>
        /// Bool Animation 실행
        /// </summary>
        /// <param name="animType">실행될 Animator</param>
        /// <param name="name">animation 파라미터</param>
        /// <param name="num">animation 플레이 인수</param>
        void PlayIntAnimation(AnimationType animType, string name, int num);
    }
}