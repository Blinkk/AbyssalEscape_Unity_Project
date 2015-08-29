using UnityEngine;
using System.Collections;

public class Shell : MonoBehaviour 
{
    // Shell fading variables
    private float lifeTime = 5;

    private Material material;
    private Color originalCol;
    private float fadePercent;
    private float deathTime;
    private bool fading;

    void Start()
    {
        material = GetComponent<Renderer>().material;
        originalCol = material.color;
        deathTime = Time.time + lifeTime;

        StartCoroutine("Fade");
    }

    IEnumerator Fade()
    {
        while (true)
        {
            yield return new WaitForSeconds(.2f);
            if (fading)
            {
                fadePercent += Time.deltaTime;
                material.color = Color.Lerp(originalCol, Color.clear, fadePercent);

                if (fadePercent >= 1)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                if (Time.time > deathTime)
                {
                    fading = true;
                }
            }
        }
    }

    void OnTriggerEnter(Collider c)
    {
        // Stop moving when shell hits ground
        if (c.tag == "Ground")
            GetComponent<Rigidbody>().Sleep();
    }
}
