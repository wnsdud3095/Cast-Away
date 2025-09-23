using EXPService;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

namespace KeyService
{
    public class KeyDataService : ISaveable, IKeyService
    {
        private Dictionary<string, KeyCode> m_key_dict;

        public event Action<KeyCode, string> OnUpdatedKey;

        public KeyDataService()
        {
            m_key_dict = new();

            CreateDirectory();		// 디렉터리 경로가 없는 경우 새롭게 생성한다.
            Reset();				// 기본 설정 키로 초기화한다.
        }

        private void CreateDirectory()
        {
            var local_directory_path = Path.Combine(Application.persistentDataPath, "Key");

            if (!Directory.Exists(local_directory_path))
            {
                Directory.CreateDirectory(local_directory_path);

#if UNITY_EDITOR
                Debug.Log($"<color=cyan>Key 디렉터리를 새롭게 생성합니다.</color>");
#endif
            }
        }
        // 모든 바인딩된 키를 업데이트한다.
        public void Initialize()
        {

            foreach (var pair in m_key_dict)
            {
                OnUpdatedKey?.Invoke(pair.Value, pair.Key);

            }

        }

        // 기본 설정 키로 초기화한다.
        public void Reset()
        {
            m_key_dict.Clear();

            Register(KeyCode.I, "Inventory");
            Register(KeyCode.U, "Crafting");
            Register(KeyCode.P, "Binder");
            Register(KeyCode.H, "Shortcut");

            Register(KeyCode.Alpha1, "Shortcut0");
            Register(KeyCode.Alpha2, "Shortcut1");
            Register(KeyCode.Alpha3, "Shortcut2");
            Register(KeyCode.Alpha4, "Shortcut3");
            Register(KeyCode.Alpha5, "Shortcut4");

            Register(KeyCode.Escape, "Pause");
        }

        // 변경하려는 키가 유효한 키인지 확인한다.
        public bool Check(KeyCode key, KeyCode current_key)
        {
            // 변경하려는 키가 현재 키와 같다면 변경이 가능하다.
            if (current_key == key)
            {
                return true;
            }

            // 키보드 알파벳 자판과 숫자만 가능하다.
            if (KeyCode.A <= key && key <= KeyCode.Z ||
                KeyCode.Alpha0 <= key && key <= KeyCode.Alpha9) { }
            else
            {
                return false;
            }

            // WASD는 이동 키로 예약되어 있으므로 이 키는 바인딩이 불가능하다.
            if (key == KeyCode.W ||
                key == KeyCode.A ||
                key == KeyCode.S ||
                key == KeyCode.D)
            {
                return false;
            }

            // 이미 바인딩이 되어있는 키라면 이 키는 바인딩이 불가능하다.
            foreach (var pair in m_key_dict)
            {
                if (key == pair.Value)
                {
                    return false;
                }
            }

            return true;
        }

        // 입력한 키를 주어진 문자열과 매핑하여 바인딩한다.
        public void Register(KeyCode key, string key_name)
        {
            m_key_dict[key_name] = key;

            OnUpdatedKey?.Invoke(key, key_name);
        }

        // 문자열과 매핑된 키 코드를 반환한다.
        public KeyCode GetKeyCode(string key_name)
        {
            return m_key_dict.TryGetValue(key_name, out var code) ? code : KeyCode.None;
        }

        public bool Load()
        {
            var local_data_path = Path.Combine(Application.persistentDataPath, "Key", $"KeyData.json");

            if (File.Exists(local_data_path))
            {
                m_key_dict.Clear();

                var json_data = File.ReadAllText(local_data_path);
                var wrapped_data = JsonUtility.FromJson<DataWrapper>(json_data);

                foreach (var key_data in wrapped_data.Data)
                {
                    Register(key_data.Code, key_data.Name);
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        public void Save()
        {
            var local_data_path = Path.Combine(Application.persistentDataPath, "Key", $"KeyData.json");

            var temp_list = new List<KeyData>();
            foreach (var pair in m_key_dict)
            {
                temp_list.Add(new(pair.Key, pair.Value));
            }

            var wrapped_data = new DataWrapper(temp_list.ToArray());
            var json_data = JsonUtility.ToJson(wrapped_data, true);

            File.WriteAllText(local_data_path, json_data);
        }
    }
}