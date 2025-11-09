using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ItemService
{
    #region Serialization
    [System.Serializable]
    public struct ItemData
    {
        public ItemCode Code;
        public string Name;
        public string Description;
    }

    [System.Serializable]
    public class DataWrapper
    {
        public ItemData[] Data;
    }
    #endregion Serialization

    public class ItemDataService : IItemDataService
    {
        private Dictionary<ItemCode, string> m_name_dict;
        private Dictionary<ItemCode, string> m_desc_dict;

        public ItemDataService()
        {
            m_name_dict = new();
            m_desc_dict = new();

            if (!Load())
                Debug.LogError("[ItemDataService] ❌ ItemData.json 파일을 불러오지 못했습니다.");
        }

        public bool Load()
        {
            var local_data_path = Path.Combine(Application.persistentDataPath, "ItemData.json");

            if (File.Exists(local_data_path))
            {
                var json_data = File.ReadAllText(local_data_path);

                var wrapped_data = JsonUtility.FromJson<DataWrapper>(json_data);

                foreach (var data in wrapped_data.Data)
                {
                        m_name_dict.Add(data.Code, data.Name);
                        m_desc_dict.Add(data.Code, data.Description);
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        public string GetName(ItemCode code)
        {
            return m_name_dict.TryGetValue(code, out var name) ? name : null;
        }

        public string GetDescription(ItemCode code)
        {
            return m_desc_dict.TryGetValue(code, out var desc) ? desc : null;
        }
    }
}