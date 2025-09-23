using UnityEngine;
using System;
using System.IO;

namespace UserService
{
    public class UserDataService : ISaveable, IUserService
    {
        private Vector3 m_position;
        private StatusData m_status;

        public event Action<int, int> OnUpdatedLevel;

        public Vector3 Position 
        { 
            get => m_position;
            set => m_position = value;
        }
        public StatusData Status
        {
            get => m_status;
            set => m_status = value;
        }

        public UserDataService() //최초실행시 데이터
        {
            var user_data = new UserData();

            m_position = user_data.Position;
            m_status = user_data.Status;
            CreateDirectory();
        }
        // 디렉터리 경로가 없는 경우에는 디렉터리 경로를 생성한다.
        private void CreateDirectory()
        {
            var local_directory = Path.Combine(Application.persistentDataPath, "User");

            if (!Directory.Exists(local_directory))
            {
                Directory.CreateDirectory(local_directory);
            }
        }

        //최초 실행시 경험치 획득이 없어도 현재 레벨/경험치를 UI나 시스템에 알리기 위해
        public void InitLevel()
        {
            OnUpdatedLevel?.Invoke(m_status.Level, m_status.EXP);
        }

        public bool Load()
        {
            var local_data_path = Path.Combine(Application.persistentDataPath, "User", $"UserData.json");

            if (File.Exists(local_data_path))
            {
                var json_data = File.ReadAllText(local_data_path);
                var user_data = JsonUtility.FromJson<UserData>(json_data);

                m_position = user_data.Position;
                m_status = user_data.Status;

                return true;
            }
            return false;
        }

        public void Save()
        {
            var local_data_path = Path.Combine(Application.persistentDataPath, "User", $"UserData.json");

            var user_data = new UserData(m_position,m_status);
            var json_data = JsonUtility.ToJson(user_data, true);

            File.WriteAllText(local_data_path, json_data);
        }

        public void UpdateLevel(int exp)
        {
            m_status.EXP += exp;

            OnUpdatedLevel?.Invoke(m_status.Level, m_status.EXP);
        }
    }
}