using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource fruitSoundSource;
    public AudioSource winSoundSource;
    public AudioSource loseSoundSource;

    private void Awake()
    {
        GameManager.Score += () => { fruitSoundSource.Play(); };
        GameManager.Lose += () => { loseSoundSource.Play(); };
        GameManager.Win += () => { winSoundSource.Play(); };
    }
}
