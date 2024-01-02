using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ifland.main.minigame
{
    /// <summary>
    /// Turn 제어 이벤트 
    /// </summary>
    [System.Serializable] public class TurnAnimEvent : UnityEvent<bool> { }
    /// <summary>
    /// 버튼 클릭 이벤트 
    /// </summary>
    [System.Serializable] public class ClickBtnEvent : UnityEvent<GameObject> { }
    /// <summary>
    /// GameStatus = 게임 상태 Enum
    /// </summary>
    [System.Serializable] public class GameFlowEvent : UnityEvent<GameStatus> { }


   
}