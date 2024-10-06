using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class UIManager : MonoBehaviour
{
    private VisualElement mainPanel;
    private VisualElement gridContainer;
    private VisualElement gridLinesContainer;
    public Sprite circleSprite; // Assign your circle SVG here
    public Sprite rulerSprite; // Assign your ruler SVG here
    public RenderTexture planetRenderTexture; // Assign the Render Texture here

    private VisualElement planetDisplay;
    private Texture2D planetTexture;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Assuming you have a main container element named "MainPanel"
        mainPanel = root.Q<VisualElement>("MainPanel");

        // Generate the radial gradient texture
        Texture2D gradientTexture = GenerateRadialGradient(256, 256, new Color(7/255.0F, 22/255.0F, 102/255.0F), new Color(0/255.0F, 15/255.0F, 30/255.0F));

        // Set the gradient texture as the background image
        mainPanel.style.backgroundImage = new StyleBackground(gradientTexture);

        // Get the grid container element inside the main panel
        gridContainer = mainPanel.Q<VisualElement>("GridContainer");

        // Create a container for grid lines
        gridLinesContainer = new VisualElement();
        gridLinesContainer.style.position = Position.Absolute;
        gridLinesContainer.style.left = 0;
        gridLinesContainer.style.right = 0;
        gridLinesContainer.style.top = 0;
        gridLinesContainer.style.bottom = 0;

        // Add the grid lines container as the first child of the grid container
        gridContainer.Insert(0, gridLinesContainer);

        // Check if gridContainer is assigned
        if (gridContainer != null)
        {
            Debug.Log("Grid container found.");

            // Register a callback to ensure the layout is calculated before generating the grid
            gridContainer.RegisterCallback<GeometryChangedEvent>(evt =>
            {
                Debug.Log($"Grid container size: {gridContainer.resolvedStyle.width} x {gridContainer.resolvedStyle.height}");

                // Clear existing grid lines
                ClearGridLines();

                // Generate the grid lines
                GenerateGridLines();
            });
        }
        else
        {
            Debug.LogError("Grid container not found.");
        }


        // Create the circle element
        VisualElement circleElement = new VisualElement();
        circleElement.name = "circleElement";
        circleElement.style.backgroundImage = new StyleBackground(circleSprite);
        circleElement.style.width = new Length(40, LengthUnit.Percent);
        circleElement.style.height = new Length(72, LengthUnit.Percent);
        circleElement.style.position = Position.Absolute;
        circleElement.style.left = new Length(29, LengthUnit.Percent);
        circleElement.style.top = new Length(12, LengthUnit.Percent);

        // Add the circle to the root element
        root.Add(circleElement);

        // Create the ruler element
        VisualElement rulerElement = new VisualElement();
        rulerElement.name = "rulerElement";
        rulerElement.style.backgroundImage = new StyleBackground(rulerSprite);

        // Set the width to 50% of the parent container
        float parentWidth = root.resolvedStyle.width * 0.5f;
        rulerElement.style.width = new Length(40, LengthUnit.Percent);

        // Calculate height based on the aspect ratio (for example, aspect ratio is 0.33)
        float aspectRatio = 0.33f;
        rulerElement.style.height = new Length(10, LengthUnit.Pixel);

        // Set position
        rulerElement.style.position = Position.Absolute;
        rulerElement.style.left = new Length(30, LengthUnit.Percent);
        rulerElement.style.top = new Length(90, LengthUnit.Percent);

        // Add to root element
        root.Add(rulerElement);

        // Create a VisualElement to display the Render Texture
        planetDisplay = new VisualElement();
        planetDisplay.name = "planetDisplay";
        planetDisplay.style.width = new Length(55, LengthUnit.Percent);
        planetDisplay.style.height = new Length(95, LengthUnit.Percent);
        planetDisplay.style.position = Position.Absolute;
        planetDisplay.style.left = new Length(22, LengthUnit.Percent);
        planetDisplay.style.top = new Length(2, LengthUnit.Percent);

        // Add the VisualElement to the root
        root.Add(planetDisplay);

        // Create a Texture2D to store the RenderTexture content
        planetTexture = new Texture2D(planetRenderTexture.width, planetRenderTexture.height, TextureFormat.RGBA32, false);

        // Start the coroutine to update the texture regularly
        StartCoroutine(UpdatePlanetTexture());
    }

    private IEnumerator UpdatePlanetTexture()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame(); // Wait until the end of the frame to read pixels

            // Copy the RenderTexture content to the Texture2D
            RenderTexture.active = planetRenderTexture;
            planetTexture.ReadPixels(new Rect(0, 0, planetRenderTexture.width, planetRenderTexture.height), 0, 0);
            planetTexture.Apply();
            RenderTexture.active = null;

            // Set the updated Texture2D as the background image
            planetDisplay.style.backgroundImage = new StyleBackground(planetTexture);
        }
    }

    private Texture2D GenerateRadialGradient(int width, int height, Color innerColor, Color outerColor)
    {
        Texture2D texture = new Texture2D(width, height);
        Vector2 center = new Vector2(width / 2f, height / 2f);
        float maxDistance = Vector2.Distance(Vector2.zero, center);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), center);
                float t = Mathf.Clamp01(distance / maxDistance);
                Color pixelColor = Color.Lerp(innerColor, outerColor, t);
                texture.SetPixel(x, y, pixelColor);
            }
        }

        texture.Apply();
        return texture;
    }

    private void ClearGridLines()
    {
        // Remove all children from the grid lines container
        gridLinesContainer.Clear();
    }

    private void GenerateGridLines()
    {
        int gridSquareSize = 25; // Size of each grid square in pixels
        int lineThickness = 1; // Thickness of the grid lines in pixels

        // Calculate the number of horizontal and vertical lines based on container size
        float containerWidth = gridContainer.resolvedStyle.width;
        float containerHeight = gridContainer.resolvedStyle.height;

        if (containerWidth == 0 || containerHeight == 0)
        {
            Debug.LogError("Grid container has zero width or height.");
            return;
        }

        // Retrieve padding values
        float paddingLeft = gridContainer.resolvedStyle.paddingLeft;
        float paddingRight = gridContainer.resolvedStyle.paddingRight;
        float paddingTop = gridContainer.resolvedStyle.paddingTop;
        float paddingBottom = gridContainer.resolvedStyle.paddingBottom;

        // Adjust container dimensions to account for padding
        float adjustedWidth = containerWidth - paddingLeft - paddingRight;
        float adjustedHeight = containerHeight - paddingTop - paddingBottom;

        int numHorizontalLines = Mathf.FloorToInt(adjustedHeight / gridSquareSize);
        int numVerticalLines = Mathf.FloorToInt(adjustedWidth / gridSquareSize);

        // Generate horizontal lines
        for (int i = 0; i <= numHorizontalLines; i++)
        {
            VisualElement horizontalLine = new VisualElement();
            horizontalLine.style.position = Position.Absolute;
            horizontalLine.style.left = paddingLeft;
            horizontalLine.style.right = paddingRight;
            horizontalLine.style.top = paddingTop + i * gridSquareSize;
            horizontalLine.style.height = lineThickness;
            horizontalLine.style.backgroundColor = new Color(0, 0, 0, 0.1f); // 50% transparent black

            gridLinesContainer.Add(horizontalLine);
        }

        // Generate vertical lines
        for (int i = 0; i <= numVerticalLines; i++)
        {
            VisualElement verticalLine = new VisualElement();
            verticalLine.style.position = Position.Absolute;
            verticalLine.style.top = paddingTop;
            verticalLine.style.bottom = paddingBottom;
            verticalLine.style.left = paddingLeft + i * gridSquareSize;
            verticalLine.style.width = lineThickness;
            verticalLine.style.backgroundColor = new Color(0, 0, 0, 0.1f); // 50% transparent black

            gridLinesContainer.Add(verticalLine);
        }

        Debug.Log("Grid lines generated.");
    }
}