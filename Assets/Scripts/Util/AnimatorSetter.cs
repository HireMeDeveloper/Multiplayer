using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorSetter : MonoBehaviour
{
    private Animator anim;

    public new string name;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void setBool(bool value)
    {
        anim.SetBool(name, value);
    }

    public void setInt(int value)
    {
        anim.SetInteger(name, value);
    }

    public void setFloat(float value)
    {
        anim.SetFloat(name, value);
    }
}
