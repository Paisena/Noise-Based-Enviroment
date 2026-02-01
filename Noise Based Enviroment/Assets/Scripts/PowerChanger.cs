using UnityEngine;
using UnityEngine.UI;

public class PowerChanger : MonoBehaviour
{
    public Slider powerInput;
    [SerializeField] PerlinNoise perlinNoise;
    public bool whichSide;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        powerInput.value = whichSide ? (float)perlinNoise.level4Threshold : (float)perlinNoise.level3Threshold;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updatePower()
    {
        if(whichSide)
            perlinNoise.level4Threshold = (double)powerInput.value;
        else
            perlinNoise.level3Threshold = (double)powerInput.value;
    }
}
