using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ScheduleManager : Singleton<ScheduleManager> 
{
    public enum DAYS
    {
        MONDAY,
        TUESDAY,
        WEDNESDAY,
        THURSDAY,
        FRIDAY,
        SATURDAY,
        SUNDAY,
        COUNT,
    };

    public enum TIMESTEP
    {
        MORNING,
        NOON,
        AFTER_NOON,
        EVENING,
        COUNT
    };

    [SerializeField] string filepath;
    List<string>[,] list = new List<string>[7, 4];
    // Start is called before the first frame update
    void Awake()
    {
        if (filepath.NullIfEmpty() == null)
        {
            filepath = "Assets\\Resources\\NPCsSchedule.csv";
        }
        string[] read;


        using (StreamReader sr = new StreamReader(filepath))
        {
            string dataReceived;
            int id2 = 0;
            dataReceived = sr.ReadLine();
            while ((dataReceived = sr.ReadLine()) != null)
            {
                read = dataReceived.Split(',', StringSplitOptions.RemoveEmptyEntries);
                List<string> listNpc = new List<string>();
                int id1 = 0;
                for (int i = 1; i < read.Length; i++)
                {
                    if (read[i].Last() == '"')
                    {
                        listNpc.Add(read[i].Substring(0, read[i].Length - 1));
                        string[] tArray = new string[listNpc.Count];
                        listNpc.CopyTo(tArray);
                        list[id1, id2] = tArray.ToList();

                        listNpc.Clear();
                        id1++;
                    }
                    else
                    {
                        listNpc.Add(read[i].Substring(1));
                    }
                }


                id1 = 0;
                id2++;
            }
        }

        //Debug.Log(ToString());
        //
        //int[,] t = GetSchedule("Veuve");
        //int[,] t1 = GetSchedule("Vielle");
        //int[,] t2 = GetSchedule("Boucher");
        //int[,] t3 = GetSchedule("Sportif");
        //int[,] t4 = GetSchedule("Nain");
    }

    public override string ToString()
    {
        string text = string.Empty;
        for (int i = 0; i < list.GetLength(0); i++)
        {
            for (int j = 0; j < list.GetLength(1); j++)
            {
                text += ((DAYS)i).ToString() + " " + ((TIMESTEP)j).ToString() + " :" + "\n";
                foreach (string s in list[i, j])
                {
                    text += "\t" + s;
                }
                text += "\n";
            }

        }
        return text;
    }

    public int[,] GetSchedule(string npcName)
    {
        int[,] tab = new int[7, 4];
        for (int i = 0; i < list.GetLength(0); i++)
        {
            for (int j = 0; j < list.GetLength(1); j++)
            {
                bool here = false;
                foreach (string s in list[i, j])
                {
                    if (s == npcName)
                    {
                        here = true;
                    }
                }
                tab[i, j] = here ? 1 : 0;
            }
        }
        return tab;
    }
}
