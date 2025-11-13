using System;
using UnityEngine;
public interface IPatroller {
    public Vector3 position { get; }
    public void SetTarget(Vector3 pos);

    public event Action OnDestroyEvt;
}
