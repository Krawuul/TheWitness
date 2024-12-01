using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notebook : MonoBehaviour
{
    #region Properties

    [SerializeField] private GameObject questPanel;
    [SerializeField] private GameObject residentPanel;
    [SerializeField] private GameObject schedulePanel;
    [SerializeField] private GameObject inventoryPanel;

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

    #endregion
}
