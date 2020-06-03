using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class OneButtonMoveTest : MonoBehaviour
{   
    [SerializeField]
    KeyCode key = KeyCode.Space;
    [SerializeField]
    float PeriodXSeg = 1f;
    [SerializeField]
    float speed = 10f;
    [SerializeField]
    Transform flecha,punto;
    Transform f, p;
    [SerializeField]
    float disMax;

    Vector3 objective;
    enum STATE { INI, DIR, DIS };

    STATE state = STATE.INI;
    bool moving = false;
    bool selectDir = false;
    bool selectDis = false;
    Vector3 dir;
    CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        if(!moving)
        {
            if(state == STATE.INI)
            {
                f = Instantiate(flecha) as Transform;
                state = STATE.DIR;
                f.position = transform.position;
                return;
            }
            if(state == STATE.DIR )
            {
                if(Input.GetKeyDown(key))
                {
                    dir = Quaternion.Euler(0, f.eulerAngles.y, 0) * Vector3.right;
                    state = STATE.DIS;
                    p = Instantiate(punto) as Transform;
                    p.position = transform.position;
                    return;
                }
                f.Rotate(Vector3.up, 360 * PeriodXSeg * Time.deltaTime, Space.World);
                return;
            }
            if(Input.GetKeyDown(key))
            {
                moving = true;
                objective = p.position;
                Destroy(p.gameObject);
                Destroy(f.gameObject);
                return;
            }

            if((transform.position-p.position).magnitude>disMax)
            {
                p.position = transform.position;
                return;
            }
            p.Translate(dir * speed * Time.deltaTime,Space.World);
        }

    }

    void FixedUpdate()
    {
        if(moving)
        {
            Vector3 dis = objective - transform.position;
            Vector3 step = (dir * speed * Time.deltaTime);
            Debug.Log(dis.magnitude);
            if (step.magnitude > dis.magnitude)
            {
                Debug.Log("acabado");
                controller.Move(dis);
                moving = false;
                state = STATE.INI;
            }
            else
            {
                controller.Move(step);
            }
        }
        
    }
}

