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

            CreateDirectory();		// ���͸� ��ΰ� ���� ��� ���Ӱ� �����Ѵ�.
            Reset();				// �⺻ ���� Ű�� �ʱ�ȭ�Ѵ�.
        }

        private void CreateDirectory()
        {
            var local_directory_path = Path.Combine(Application.persistentDataPath, "Key");

            if (!Directory.Exists(local_directory_path))
            {
                Directory.CreateDirectory(local_directory_path);

#if UNITY_EDITOR
                Debug.Log($"<color=cyan>Key ���͸��� ���Ӱ� �����մϴ�.</color>");
#endif
            }
        }
        // ��� ���ε��� Ű�� ������Ʈ�Ѵ�.
        public void Initialize()
        {

            foreach (var pair in m_key_dict)
            {
                OnUpdatedKey?.Invoke(pair.Value, pair.Key);

            }

        }

        // �⺻ ���� Ű�� �ʱ�ȭ�Ѵ�.
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

        // �����Ϸ��� Ű�� ��ȿ�� Ű���� Ȯ���Ѵ�.
        public bool Check(KeyCode key, KeyCode current_key)
        {
            // �����Ϸ��� Ű�� ���� Ű�� ���ٸ� ������ �����ϴ�.
            if (current_key == key)
            {
                return true;
            }

            // Ű���� ���ĺ� ���ǰ� ���ڸ� �����ϴ�.
            if (KeyCode.A <= key && key <= KeyCode.Z ||
                KeyCode.Alpha0 <= key && key <= KeyCode.Alpha9) { }
            else
            {
                return false;
            }

            // WASD�� �̵� Ű�� ����Ǿ� �����Ƿ� �� Ű�� ���ε��� �Ұ����ϴ�.
            if (key == KeyCode.W ||
                key == KeyCode.A ||
                key == KeyCode.S ||
                key == KeyCode.D)
            {
                return false;
            }

            // �̹� ���ε��� �Ǿ��ִ� Ű��� �� Ű�� ���ε��� �Ұ����ϴ�.
            foreach (var pair in m_key_dict)
            {
                if (key == pair.Value)
                {
                    return false;
                }
            }

            return true;
        }

        // �Է��� Ű�� �־��� ���ڿ��� �����Ͽ� ���ε��Ѵ�.
        public void Register(KeyCode key, string key_name)
        {
            m_key_dict[key_name] = key;

            OnUpdatedKey?.Invoke(key, key_name);
        }

        // ���ڿ��� ���ε� Ű �ڵ带 ��ȯ�Ѵ�.
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