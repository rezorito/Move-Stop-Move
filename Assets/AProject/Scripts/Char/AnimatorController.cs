using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    public Animator animPlayer;

    public void setBoolAnimIdle(bool isIdle)
    {
        animPlayer.SetBool("IsIdle", isIdle);
    }
    public void setBoolAnimUlti(bool isUlti)
    {
        animPlayer.SetBool("IsUlti", isUlti);
    }
    public void setBoolAnimWin(bool isWin)
    {
        animPlayer.SetBool("IsWin", isWin);
    }
    public void setBoolAnimAttack(bool isAttack)
    {
        animPlayer.SetBool("IsAttack", isAttack);
    }
    public void setBoolAnimDance(bool isDance)
    {
        animPlayer.SetBool("IsDance", isDance);
    }
    public void setBoolAnimDead(bool isDead)
    {
        animPlayer.SetBool("IsDead", isDead);
    }
}
