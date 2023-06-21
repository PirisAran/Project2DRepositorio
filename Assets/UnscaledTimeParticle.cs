using UnityEngine;

public class UnscaledTimeParticle : MonoBehaviour
{
    // Update is called once per frame

    ParticleSystem _ps;

    private void Start()
    {
        _ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        _ps.Simulate(Time.unscaledDeltaTime, true, false);
    }
}