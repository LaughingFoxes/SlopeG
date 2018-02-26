using UnityEngine;

public class Physics2DSpawner : MonoBehaviour {
    public bool randomize = false;

    void Start()
    {
        InvokeRepeating("SpawnObject", 0f, randomize ? 1f : 0.5f);
    }

    public void SpawnObject()
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.position = transform.position;
        DestroyImmediate(go.GetComponent<BoxCollider>());
        go.AddComponent<BoxCollider2D>();
        Rigidbody2D rb = go.AddComponent<Rigidbody2D>();
        rb.AddForce(Vector2.right*2f, ForceMode2D.Impulse);

        if (randomize)
            go.transform.localScale = new Vector3(Random.Range(0.1f, 2f), Random.Range(0.1f, 2f), 1f);
    }
}
