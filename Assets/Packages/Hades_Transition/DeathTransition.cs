using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTransition : MonoBehaviour {

    [SerializeField] private Material material;

    private float maskAmount = 0f;
    private float targetValue = 1f;
    [SerializeField]
    float MaskAmountMultiplier=6f;
    private void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            targetValue = -.1f;
        }
        if (Input.GetKeyDown(KeyCode.Y)) {
            targetValue = 1f;
        }

        float maskAmountChange = targetValue > maskAmount ? +.1f : -.1f;
        maskAmount += maskAmountChange * Time.deltaTime * MaskAmountMultiplier;
        maskAmount = Mathf.Clamp01(maskAmount);

        material.SetFloat("_MaskAmount", maskAmount);
    }


}