using System.Collections;
using UnityEngine;

public class BreakableTree : BaseBreakable
{
    [Space(30f)]
    [Header("나무의 높이")]
    [SerializeField] private float m_tree_height;

    private readonly int m_min_log_count = 2;
    private readonly int m_max_log_count = 4;

    protected override void InstantiateEffect(Vector3 point)
    {
        m_camera_shaker.Shaking(0.1f, 0.4f);

        var leaf_obj = ObjectManager.Instance.GetObject(ObjectType.LEAF_PARTICLE);
        leaf_obj.transform.position = transform.position + m_tree_height * Vector3.up;

        var leaf_vfx = leaf_obj.GetComponent<ParticleSystem>();
        leaf_vfx.Play();
    }

    protected override void PlaySFX()
    {
        SoundManager.Instance.PlaySFX("Tree Interaction", false, Vector3.zero);
    }

    protected override void Break(Vector3 point)
    {
        SoundManager.Instance.PlaySFX("Tree Fall", true, transform.position);
        StartCoroutine(FallAnimation(point));
    }

    private IEnumerator FallAnimation(Vector3 point)
    {
        var direction = (transform.position - point).normalized;

        var target_rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(15f, 0f, 0f);

        var elasped_time = 0f;
        var target_time = 1.5f;

        var start_rotation = transform.rotation;

        while (elasped_time < target_time)
        {
            elasped_time += Time.deltaTime;

            var delta = elasped_time / target_time;
            transform.rotation = Quaternion.Slerp(start_rotation, target_rotation, delta);
            
            yield return null;
        }

        m_camera_shaker.Shaking(0.1f, 0.75f);
        InstantiateDeathEffect();
        InstantiateLog();
        transform.parent.gameObject.SetActive(false);
    }

    private void InstantiateDeathEffect()
    {
        var death_obj = ObjectManager.Instance.GetObject(ObjectType.DEATH_SMOKE);
        death_obj.transform.position = transform.position + Vector3.up;

        var death_vfx = death_obj.GetComponent<ParticleSystem>();
        death_vfx.Play();
    }

    private void InstantiateLog()
    {
        var random_count = Random.Range(m_min_log_count, m_max_log_count + 1);

        while(random_count-- > 0)
        {
            var offset = new Vector3(Random.Range(-0.2f, 0.2f), 1f, Random.Range(-0.2f, 0.2f));

            var log_obj = ObjectManager.Instance.GetObject(ObjectType.WOOD_LOG);
            log_obj.transform.position = transform.position + offset;

            var raw_meat_rb = log_obj.GetComponent<Rigidbody>();
            raw_meat_rb.AddForce(Vector3.up * 1.25f, ForceMode.Impulse);
        }
    }
}
