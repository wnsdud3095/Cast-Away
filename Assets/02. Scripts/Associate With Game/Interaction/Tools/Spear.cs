using UnityEngine;

public class Spear : BaseTool
{
    [Header("공격할 때 때의 데미지")]
    [SerializeField] private float m_target_damage;

    protected override void OnLeftUse()
    {
        m_player_ctrl.ChangeState(PlayerState.ATTACK);
    }

    protected override void OnRightUse() { }

    protected override void OnInteract(BaseBreakable target, Vector3 point)
    {
        target.UpdateHP(-m_default_damage, point);
    }    

    protected override void OnInteract(AnimalCtrl animal)
    {
        animal.Status.UpdateHP(-m_target_damage);
    }

    public override void TriggerEnter()
    {
        m_is_attacking = true;
    }

    public override void TriggerExit()
    {
        m_is_attacking = false;
        m_player_ctrl.ChangeState(PlayerState.IDLE);
    }
}
