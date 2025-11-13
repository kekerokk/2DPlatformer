using System;
using UnityEngine;
public interface IInput {
    public Vector2 move { get;}
    public bool isJump { get; }
    public event Action OnAttack;
    public event Action OnPause;
    public event Action OnRestart;
    public event Action<float> OnRestartStarted;
    public event Action OnRestartCancelled;
}
