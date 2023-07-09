using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalComics : MonoBehaviour {
    public void PlayAgain() {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Loading");
    }
}