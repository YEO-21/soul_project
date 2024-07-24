using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Ҹ� ����� ���� ������Ʈ
/// </summary>
public sealed class SoundManager : ManagerClassBase<SoundManager>
{
    /// <summary>
    /// ������ ȿ���� ��ü�� �����ص� ����Ʈ
    /// </summary>
    public List<AudioSource> _EffectSoundPool = new List<AudioSource>();






    public AudioSource PlayEffectSound(string effectSoundID, Vector3 position)
    {
        // �߰��� ���� ��ü�� �����Ѵٸ�
        if(_EffectSoundPool.Count > 0)
        {
            // ���� ��������� ���� ���� ��ü�� ã���ϴ�.
            AudioSource createdAudioSource =
                _EffectSoundPool.Find(audiooSource => !audiooSource.isPlaying);

            // ��� ������ ��ü�� ã�� ��� �ش� ��ü�� ��ȯ�մϴ�.
            if (createdAudioSource != null)
                return createdAudioSource;
        }

        // AudioSource ������Ʈ�� ���� �� ���ο� ���� ������Ʈ�� �����մϴ�.
        GameObject audioObject = new GameObject($"Object_Audio");

        audioObject.transform.position = position;

        // ������ ���� ������Ʈ�� AudioSource ������Ʈ�� �߰��մϴ�.
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();

        // ������ ���� ��ü�� ����Ʈ�� ����ϴ�.
        _EffectSoundPool.Add(audioSource);

        return audioSource;
    }



}
