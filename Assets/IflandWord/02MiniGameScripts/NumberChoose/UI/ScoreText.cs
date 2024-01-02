/********************************************
 * Title    : Mini Game Score Text Object
 * Ver      : 0.01
 * Date     : 2022.02.03
 * Coder    : OHM
 *******************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// 미니게임 점수 UI control 클래스 
/// </summary>
public class ScoreText : MonoBehaviour
{

    public TMP_Text text;
    public int damage;
    private Animator anim;
    void Start()
    { 
        text = GetComponent<TMP_Text>();
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// 점수 추가시 실행되는 함수 
    /// </summary>
    /// <param name="sign">점수 plus : "+"  minus : "-" </param>
    /// <param name="damage"></param>
    public void SetScoreText(string sign, int damage)
    {
        text.text = sign + "" + damage;
        anim.SetTrigger("doDamage");
        Debug.Log(">>ScoreText : SetScoreText sign : " + sign + " damage : " + damage);
    }


}
