using UnityEngine;

public interface IStatusView
{
    void Inject(StatusPresenter presenter);
    void UpdateLV(int level, float exp_rate);  	
    void UpdateHP(float hp_rate);				
    void UpdateThirst(float thirst_rate);		
    void UpdateHunger(float hunger_rate);
}
