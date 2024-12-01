using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PocketWatch : Singleton<PocketWatch>
{

    [SerializeField] Vector3 startPos;
    [SerializeField] Vector3 endPos;
    [SerializeField] GameObject hours;
    bool show;
    float timer = 0f;
    [SerializeField] float time = 2f;
    [SerializeField] GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        endPos = Camera.main.WorldToScreenPoint(transform.position);
        startPos = endPos + Vector3.down * 1000;
        player = GameObject.FindObjectOfType<PlayerControl>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(show)
        {
            timer += Time.deltaTime;
            timer = Mathf.Clamp(timer,0,time);
            transform.position = Vector3.Lerp(Camera.main.ScreenToWorldPoint(startPos), Camera.main.ScreenToWorldPoint(endPos), timer / time);
        }else
        {
            timer += Time.deltaTime;
            timer = Mathf.Clamp(timer, 0, time);
            transform.position = Vector3.Lerp(Camera.main.ScreenToWorldPoint(endPos), Camera.main.ScreenToWorldPoint(startPos), timer / time);
            if(timer >= time)
            {
                gameObject.SetActive(false);
            }
        }
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position,Camera.main.transform.up);
    }

    public void ShowWatch()
    {
        show = !show;
        if(show)
        {
            gameObject.SetActive (true);
        }
        timer = time -timer;
       switch( GameManager.instance.GetTime().timestep)
        {
            case ScheduleManager.TIMESTEP.MORNING:
                hours.transform.localRotation = Quaternion.Euler(0, 0, 120);
                break;
            case ScheduleManager.TIMESTEP.NOON:
                hours.transform.localRotation = Quaternion.Euler(0, 0, 270);
                break;
            case ScheduleManager.TIMESTEP.AFTER_NOON:
                hours.transform.localRotation = Quaternion.Euler(0, 0, 0);
                break;
            case ScheduleManager.TIMESTEP.EVENING:
                hours.transform.localRotation = Quaternion.Euler(0, 0, 210);
                break;
        }
    }
}
