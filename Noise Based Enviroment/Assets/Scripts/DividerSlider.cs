using UnityEngine;
using UnityEngine.UI;

public class DividerSlider : MonoBehaviour
{
    public Slider dividerSlider;
    public GameObject dividerGO;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void updateDivider()
    {
        dividerGO.transform.position = new Vector3(dividerSlider.value* 4, dividerGO.transform.position.y, dividerGO.transform.position.z);
    }
}
