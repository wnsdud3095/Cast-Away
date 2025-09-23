using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace EXPService
{
    [System.Serializable]
    public struct EXPData
    {
        public int Level;  // 레벨
        public int EXP;    // 해당 레벨의 레벨업을 위해 필요한 경험치
    }

    [System.Serializable]
    public class DataWrapper //Json에서 배열을 읽기 위해 레핑
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

        // StreamingAssets 폴더 하위에서 EXPData.json을 읽어 파싱하여 딕셔너리에 저장
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

                Debug.Log("성공적으로 EXP 데이터를 로드");

            }
            else
            {
                // 존재하지 않는다면, 정상적인 게임이 불가능하므로 강제 종료
                Debug.LogError($"{local_data_path}가 존재하지 않습니다.");
                UnityEditor.EditorApplication.isPlaying = false;
                Application.Quit();
            }
        }

        public int GetEXP(int current_level) //레벨업을 위해 필요한 경험치 반환
        {
            return m_exp_dict.TryGetValue(current_level + 1, out var exp) ? exp : 0;
        }
    }
}