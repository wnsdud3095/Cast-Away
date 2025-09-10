using UnityEngine;
using System;


namespace UserService
{
    public interface IUserService 
    {
        Vector3 Position { get; set; }
        StatusData Status { get; set; }

        event Action<int, int> OnUpdatedLevel;

        void InitLevel();
        void UpdateLevel(int exp);
    }
}