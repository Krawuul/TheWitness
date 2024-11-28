using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    #region Properties

    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private GameObject container;
    [SerializeField] private TextMeshProUGUI itemTitle;
    [SerializeField] private TextMeshProUGUI itemDescription;

    #endregion

    #region Methods

    public void AddItem(Item _item)
    {
        GameObject obj = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
        obj.transform.SetParent(container.transform, false);
        obj.GetComponentInChildren<Button>().onClick.AddListener(() => DisplayItemInfo(_item));
    }

    private void DisplayItemInfo(Item _item)
    {
        itemTitle.text = _item.Info;
        itemDescription.text = _item.Infoplus;
    }

    #endregion
}
