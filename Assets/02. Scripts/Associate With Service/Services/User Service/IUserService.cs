using UnityEngine;
using System;


namespace UserService
{
    public interface IUserService : ISaveable
    {
        Vector3 Position { get; set; }
        StatusData Status { get; set; }

        event Action<int, int> OnUpdatedLevel;
        event Action<UserData> OnLoaded;

        void InitLevel();
        void UpdateLevel(int exp);
    }
}