using UnityEngine;

public class Bed : MonoBehaviour, IInteractable
{
    #region Methods
    public void Interact()
    {
        if (Manager.CanvasManager.instance.IsFade) return;
        GameManager.instance.NextDay();
       
    }

    #endregion
}
