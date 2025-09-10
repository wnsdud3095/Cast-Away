using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace EXPService
{
    [System.Serializable]
    public struct EXPData
    {
        public int Level;  // ����
        public int EXP;    // �ش� ������ �������� ���� �ʿ��� ����ġ
    }

    [System.Serializable]
    public class DataWrapper //Json���� �迭�� �б� ���� ����
    {
        public EXPData[] List;   
    }                            

    public class EXPDataService : IEXPService
    {
        private Dictionary<int, int> m_exp_dict = new();

        public EXPDataService()
        {
            Load();
        }

        // StreamingAssets ���� �������� EXPData.json�� �о� �Ľ��Ͽ� ��ųʸ��� ����
        public void Load()
        {
            var local_data_path = Path.Combine(Application.streamingAssetsPath, "EXPData.json");

            if (File.Exists(local_data_path))
            {
                var json_data = File.ReadAllText(local_data_path);
                var wrapped_data = JsonUtility.FromJson<DataWrapper>(json_data);

                foreach (var exp_data in wrapped_data.List)
                {
                    m_exp_dict.TryAdd(exp_data.Level, exp_data.EXP);
                }

                Debug.Log("���������� EXP �����͸� �ε�");

            }
            else
            {
                // �������� �ʴ´ٸ�, �������� ������ �Ұ����ϹǷ� ���� ����
                Debug.LogError($"{local_data_path}�� �������� �ʽ��ϴ�.");
                UnityEditor.EditorApplication.isPlaying = false;
                Application.Quit();
            }
        }

        public int GetEXP(int current_level) //�������� ���� �ʿ��� ����ġ ��ȯ
        {
            return m_exp_dict.TryGetValue(current_level + 1, out var exp) ? exp : 0;
        }
    }
}