using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ifland.main.minigame
{
    public interface IGameObjectManager
    {
        /// <summary>
        /// 초기화 함수 
        /// </summary>
        void OnINIT();

        /// <summary>
        /// 미니 게임 시작 setting 함수 
        /// </summary>
        void StartSetting();

        /// <summary>
        /// 미니 게임 종료 setting 함수 
        /// </summary>
        void StopSetting();

        /// <summary>
        /// 게임 종료 함수 
        /// </summary>
        void FinishGame();

        /// <summary>
        /// 게임 강제 종료 
        /// </summary>
        void FinishForceGame();

        /// <summary>
        ///  Release 함수 
        /// </summary>
        void Release();

    }
}

