using UnityEngine;

public class Bed : MonoBehaviour, IInteractable
{
    #region Methods
    public void Interact()
    {
        GameManager.instance.NextDay();
    }

    #endregion
}
