using UnityEngine;
using TMPro;

namespace ShadowRace.UI
{
    public class FloatingDamageNumber : MonoBehaviour
    {
        [Header("Settings")]
        public TextMeshPro textMesh;
        public float floatSpeed = 2f;
        public float fadeSpeed = 3f;
        public float lifetime = 1f;
        
        [Header("Colors")]
        public Color normalDamageColor = Color.white;
        public Color critDamageColor = Color.red;
        public Color healColor = Color.green;

        private Color currentColor;

        private void Start()
        {
            Destroy(gameObject, lifetime);
        }

        private void Update()
        {
            // Float upward
            transform.position += new Vector3(0, floatSpeed * Time.deltaTime, 0);

            // Fade out
            if (textMesh != null)
            {
                currentColor.a -= fadeSpeed * Time.deltaTime;
                textMesh.color = currentColor;
            }
        }

        public void Initialize(float amount, bool isCrit, bool isHeal = false)
        {
            if (textMesh == null) textMesh = GetComponent<TextMeshPro>();
            
            textMesh.text = Mathf.RoundToInt(amount).ToString();

            if (isHeal)
            {
                textMesh.text = "+" + textMesh.text;
                currentColor = healColor;
            }
            else if (isCrit)
            {
                textMesh.text += "!";
                currentColor = critDamageColor;
                transform.localScale *= 1.5f; // Make crits bigger
            }
            else
            {
                currentColor = normalDamageColor;
            }

            textMesh.color = currentColor;
        }
    }
}
