using UnityEngine;

public class BreakableBuilding : BaseBreakable
{
    [Header("건물의 레시피")]
    [SerializeField] private CraftReceipe m_craft_receipe;

    private IItemObjectConverter m_item_object_converter;

    public void Inject(CameraShaker camera_shaker,
                       IItemObjectConverter item_object_converter)
    {
        base.Inject(camera_shaker);
        m_item_object_converter = item_object_converter;
    }

    protected override void Break(Vector3 point)
    {
        InstantiateDeathEffect();
        InstantiateIngredients();
        Destroy(gameObject);
    }

    protected override void InstantiateEffect(Vector3 point)
    {
        m_camera_shaker.Shaking(0.15f, 0.35f);
    }

    protected override void PlaySFX()
    {

    }

    private void InstantiateDeathEffect()
    {
        var death_obj = ObjectManager.Instance.GetObject(ObjectType.DEATH_SMOKE);
        death_obj.transform.position = transform.position + Vector3.up;

        var death_vfx = death_obj.GetComponent<ParticleSystem>();
        death_vfx.Play();
    }

    private void InstantiateIngredients()
    {
        foreach(var ingredient in m_craft_receipe.Ingredients)
        {
            var object_type = m_item_object_converter.GetObjectType(ingredient.Item.Code);

            for(int count = 0; count < ingredient.Count; count++)
            {
                var offset = new Vector3(Random.Range(-0.2f, 0.2f), 1f, Random.Range(-0.2f, 0.2f));

                var ingredient_obj = ObjectManager.Instance.GetObject(object_type);
                ingredient_obj.transform.position = transform.position + offset;

                var ingredient_rb = ingredient_obj.GetComponent<Rigidbody>();
                ingredient_rb.AddForce(Vector3.up * 1.25f, ForceMode.Impulse);                
            }
        }
    }
}
