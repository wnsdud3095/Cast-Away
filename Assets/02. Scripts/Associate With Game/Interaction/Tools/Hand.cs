using UnityEngine;

public class Hand : BaseTool
{
    protected override void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Animal"))
        {
            Debug.Log(collider.name);
            var animal = collider.GetComponent<AnimalCtrl>();
            OnInteract(animal);
        }
    }

    protected override void OnLeftUse()
    {
        m_player_ctrl.ChangeState(PlayerState.WORK);
    }

    protected override void OnRightUse() { }

    protected override void OnInteract(BaseBreakable target, Vector3 point)
    {
        target.UpdateHP(-m_default_damage, point);
    }    

    protected override void OnInteract(AnimalCtrl animal)
    {
        animal.Status.UpdateHP(-m_default_damage);
    }

    public override void TriggerExit()
    {
        base.TriggerExit();
        m_player_ctrl.ChangeState(PlayerState.IDLE);
    }
}
