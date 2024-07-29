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
/// 소리 재생을 위한 컴포넌트
/// </summary>
public sealed partial class SoundManager : ManagerClassBase<SoundManager>
{
    private static SoundInfoScriptableObject _SoundInfoScriptableObject;


    /// <summary>
    /// 생성된 효과음 객체를 저장해둘 리스트
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

        // 추가된 사운드 객체가 존재한다면
        if (_EffectSoundPool.Count > 0)
        {
            // 현재 재생중이지 않은 사운드 객체를 찾습니다.
            audioSource = _EffectSoundPool.Find(
                audioSource => !audioSource.isPlaying);
        }

        // 사용 가능한 AudioSource 객체를 찾지 못한 경우
        if (audioSource == null)
        {
            // AudioSource 컴포넌트를 갖게 될 새로운 게임 오브젝트를 생성합니다.
            GameObject audioObject = new GameObject($"Object_Audio");
            audioObject.transform.SetParent(transform);

            audioObject.transform.position = position;

            // 생성된 게임 오브젝트에 AudioSource 컴포넌트를 추가합니다/
            audioSource = audioObject.AddComponent<AudioSource>();

            audioSource.playOnAwake = false;

            // 생성된 사운드 객체를 리스트에 담습니다.
            _EffectSoundPool.Add(audioSource);
        }

        // AudioSource 초기화 
        // 재생시킬 사운드를 설정합니다.
        audioSource.clip = _SoundInfoScriptableObject.GetAudioClipFromSoundID(effectSoundID);

        // 반복 재생 비활성화
        audioSource.loop = false;

        // 소리 재생
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
        // 메모리 절약을 위하여 생성된 AudioSource 객체들을 모두 제거합니다.
        foreach(AudioSource createdAudioSource in _EffectSoundPool)
            Destroy(createdAudioSource.gameObject);

        // 리스트를 비웁니다.
        _EffectSoundPool.Clear();
    }


    
}
