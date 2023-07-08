using System;
using UnityEngine;

public class Player : Controllable {

    public static Player Instance;
    public bool IsUnderControl => _isUnderControl;
    
    private void Awake() {
        Instance = this;
        StartControl();
    }

    protected override void MainAbility() {
        Debug.Log("Main player ability casted!");
    }

    public override void StartControl() {
        base.StartControl();
        gameObject.SetActive(true);
    }

    public override void EndControl() {
        base.EndControl();
        gameObject.SetActive(false);
    }
}