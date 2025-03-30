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

    void Update()
    {
        
    }
    public void WalkAnim(float Magnetude) {
        anim.SetFloat("Walk", Magnetude);
    }
}
