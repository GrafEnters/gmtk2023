using UnityEngine;

public class MinableRock : MonoBehaviour {
    [SerializeField]
    private int _hpLeft;

    private int _startingHp;
    private float _startingScale;

    private void Awake() {
        _startingHp = _hpLeft;
        _startingScale = transform.localScale.x;
    }

    public void OnMined() {
        _hpLeft--;
        if (_hpLeft <= 0) {
            Destroy(gameObject);
        } else {
            transform.localScale = Vector3.one * (_startingScale * ((_hpLeft + 0f) / _startingHp));
        }
    }
}