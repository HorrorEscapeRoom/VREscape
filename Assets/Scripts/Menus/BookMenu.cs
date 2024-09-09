using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookMenu : MonoBehaviour
{
    public ObjectivesManager manager;
    public TextMeshProUGUI text;
    public XRSlider xrSlider;
    public Slider uiSlider;
    public AudioSource audioSource;

    public void UpdateObjectivesGui(List<Objective> objectives)
    {
        string objesctivesText = string.Empty;

        foreach ( Objective obj in objectives )
        {
            objesctivesText += obj.title + "\n";
            objesctivesText += "- " + obj.text;
        }

        text.text = objesctivesText;
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
        manager.OnValueChanged.AddListener(UpdateObjectivesGui);
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
