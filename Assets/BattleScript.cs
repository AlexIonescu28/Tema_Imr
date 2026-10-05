using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class BattleScript : MonoBehaviour
{
    public GameObject character1;
    public GameObject character2;
    private Animator character1_animator;
    private Animator character2_animator;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        character1_animator=character1.GetComponent<Animator>();
        character2_animator = character2.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(character1.transform.position,character2.transform.position);
        

        if (distance<0.25f)
        {
            UnityEngine.Debug.Log("ATAC: " + distance);
            character1_animator.SetBool("isClose", true);
            character2_animator.SetBool("isClose", true);

        }
        else
        {
            character1_animator.SetBool("isClose", false);
            character2_animator.SetBool("isClose", false);
        }

    }
}
