using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public sealed partial class SoundManager : ManagerClassBase<SoundManager>
{
    public const string EFFECT_SWING01  = "EFFSND_Swing01";
    public const string EFFECT_SWING02  = "EFFSND_Swing02";
    public const string EFFECT_SWING03  = "EFFSND_Swing03";
    public const string EFFECT_HIT01    = "EFFSND_Hit01";
    public const string EFFECT_HIT02    = "EFFSND_Hit02";
    public const string EFFECT_BLOCK01  = "EFFSND_Block01";
    public const string EFFECT_BLOCK02  = "EFFSND_Block02";
    public const string EFFECT_BLOCK03  = "EFFSND_Block03";
}


/// <summary>
/// �Ҹ� ����� ���� ������Ʈ
/// </summary>
public sealed partial class SoundManager : ManagerClassBase<SoundManager>
{
    private static SoundInfoScriptableObject _SoundInfoScriptableObject;


    /// <summary>
    /// ������ ȿ���� ��ü�� �����ص� ����Ʈ
    /// </summary>
    private List<AudioSource> _EffectSoundPool = new List<AudioSource>();


    public override void OnManagerInitialized()
    {
        base.OnManagerInitialized();

        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += CALLBACK_OnSceneChanged;

        if (_SoundInfoScriptableObject == null)
        {
            _SoundInfoScriptableObject =
                Resources.Load<SoundInfoScriptableObject>("ScriptableObject/SoundInfo");
        }
    }

    public AudioSource PlayEffectSound(string effectSoundID, Vector3 position)
    {
        AudioSource audioSource = null;

        // �߰��� ���� ��ü�� �����Ѵٸ�
        if (_EffectSoundPool.Count > 0)
        {
            // ���� ��������� ���� ���� ��ü�� ã���ϴ�.
            audioSource = _EffectSoundPool.Find(
                audioSource => !audioSource.isPlaying);
        }

        // ��� ������ AudioSource ��ü�� ã�� ���� ���
        if (audioSource == null)
        {
            // AudioSource ������Ʈ�� ���� �� ���ο� ���� ������Ʈ�� �����մϴ�.
            GameObject audioObject = new GameObject($"Object_Audio");
            audioObject.transform.SetParent(transform);

            audioObject.transform.position = position;

            // ������ ���� ������Ʈ�� AudioSource ������Ʈ�� �߰��մϴ�/
            audioSource = audioObject.AddComponent<AudioSource>();

            audioSource.playOnAwake = false;

            // ������ ���� ��ü�� ����Ʈ�� ����ϴ�.
            _EffectSoundPool.Add(audioSource);
        }

        // AudioSource �ʱ�ȭ 
        // �����ų ���带 �����մϴ�.
        audioSource.clip = _SoundInfoScriptableObject.GetAudioClipFromSoundID(effectSoundID);

        // �ݺ� ��� ��Ȱ��ȭ
        audioSource.loop = false;

        // �Ҹ� ���
        audioSource.Play();

        return audioSource;
    }

    public void PlayHitSound(Vector3 playPosition)
    {
        PlayEffectSound(
            Random.Range(0, 2) == 0 ? EFFECT_HIT01 : EFFECT_HIT02,
            playPosition);
    }

    public void PlayGuardSound(Vector3 playPosition)
    {
        int randomGuardSound = Random.Range(0, 3);

        switch(randomGuardSound)
        {
            case 0:
                PlayEffectSound(EFFECT_BLOCK01, playPosition);
                break;

            case 1:
                PlayEffectSound(EFFECT_BLOCK02, playPosition);
                break;

            case 2:
                PlayEffectSound(EFFECT_BLOCK03, playPosition);
                break;
        }
    }

    private void CALLBACK_OnSceneChanged(
        UnityEngine.SceneManagement.Scene prevScene, 
        UnityEngine.SceneManagement.Scene nextScene)
    {
        // �޸� ������ ���Ͽ� ������ AudioSource ��ü���� ��� �����մϴ�.
        foreach(AudioSource createdAudioSource in _EffectSoundPool)
            Destroy(createdAudioSource.gameObject);

        // ����Ʈ�� ���ϴ�.
        _EffectSoundPool.Clear();
    }


    
}
