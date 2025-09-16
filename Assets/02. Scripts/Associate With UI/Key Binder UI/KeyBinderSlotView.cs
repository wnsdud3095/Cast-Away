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

    [Header("매핑 문자열")]
    [SerializeField] private string m_key_name;

    [Header("바인딩 버튼")]
    [SerializeField] private Button m_binding_button;

    [Header("버튼 텍스트")]
    [SerializeField] private TMP_Text m_button_text;

    [Header("예외 텍스트")]
    [SerializeField] private TMP_Text m_wrong_text;

    private Coroutine m_wrong_key_coroutine;

    private void Awake()
    {
        // 바인딩 버튼에 키 변경 이벤트를 등록한다.
        m_binding_button.onClick.AddListener(ModifyKey);
    }

    // Inject()를 통해서 키 서비스를 주입받는다.
    public void Inject(IKeyService key_service)
    {
        m_key_service = key_service;

        // 처음의 키 코드는 인스펙터에서 입력받은 문자열을 통해서 얻어낸다.
        m_origin_key_code = m_key_service.GetKeyCode(m_key_name);

        // 그리고 이 키 코드를 이용하여 현재 키를 사용자에게 보여준다.
        m_button_text.text = ((char)m_origin_key_code).ToString().ToUpper();
    }

    // 키를 변경할 때 사용한다.
    public void ModifyKey()
    {
        // 키가 변경 중임을 기존의 키에서 '-'으로 변경하여 나타낸다.
        m_button_text.text = "-";

        // 실제 키가 변경되는 코루틴이다.
        StartCoroutine(Co_AssignKey());
    }

    private IEnumerator Co_AssignKey()
    {
        // 버튼이 선택되어 있는 동안 발생하며
        while (true)
        {
            // 아무 키를 입력받고
            if (Input.anyKeyDown)
            {
                // 그 키에 해당하는 키 코드가 있는지 확인한다.
                foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
                {
                    // 만약 해당하는 키 코드가 있다면
                    if (Input.GetKey(code))
                    {
                        // 변경이 가능한 유효한 키인지의 여부를 확인한다.
                        if (m_key_service.Check(code, m_origin_key_code))
                        {
                            // 유효하다면 그 키를 등록하는 과정을 거친다.
                            m_key_service.Register(code, m_key_name);
                            m_origin_key_code = code;

                            m_button_text.text = ((char)m_origin_key_code).ToString().ToUpper();
                        }
                        else
                        {

                            // 유효하지 않다면 이전의 바인딩된 키로 돌아가는 과정을 거친다.
                            // 추가적으로, 사용자에게 잘못된 입력임을 나타낸다.
                            // 여기서는 코루틴을 활용하여 에러 메시지를 발생시킨다.
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

    // 잘못된 키 입력임을 에러 메시지로 발생시키는 코루틴이다.
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