using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    // �����̴��� ���� ����
    // ���� ���尡 0�̸� �̹��� ����

    // �¿��� ��ư���� ���� ���� �ѱ�

    // -> ������ ������ �� �ൿ           
    // 1. ���� �̹��� ���� (Off Image)
    // 2. ���� ���带 �ӽ� ����ҿ� ����
    // 3. ���� ���带 0���� ����

    // -> ������ ������ �� �ൿ
    // 1. ���� �̹��� ���� (On Image)
    // 2. �ӽ� ����ҿ� �ִ� ���� ������ ���� ���� �� ����

    // ���۽� �ϴ� �ൿ
    // 1. �� ��ũ��Ʈ�� Main Audio(BGM)�� �޷��ִ�.
    // 2. ���۽� GetComponent�� Main Audio�� �����´�.
    // 3. BGM Slider�� Effect Slider�� ���� �ش� Properties�� ������ �ʱ�ȭ��Ų��.
    // 4. MainAudio�� Volumn�� BGM Properties�� ������ �ʱ�ȭ ��Ų��.
    // ++ effect sound�� ���� ������Ʈ�鵵 ������ Effect Properties�� ������ Volumn�� �ʱ�ȭ ��Ų��.


    //BGM ���� ���� slider�� ��ư �̹���
    [SerializeField] private GameObject BGMSoundOffImage = null;
    [SerializeField] private Slider BGMslider = null;

    //Effect ���� ���� slider�� ��ư �̹���
    [SerializeField] private GameObject EffectSoundOffImage = null;
    [SerializeField] private Slider Effectslider = null;

    //BGM sound
    private AudioSource MainAudio = null;

    //ȿ�������� ��������Ʈ�� ���� ����
    public delegate void EffectSound(float volumn);
    static public EffectSound effectsound;

    void Start()
    {
        MainAudio = this.GetComponent<AudioSource>();

        //��ũ��Ʈ Ȱ��ȭ �� �����̴��� ���� �ʱ�ȭ
        BGMslider.value = BGMVolumn;
        Effectslider.value = EffectVolumn;
    }

    #region Sound PlayerPrefs Properties

    //���Ұ� �� ���带 �ӽ� ������ Properties
    public float TMPBGM
    {
        get => PlayerPrefs.GetFloat("TMPBGM");
        set => PlayerPrefs.SetFloat("TMPBGM", value);
    }
    public float TMPEffect
    {
        get => PlayerPrefs.GetFloat("TMPEffect");
        set => PlayerPrefs.SetFloat("TMPEffect", value);
    }

    //BGM���� ������ PlayerPrefs���� ���� ���
    public float BGMVolumn
    {
        get => PlayerPrefs.GetFloat("BSound");
        set => PlayerPrefs.SetFloat("BSound", value);
    }

    //Effect���� ������ PlayerPrefs���� ���� ���
    public float EffectVolumn
    {
        get => PlayerPrefs.GetFloat("ESound");
        set => PlayerPrefs.SetFloat("ESound", value);
    }
    #endregion




    #region Sound Volumn Change
    //dynamic���� slider ���� ���� �� ȣ��
    public void EffectSoundChange(float _volumn)
    {
        //PlayerPrefs�� �� ����
        EffectVolumn = _volumn;

        //delegate�� ����Ǿ� �ִ� �Լ��� volumn�� ����
        //delegate�� ����� �Լ��� ���� ��쿡�� ����
        if(effectsound != null)
        {
            effectsound(_volumn);
        }

        if (_volumn != 0)
        {
            EffectSoundOffImage.SetActive(false);
        }
        else if(_volumn == 0)
        {
            EffectSoundOffImage.SetActive(true);
        }

    }    
    public void BGMSoundChange(float _volumn)
    {
        //PlayerPrefs�� �� ����
        BGMVolumn = _volumn;

        //MainAudio�� Volumn ����
        MainAudio.volume = _volumn;

        if (_volumn != 0)
        {
            BGMSoundOffImage.SetActive(false);
        }
        else if (_volumn == 0)
        {
            BGMSoundOffImage.SetActive(true);
        }
    }
    #endregion

    #region OnOff Button

    //���� �̹��� Ŭ�� �� ���� ���� ���� �ӽ�����
    public void BGMSoundOff()
    {
        TMPBGM = BGMVolumn;
        BGMslider.value = 0;
        BGMSoundOffImage.SetActive(true);
    }

    public void BGMSoundOn()
    {
        BGMslider.value = TMPBGM;
        BGMSoundOffImage.SetActive(false);
    }

    public void EffectSoundOff()
    {
        TMPEffect = EffectVolumn;
        Effectslider.value = 0;
        EffectSoundOffImage.SetActive(true);
    }

    public void EffectSoundOn()
    {
        Effectslider.value = TMPEffect;
        EffectSoundOffImage.SetActive(false);
    }
    #endregion

}
