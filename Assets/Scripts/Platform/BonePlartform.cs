using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BonePlatform : MonoBehaviour
{
    private Tilemap tilemap;
    private TilemapRenderer tilemapRenderer;
    private TilemapCollider2D tilemapCollider;

    public float fadeDuration = 3f;
    public float reappearDelay = 5f;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        tilemapRenderer = GetComponent<TilemapRenderer>();
        tilemapCollider = GetComponent<TilemapCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FadeOutAndReappear());
        }
    }

    private IEnumerator FadeOutAndReappear()
    {
        float fadeSpeed = 1f / fadeDuration;
        Color color = tilemap.color;

        for (float t = 0; t < 1f; t += Time.deltaTime * fadeSpeed)
        {
            color.a = Mathf.Lerp(1f, 0f, t);
            tilemap.color = color;
            yield return null;
        }

        tilemap.color = new Color(color.r, color.g, color.b, 0);
        tilemapRenderer.enabled = false;
        tilemapCollider.enabled = false;

        yield return new WaitForSeconds(reappearDelay);

        tilemap.color = new Color(color.r, color.g, color.b, 1f);
        tilemapRenderer.enabled = true;
        tilemapCollider.enabled = true;
    }
}