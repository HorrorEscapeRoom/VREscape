using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookMenu : MonoBehaviour
{
    public Slider uiSlider;
    public AudioSource audioSource;


    public void UpdateVolume()
    {
        // to add later
        print(uiSlider.value);
        audioSource.volume = uiSlider.value;
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
        Application.Quit();
    }
}
