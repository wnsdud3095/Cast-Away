using UnityEngine;

public class NameTagTrigger : MonoBehaviour
{
    private AnimalCtrl m_animal_ctrl;

    public NameTagPresenter NameTagPresenter { get; protected set; }

    private void Awake()
    {
        m_animal_ctrl = transform.parent.GetComponent<AnimalCtrl>();
        m_animal_ctrl.Status.OnUpdatedHP += UpdateUI;
    }

    private void OnDestroy()
    {
        m_animal_ctrl.Status.OnUpdatedHP -= UpdateUI;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player"))
        {
            NameTagPresenter.OpenUI(m_animal_ctrl.SO.Name, 
                                    m_animal_ctrl.Status.CurrentHP, 
                                    m_animal_ctrl.Status.MaxHP);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if(collider.CompareTag("Player"))
        {
            NameTagPresenter.CloseUI();
        }
    }

    public void Inject(NameTagPresenter name_tag_presenter)
    {
        NameTagPresenter = name_tag_presenter;
    }

    private void UpdateUI(float current_hp, float max_hp)
    {
        NameTagPresenter.UpdateUI(m_animal_ctrl.SO.Name, 
                                  current_hp, 
                                  max_hp);
    }
}
