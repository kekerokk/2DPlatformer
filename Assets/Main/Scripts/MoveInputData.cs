using UnityEngine;
public readonly ref struct MoveInputData {
    public readonly Vector2 move;
    public readonly bool jumpForced;
    public MoveInputData(Vector2 move, bool jumpForced) {
        this.move = move;
        this.jumpForced = jumpForced;
    }
}
