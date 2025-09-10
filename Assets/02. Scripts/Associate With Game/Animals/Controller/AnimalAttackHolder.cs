using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AnimalAttackHolder : MonoBehaviour
{
    [Header("동물 컨트롤러")]
    [SerializeField] private AnimalCtrl m_animal_ctrl;

    public BoxCollider Collider { get; private set; }

    private void Awake()
    {
        Collider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log($"닿았음 {(m_animal_ctrl as AggressiveAnimalCtrl).Attack.ATK} 피해를 입힘");
        }
    }
}
