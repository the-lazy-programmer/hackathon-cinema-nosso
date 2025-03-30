using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenarioMecanica : MonoBehaviour
{
    private SpriteRenderer BKSprite;
    public Transform PlayerTransform;
    private void Start() {
        BKSprite = GetComponent<SpriteRenderer>();
    }
    private void FixedUpdate() {
        //folowPlayer(PlayerTransform);
    }
    private void folowPlayer(Transform PlayerTr) {
        this.transform.position += (this.transform.position + PlayerTr.position).normalized * Time.deltaTime;
    }
}
