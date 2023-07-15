using UnityEngine;

public class CircleArrangement : MonoBehaviour
{
    public GameObject blockPrefab;
    public int numberOfBlocks = 13;
    public float radius = 5f;

    void Start()
    {
        float angleStep = 360f / numberOfBlocks;

        for (int i = 0; i < numberOfBlocks; i++)
        {
            float angle = i * angleStep;
            float xPos = radius * Mathf.Cos(Mathf.Deg2Rad * angle);
            float zPos = radius * Mathf.Sin(Mathf.Deg2Rad * angle);

            GameObject block = Instantiate(blockPrefab, transform);
            block.transform.localPosition = new Vector3(xPos, 0f, zPos);
            block.transform.localRotation = Quaternion.Euler(0f, angle, 0f);
        }
    }
}
