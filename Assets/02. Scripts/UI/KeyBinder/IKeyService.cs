using System;
using UnityEngine;

namespace KeyService
{
    public interface IKeyService
    {  
        event Action<KeyCode, string> OnUpdatedKey;// ������ Ű�� �޶��� Ű�� �ִ� ��쿡 ȣ��.


        void Initialize();	// �ʱ�ȭ�� ���� OnUpdatedKey�� �����Ű�� �뵵
        void Reset();       // Ű�� �⺻ �������� �ʱ�ȭ

        KeyCode GetKeyCode(string key_name);			// ���ڿ��� ���ε� Ű �ڵ带 ��ȯ
        bool Check(KeyCode key, KeyCode current_key);	// �����Ϸ��� Ű�� ��ȿ�� Ű���� Ȯ��
        void Register(KeyCode key, string key_name);	// ��ȿ�� Ű�� ���� Ű�� ����
    }
}