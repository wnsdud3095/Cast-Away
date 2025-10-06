using UnityEngine;

public class BreakableRock : BaseBreakable
{
    [Space(30f)]
    [Header("돌의 높이")]
    [SerializeField] private float m_stone_height;

    protected override void InstantiateEffect(Vector3 point)
    {
        var rock_obj = ObjectManager.Instance.GetObject(ObjectType.ROCK_PARTICLE);
        rock_obj.transform.position = point;

        var rock_vfx = rock_obj.GetComponent<ParticleSystem>();
        rock_vfx.Play();
    }

    protected override void Break(Vector3 point)
    {
        InstantiateDeathEffect(point);
        gameObject.SetActive(false);
    }

    private void InstantiateDeathEffect(Vector3 point)
    {
        var death_obj = ObjectManager.Instance.GetObject(ObjectType.DEATH_SMOKE);
        death_obj.transform.position = point;

        var death_vfx = death_obj.GetComponent<ParticleSystem>();
        death_vfx.Play();
    }
}
