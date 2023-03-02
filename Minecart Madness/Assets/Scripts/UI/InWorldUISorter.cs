using System.Collections.Generic;
using UnityEngine;

public class InWorldUISorter : MonoBehaviour
{
    private Dictionary<float, Transform> childrenAtDistance = new Dictionary<float, Transform>();
    private List<float> distances = new List<float>();
    private Transform cameraTransform;

    private void Awake()
    {
        cameraTransform = transform.parent.parent.Find("Player Head Position").Find("Camera");
    }

    private void Update()
    {
        childrenAtDistance.Clear();
        distances.Clear();

        foreach (Transform child in transform)
        {
            float distance = Vector3.Distance(cameraTransform.position, child.GetComponent<InWorldUIElement>().ObjectToFollow.position);

            while (childrenAtDistance.ContainsKey(distance))
                distance += 0.0001f;

            childrenAtDistance[distance] = child;
            distances.Add(distance);
        }

        distances.Sort();
        distances.Reverse();

        for (int i = 0; i < distances.Count; ++i)
        {
            childrenAtDistance[distances[i]].SetSiblingIndex(i);
        }
    }

}
