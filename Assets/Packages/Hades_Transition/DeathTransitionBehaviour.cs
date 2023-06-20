using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTransitionBehaviour : MonoBehaviour 
{
    [SerializeField] private Material material;

    private float maskAmount = 0f;
    private static float targetValue = -0.1f;
    [SerializeField]
    static float  MaskAmountMultiplier=12f;
    private void Update() 
    {
        float maskAmountChange = targetValue > maskAmount ? +.1f : -.1f;
        maskAmount += maskAmountChange * Time.deltaTime * MaskAmountMultiplier;
        maskAmount = Mathf.Clamp01(maskAmount);

        material.SetFloat("_MaskAmount", maskAmount);
    }

    public static void DoDeathTransition(float speed)
    {
        targetValue = 1f;
        MaskAmountMultiplier = speed;
        Debug.Log("DoingDeathTransition");
    }
    public static void UndoDeathTransition(float speed)
    {
        targetValue = -0.1f;
        MaskAmountMultiplier = speed;
        Debug.Log("UndoingDeathTransition");

    }
}