using UnityEngine;

public class BearHurtState : AnimalHurtState
{
    public override void OnHurtAnimationEnd()
    {   
        m_controller.ChangeState(AnimalState.TRACE);
    }
}
