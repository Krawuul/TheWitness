using UnityEngine;

public class Bed : MonoBehaviour, IInteractable
{
    #region Methods
    public void Interact()
    {
        GameManager.instance.NextDay();
        Manager.CanvasManager.instance.Fade();
    }

    #endregion
}
