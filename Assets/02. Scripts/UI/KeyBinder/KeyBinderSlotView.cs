using System;
using System.Collections;
using KeyService;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyBinderSlotView : MonoBehaviour
{
    private KeyCode m_origin_key_code;
    private IKeyService m_key_service;

    [Header("���� ���ڿ�")]
    [SerializeField] private string m_key_name;

    [Header("���ε� ��ư")]
    [SerializeField] private Button m_binding_button;

    [Header("��ư �ؽ�Ʈ")]
    [SerializeField] private TMP_Text m_button_text;

    [Header("���� �ؽ�Ʈ")]
    [SerializeField] private TMP_Text m_wrong_text;

    private Coroutine m_wrong_key_coroutine;

    private void Awake()
    {
        // ���ε� ��ư�� Ű ���� �̺�Ʈ�� ����Ѵ�.
        m_binding_button.onClick.AddListener(ModifyKey);
    }

    // Inject()�� ���ؼ� Ű ���񽺸� ���Թ޴´�.
    public void Inject(IKeyService key_service)
    {
        m_key_service = key_service;

        // ó���� Ű �ڵ�� �ν����Ϳ��� �Է¹��� ���ڿ��� ���ؼ� ����.
        m_origin_key_code = m_key_service.GetKeyCode(m_key_name);

        // �׸��� �� Ű �ڵ带 �̿��Ͽ� ���� Ű�� ����ڿ��� �����ش�.
        m_button_text.text = ((char)m_origin_key_code).ToString().ToUpper();
    }

    // Ű�� ������ �� ����Ѵ�.
    public void ModifyKey()
    {
        // Ű�� ���� ������ ������ Ű���� '-'���� �����Ͽ� ��Ÿ����.
        m_button_text.text = "-";

        // ���� Ű�� ����Ǵ� �ڷ�ƾ�̴�.
        StartCoroutine(Co_AssignKey());
    }

    private IEnumerator Co_AssignKey()
    {
        // ��ư�� ���õǾ� �ִ� ���� �߻��ϸ�
        while (true)
        {
            // �ƹ� Ű�� �Է¹ް�
            if (Input.anyKeyDown)
            {
                // �� Ű�� �ش��ϴ� Ű �ڵ尡 �ִ��� Ȯ���Ѵ�.
                foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
                {
                    // ���� �ش��ϴ� Ű �ڵ尡 �ִٸ�
                    if (Input.GetKey(code))
                    {
                        // ������ ������ ��ȿ�� Ű������ ���θ� Ȯ���Ѵ�.
                        if (m_key_service.Check(code, m_origin_key_code))
                        {
                            // ��ȿ�ϴٸ� �� Ű�� ����ϴ� ������ ��ģ��.
                            m_key_service.Register(code, m_key_name);
                            m_origin_key_code = code;

                            m_button_text.text = ((char)m_origin_key_code).ToString().ToUpper();
                        }
                        else
                        {

                            // ��ȿ���� �ʴٸ� ������ ���ε��� Ű�� ���ư��� ������ ��ģ��.
                            // �߰�������, ����ڿ��� �߸��� �Է����� ��Ÿ����.
                            // ���⼭�� �ڷ�ƾ�� Ȱ���Ͽ� ���� �޽����� �߻���Ų��.
                            m_button_text.text = ((char)m_origin_key_code).ToString().ToUpper();

                            if (m_wrong_key_coroutine != null)
                            {
                                StopCoroutine(m_wrong_key_coroutine);
                                m_wrong_key_coroutine = null;
                            }
                            m_wrong_key_coroutine = StartCoroutine(Co_WrongKey());
                        }

                        break;
                    }
                }

                yield break;
            }

            yield return null;
        }
    }

    // �߸��� Ű �Է����� ���� �޽����� �߻���Ű�� �ڷ�ƾ�̴�.
    private IEnumerator Co_WrongKey()
    {
        float elapsed_time = 0f;
        float target_time = 1f;

        while (elapsed_time < target_time)
        {
            float delta = elapsed_time / target_time;
            SetAlpha(delta);

            elapsed_time += Time.deltaTime;
            yield return null;
        }

        SetAlpha(1f);
        elapsed_time = 0f;

        while (elapsed_time < target_time)
        {
            float delta = elapsed_time / target_time;
            SetAlpha(1 - delta);

            elapsed_time += Time.deltaTime;
            yield return null;
        }

        SetAlpha(0f);
        m_wrong_key_coroutine = null;
    }

    private void SetAlpha(float alpha)
    {
        var color = m_wrong_text.color;
        color.a = alpha;
        m_wrong_text.color = color;
    }
}