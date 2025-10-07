using UnityEngine;

public class Axe : BaseTool
{
    [Header("나무를 벨 때의 데미지")]
    [SerializeField] private float m_target_damage;

    protected override void OnLeftUse()
    {
        m_player_ctrl.ChangeState(PlayerState.WORK);
    }

    protected override void OnRightUse() { }

    protected override void OnInteract(BaseBreakable target, Vector3 point)
    {
        if(target is BreakableTree)
        {
            target.UpdateHP(-m_target_damage, point);
        }
        else
        {
            target.UpdateHP(-m_default_damage, point);
        }
    }

    protected override void OnInteract(AnimalCtrl animal)
    {
        animal.Status.UpdateHP(-m_target_damage);
    }

    public override void TriggerExit()
    {
        base.TriggerExit();
        m_player_ctrl.ChangeState(PlayerState.IDLE);
    }
}
