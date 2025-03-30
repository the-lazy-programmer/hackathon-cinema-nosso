using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimacaoController : MonoBehaviour
{
    private Animator anim;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void WalkAnim(float Magnetude) {
        if (Magnetude!= 0)
        {
            anim.SetBool("BoolWalk" , true);
          
        } else anim.SetBool("BoolWalk" , false);
        
    }
}
