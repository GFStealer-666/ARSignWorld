using UnityEngine;

public class RandomizeChildrenOrder : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private bool IsRandom = false;
    void Start()
    {
        Randomize();
    }

    public void Randomize()
    {
        if(!IsRandom) return;
        int childCount = parent.childCount;

        // Store children
        Transform[] children = new Transform[childCount];
        for (int i = 0; i < childCount; i++)
        {
            children[i] = parent.GetChild(i);
        }

        // Shuffle
        for (int i = 0; i < childCount; i++)
        {
            int randomIndex = Random.Range(i, childCount);
            // Swap sibling index
            Transform temp = children[i];
            children[i] = children[randomIndex];
            children[randomIndex] = temp;
        }

        // Re-assign sibling index to reflect new order
        for (int i = 0; i < childCount; i++)
        {
            children[i].SetSiblingIndex(i);
        }
    }
}
