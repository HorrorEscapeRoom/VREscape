using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookMenu : MonoBehaviour
{
    public XRSlider xrSlider;
    public Slider uiSlider;
    public AudioSource audioSource;

    public void UpdateObjectivesGui(List<Objective> objectives)
    {

    }

    public void UpdateSlider(float value)
    {
        uiSlider.value = value;
    }


    public void UpdateVolume()
    {
        // to add later
        print(uiSlider.value);
        // audioSource.volume = uiSlider.value;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QuitGame()
    {
        print("ROCKY, HAS LEFT THE GAME! IF ANYONE ASKS ROCKY BROKE THIS");
        Application.Quit();
    }
}
