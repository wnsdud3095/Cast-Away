using UnityEngine;

namespace EXPService
{
    public interface IEXPService
    {
        public int GetEXP(int cur_level); //현재 레벨 입력시 레벨업을 위해 필요한 경험치 반환
        public void Load();
    }
}