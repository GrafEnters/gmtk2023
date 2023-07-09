using UnityEngine;
using UnityEngine.Events;

public class BigDoorhelper : MonoBehaviour {
    private int counter;

    [SerializeField]
    private int maxCounter;

    [SerializeField]
    private UnityEvent onCounterMax;

    public void AddToCounter() {
        counter++;
        if (counter >= maxCounter) {
            onCounterMax?.Invoke();
            counter = -100000000;
        }
    }
}