using System;
using UnityEngine;
public abstract class LevelCompleter : MonoBehaviour {
    public event Action OnComplete = delegate { };

    protected void Complete() => OnComplete.Invoke();
}
