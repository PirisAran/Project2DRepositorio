using UnityEngine;

public class SingleSoundPlayer : SoundPlayer
{
    [SerializeField] GameObject _sound;
    public override void PlaySound()
    {
        var sound = _sound;
        SoundManager.InstantiateSound(sound);
    }
}