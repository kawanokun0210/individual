using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScript : MonoBehaviour
{
    //�A�j���[�V�����R���g���[���[
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.A))
        {
            animator.SetBool("mode", true);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            animator.SetBool("mode", false);
        }

    }
}
