public class NameTagPresenter
{
    private readonly INameTagView m_view;

    public NameTagPresenter(INameTagView view)
    {
        m_view = view;
    }

    public void OpenUI(string item_name)
    {
        var result_string = $"<size=0.2><color=white>{item_name}</color></size>\n<size=0.1><color=#C5C5C5>아이템 획득(E)</color></size>";
        m_view.OpenUI(result_string);
    }

    public void OpenUI(string animal_name, float current_hp, float max_hp)
    {
        var result_string = $"<size=0.2><color=white>{animal_name}</color></size>\n<size=0.1><color=#C5C5C5>체력: [{current_hp} / {max_hp}]</color></size>";
        m_view.OpenUI(result_string);
    }

    public void UpdateUI(string animal_name, float current_hp, float max_hp)
    {
        var result_string = $"<size=0.2><color=white>{animal_name}</color></size>\n<size=0.1><color=#C5C5C5>체력: [{current_hp} / {max_hp}]</color></size>";
        m_view.UpdateUI(result_string);
    }

    public void CloseUI()
    {
        m_view.CloseUI();
    }
}
