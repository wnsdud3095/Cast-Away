using UnityEngine;

public class AggressiveAnimalReturnArea : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Enemy"))
        {
            Return(collision.collider);
        }
    }

    private void Return(Collider collider)
    {
        var animal_ctrl = collider.GetComponent<AggressiveAnimalCtrl>();
        var animal = animal_ctrl.SO;

        var container = ObjectManager.Instance.GetPool(GetObjectType(animal.Code)).Container;
        collider.transform.position = container.transform.position;

        ObjectManager.Instance.ReturnObject(collider.gameObject, GetObjectType(animal.Code));
    }

    private ObjectType GetObjectType(AnimalCode animal_code) => (ObjectType)((int)animal_code + 201);
}
