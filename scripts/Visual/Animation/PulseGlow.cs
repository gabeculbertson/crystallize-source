using UnityEngine;
using System.Collections;

public class PulseGlow : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        Renderer renderer = GetComponent<Renderer>();
        Material mat = renderer.material;

        float emission = Mathf.PingPong(Time.time * 2f, 1.0f) * 0.5f;
        Color baseColor = Color.yellow; 

        Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);

        mat.SetColor("_EmissionColor", finalColor);
	}
}
