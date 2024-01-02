/********************************************
 * Title    : Animation interface 
 * Ver      : 0.01
 * Date     : 2022.02.03
 * Coder    : OHM
 *******************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimationHandler 
{
    /// <summary>
    /// 애니메이션 실행 
    /// </summary>
    /// <param name="isBoom"></param>
    void StartAnimation(bool isBoom);

    /// <summary>
    /// 애니메이션 종료 
    /// </summary>
    void FinishAnimation();
}
