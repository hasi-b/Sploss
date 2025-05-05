using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class OutlineController : MonoBehaviour
{
    [SerializeField] private Color outlineColor = Color.white;
    [SerializeField] private float outlineWidth = 0.01f;
    

    private SpriteRenderer spriteRenderer;
    private Material outlineMaterial;
    private bool outlineEnabled = false;

    private static readonly int OutlineColorProperty = Shader.PropertyToID("_OutlineColor");
    private static readonly int OutlineWidthProperty = Shader.PropertyToID("_OutlineWidth");
    private static readonly int OutlineEnabledProperty = Shader.PropertyToID("_OutlineEnabled");

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Create a material instance from the shader
        outlineMaterial = new Material(Shader.Find("Custom/SpriteOutline"));

        // Set initial properties
        outlineMaterial.SetColor(OutlineColorProperty, outlineColor);
        outlineMaterial.SetFloat(OutlineWidthProperty, outlineWidth);
        outlineMaterial.SetFloat(OutlineEnabledProperty, 0); // Start with outline disabled

        // Apply the material to the sprite renderer
        spriteRenderer.material = outlineMaterial;
    }

    


    public void SetOutline()
    {
        outlineEnabled = !outlineEnabled;
        outlineMaterial.SetFloat(OutlineEnabledProperty, outlineEnabled ? 1 : 0);
    }

    // Public method to set outline color from elsewhere in your code
    public void SetOutlineColor(Color color)
    {
        outlineColor = color;
        if (outlineMaterial != null)
        {
            outlineMaterial.SetColor(OutlineColorProperty, outlineColor);
        }
    }

    // Public method to set outline width from elsewhere in your code
    public void SetOutlineWidth(float width)
    {
        outlineWidth = Mathf.Clamp(width, 0, 0.1f);
        if (outlineMaterial != null)
        {
            outlineMaterial.SetFloat(OutlineWidthProperty, outlineWidth);
        }
    }

    // Public method to toggle outline manually
    public void ToggleOutline(bool enable)
    {
        outlineEnabled = enable;
        if (outlineMaterial != null)
        {
            outlineMaterial.SetFloat(OutlineEnabledProperty, outlineEnabled ? 1 : 0);
        }
    }
}