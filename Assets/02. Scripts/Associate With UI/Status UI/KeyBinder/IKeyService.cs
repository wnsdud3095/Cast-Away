using System;
using UnityEngine;

namespace KeyService
{
    public interface IKeyService
    {  
        event Action<KeyCode, string> OnUpdatedKey;// 이전의 키와 달라진 키가 있는 경우에 호출.


        void Initialize();	// 초기화를 위해 OnUpdatedKey를 실행시키는 용도
        void Reset();       // 키를 기본 설정으로 초기화

        KeyCode GetKeyCode(string key_name);			// 문자열에 매핑된 키 코드를 반환
        bool Check(KeyCode key, KeyCode current_key);	// 변경하려는 키가 유효한 키인지 확인
        void Register(KeyCode key, string key_name);	// 유효한 키를 토대로 키를 변경
    }
}