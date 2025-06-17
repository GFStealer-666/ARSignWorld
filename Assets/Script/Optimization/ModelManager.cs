using UnityEngine;
using System.Collections.Generic;
public class ModelManager : MonoBehaviour
{
    // Optimization code for prefomance when running in phone 
    [SerializeField] private List<GameObject> modelList;
    [SerializeField] private bool IsHiding = false;
    void Start()
    {
        DisableModel();
    }
    public void DisableModel()
    {
        if(!IsHiding) return;
        foreach (GameObject model in modelList)
        {
            model.SetActive(false);
        }
    }
    public void EnableModel()
    {
        foreach (GameObject model in modelList)
        {
            model.SetActive(true); 
        }
    }
}
