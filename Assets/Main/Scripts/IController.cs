public interface IController {
    public bool isMoving { get; }
    public bool isJumping { get; }
    public bool isFalling { get; }
    public bool isDoubleJumping { get; }
    public bool isGrounded { get; }
    
    public void UpdateInput(ref MoveInputData input);
    public void Update();
    public void Enable();
    public void Disable();
}
