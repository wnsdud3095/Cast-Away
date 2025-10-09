using UnityEngine;

public class BreakableRock : BaseBreakable
{
    [Space(30f)]
    [Header("돌의 높이")]
    [SerializeField] private float m_stone_height;

    private readonly int m_min_rock_count = 2;
    private readonly int m_max_rock_count = 4;

    protected override void InstantiateEffect(Vector3 point)
    {
        m_camera_shaker.Shaking(0.15f, 0.35f);

        var rock_obj = ObjectManager.Instance.GetObject(ObjectType.ROCK_PARTICLE);
        rock_obj.transform.position = point;

        var rock_vfx = rock_obj.GetComponent<ParticleSystem>();
        rock_vfx.Play();
    }

    protected override void Break(Vector3 point)
    {
        InstantiateDeathEffect(point);
        InstantiateRock();
        gameObject.SetActive(false);
    }

    private void InstantiateDeathEffect(Vector3 point)
    {
        var death_obj = ObjectManager.Instance.GetObject(ObjectType.DEATH_SMOKE);
        death_obj.transform.position = point;

        var death_vfx = death_obj.GetComponent<ParticleSystem>();
        death_vfx.Play();
    }

    private void InstantiateRock()
    {
        var random_count = Random.Range(m_min_rock_count, m_max_rock_count + 1);

        while(random_count-- > 0)
        {
            var offset = new Vector3(Random.Range(-0.2f, 0.2f), 1f, Random.Range(-0.2f, 0.2f));

            var rock_obj = ObjectManager.Instance.GetObject(ObjectType.ROCK);
            rock_obj.transform.position = transform.position + offset;

            var raw_meat_rb = rock_obj.GetComponent<Rigidbody>();
            raw_meat_rb.AddForce(Vector3.up * 1.25f, ForceMode.Impulse);
        }
    }
}
