using UnityEngine;

namespace KeyService
{
    [System.Serializable]
    public struct KeyData
    {
        public string Name;     // 키에 매핑될 문자열
        public KeyCode Code;

        public KeyData(string name, KeyCode code)
        {
            Name = name;
            Code = code;
        }
    }
}