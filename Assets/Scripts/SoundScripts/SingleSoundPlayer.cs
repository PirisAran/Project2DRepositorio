using UnityEngine;

public class SingleSoundPlayer : SoundPlayer
{
    [SerializeField] GameObject _sound;
    public override GameObject PlaySound()
    {
        var sound = _sound;
        return SoundManager.InstantiateSound(sound);
    }
}