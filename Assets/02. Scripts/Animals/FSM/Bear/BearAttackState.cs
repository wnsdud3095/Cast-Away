using UnityEngine;

public class BearAttackState : MonoBehaviour, IState<AnimalCtrl>
{
    private AnimalCtrl m_controller;

    public void ExecuteEnter(AnimalCtrl sender)
    {
        if(m_controller == null)
        {
            m_controller = sender;
        }

    }

    public void ExecuteExit()
    {
        
    }
}
