using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Notebook : MonoBehaviour
{
    #region Properties

    [SerializeField] private GameObject questPanel;
    [SerializeField] private GameObject residentPanel;
    [SerializeField] private GameObject schedulePanel;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private InventoryUI inventoryUI;

    [SerializeField] private TextMeshProUGUI questTxt;
    [SerializeField] private List<GameObject> residentIcon;
    [SerializeField] private List<GameObject> residentInfos;

    #endregion

    #region Methods

    public void QuestPanel()
    {
        AllPanel(false);
        questPanel.SetActive(true);
    }

    public void ResidentPanel()
    {
        AllPanel(false);
        residentPanel.SetActive(true);
    }

    public void SchedulePanel()
    {
        AllPanel(false);
        schedulePanel.SetActive(true);
    }

    public void InventoryPanel()
    {
        AllPanel(false);
        inventoryPanel.SetActive(true);
    }

    public void AllPanel(bool _state)
    {
        questPanel.SetActive(_state);
        residentPanel.SetActive(_state);
        schedulePanel.SetActive(_state);
        inventoryPanel.SetActive(_state);
    }

    public void SetQuest(string quests)
    {
        questTxt.text = quests;
    }

    public void UnlockResident(int id)
    {
        residentIcon[id].SetActive(true);
    }

    public void LookResidentInfo(int _id)
    {
        ResetAllResidentInfo();
        residentInfos[_id].SetActive(true);
    }

    public void ResetAllResidentInfo()
    {
        for (int i = 0; i < residentInfos.Count; i++)
        {
            residentInfos[i].SetActive(false);
        }
    }

    public void AddItem(Item _item)
    {
        inventoryUI.AddItem(_item);
    }

    #endregion
}
