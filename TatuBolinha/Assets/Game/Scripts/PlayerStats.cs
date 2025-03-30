using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    public static PlayerStats Instance;
    
    public int Score;
    private void Start() {
        if (Instance != null)
        {
            Debug.LogError("Existe outro PlayerStats! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
}
