using UnityEngine;

namespace EXPService
{
    public interface IEXPService
    {
        public int GetEXP(int cur_level); //���� ���� �Է½� �������� ���� �ʿ��� ����ġ ��ȯ
        public void Load();
    }
}