
using Manager;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] string npcName;
    [SerializeField] GameObject door;
    [SerializeField] GameObject visual;
    bool canEnter =false;
    [SerializeField] int gameCheckPointNeeded;
    int[,] schedule;
    // Start is 
    private void Start()
    {
        schedule = ScheduleManager.instance.GetSchedule(npcName);
    }
    public void OnDoorInteract()
    {
        if(schedule[(int)GameManager.instance.GetTime().day, (int)GameManager.instance.GetTime().timestep] == 1)
        {
            //OpenDoor
            
            if(gameCheckPointNeeded == GameManager.instance.GameCheckPoint)
            {
                //Dialogue
                
            }
            else
            {
                //Can't talk Dialogue
            }

        }
        else
        {
            if(canEnter)
            {
                //OpenDoor
            }else
            {
                //Can't enter Dialogue
            }
        }
    }
}
