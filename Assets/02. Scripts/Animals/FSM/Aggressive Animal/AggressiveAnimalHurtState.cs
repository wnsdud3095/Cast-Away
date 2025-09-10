using UnityEngine;

public class AggressiveAnimalHurtState : AnimalHurtState
{
    public override void OnHurtAnimationEnd()
    {   
        m_controller.ChangeState(AnimalState.TRACE);
    }
}
