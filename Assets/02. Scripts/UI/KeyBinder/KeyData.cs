using UnityEngine;

namespace KeyService
{
    [System.Serializable]
    public struct KeyData
    {
        public string Name;     // Ű�� ���ε� ���ڿ�
        public KeyCode Code;

        public KeyData(string name, KeyCode code)
        {
            Name = name;
            Code = code;
        }
    }
}