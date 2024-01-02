using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScreenController
{

    /// <summary>
    /// 점수 UI 및 점수 제어 함수
    /// </summary>
    /// <param name="num"></param>
    /// <param name="isCollect">true : Score + num / false: Score - num </param>
    void CalculateScore(int num, bool isCollect);

    /// <summary>
    /// UI 리셋 함수 
    /// </summary>
    void Reset();


    /// <summary>
    /// GameObject Active Contol 함수 
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="isActive"></param>
    void ControlActiveObject(GameObject obj, bool isActive);

    /// <summary>
    /// 스코어 팝업 패널 Active control 함수 
    /// </summary>
    /// <param name="isActive"></param>
    void ControlActivePanel(bool isActive);
}
