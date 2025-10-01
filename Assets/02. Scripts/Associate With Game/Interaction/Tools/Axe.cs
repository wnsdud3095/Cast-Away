using UnityEngine;

public class Axe : BaseTool
{
    [Header("나무를 벨 때의 데미지")]
    [SerializeField] private float m_target_damage;

    protected override void OnLeftUse()
    {
        m_player_ctrl.Animator.SetBool("Working", true);
    }

    protected override void OnRightUse() { }

    protected override void OnInteract(IInteratable target)
    {
        if(target is BreakableTree)
        {
            target.UpdateHP(-m_target_damage);
        }
        else
        {
            target.UpdateHP(-m_default_damage);
        }
    }

    protected override void OnInteract(AnimalCtrl animal)
    {
        animal.Status.UpdateHP(-m_target_damage);
    }
}
