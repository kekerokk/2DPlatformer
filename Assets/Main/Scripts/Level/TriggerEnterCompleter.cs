using UnityEngine;
public class TriggerEnterCompleter : LevelCompleter {
    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log($"Level complete");
        Complete();
    }
}
