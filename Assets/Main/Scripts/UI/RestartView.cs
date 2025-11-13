using System.Collections;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class RestartView : MonoBehaviour {
    [SerializeField] Image _restartFill;
    [SerializeField] TextMeshProUGUI _restartText;
    IInput _input;

    [SerializeField] float _fillTime = 2f;
    float _restartTime;
    bool _restarting;

    void OnEnable() {
        StartCoroutine(RestartFillProcess());
    }

    public void Initialize(IInput input) {
        _input = input;
        input.OnRestartStarted += RestartStarted;
        input.OnRestartCancelled += RestartCancelled;
        input.OnRestart += RestartInvoked;
    }
    void OnDestroy() {
        _input.OnRestartStarted -= RestartStarted;
        _input.OnRestartCancelled -= RestartCancelled;
        _input.OnRestart -= RestartInvoked;
    }
    void RestartInvoked() {
        Tween.Alpha(_restartFill, 0, 0.0f);
        Tween.Alpha(_restartText, 0, 0.0f);
        _restartTime = 0;
        _restarting = false;
    }
    void RestartStarted(float obj) {
        Tween.Alpha(_restartFill, 1, 0.3f);
        Tween.Alpha(_restartText, 1, 0.3f);
        // _restartFill.gameObject.SetActive(true);
        _restarting = true;
        // _restartTime = obj;
    }
    void RestartCancelled() {
        Tween.Alpha(_restartFill, 0, 0.3f);
        Tween.Alpha(_restartText, 0, 0.3f);
        // _restartFill.gameObject.SetActive(false);
        _restarting = false;
    }

    IEnumerator RestartFillProcess() {
        while (true) {
            _restartFill.fillAmount = _restartTime / _fillTime;

            _restartTime = Mathf.Clamp(_restartTime + (_restarting ? Time.deltaTime : -Time.deltaTime * 2f), 0, _fillTime);
            // if (!_restarting) {
            // _restartTime = Mathf.Clamp(_restartTime -= Time.deltaTime * 2f, 0, _fillTime);
            // } else _restartTime = Mathf.Clamp(_restartTime += Time.deltaTime, 0, _fillTime);

            yield return null;
        }
    }
}
