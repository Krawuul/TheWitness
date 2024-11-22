using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollectable
{
    abstract public void Pickup(PlayerControl _player);

    abstract public void Show();

    abstract public void Hide();

    abstract public void Store();
}
