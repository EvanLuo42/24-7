using System.Collections;
using System.Linq;
using CardSystem;
using CardSystem.CardEffect;
using UnityEngine;
using UnityEngine.UI;

public class MonitorDisplay : MonoBehaviour
{
    [Header("Monitor Setup")]
    public GameObject monitorObject; // Reference to SM_Display1
    
    [Header("UI Elements")]
    public Canvas monitorCanvas;
    public Image cardImage;
    public Text cardNameText;
    public Text cardDescriptionText;
    
    [Header("Card Sprites")]
    public Sprite codeRefactoringSprite;
    public Sprite lockTfInSprite;
    
    [Header("Display Settings")]
    public float fadeInDuration = 0.3f;
    public float fadeOutDuration = 0.2f;
    
    [Header("Screen Corner Positioning")]
    [Tooltip("Adjust these to match the monitor screen corners")]
    public Vector3 topLeftCorner = new Vector3(-0.18f, -0.13f, 0.66f);
    public Vector3 topRightCorner = new Vector3(0.3f, 0.27f, 0.17f);
    public Vector3 bottomLeftCorner = new Vector3(0.06f, 0f, 1f);
    public Vector3 bottomRightCorner = new Vector3(0.18f, 0.19f, 0.14f);
    
    [Header("Screen Rotation")]
    [Tooltip("Adjust rotation on each axis to match monitor angle")]
    public Vector3 screenRotation = new Vector3(-90f, 180f, 0f);
    
    [Header("UI Element Positioning")]
    [Tooltip("Adjust positioning of card elements")]
    public Vector4 imagePosition = new Vector4(0.05f, 0.2f, 0.35f, 0.8f); // minX, minY, maxX, maxY
    public Vector4 namePosition = new Vector4(0.4f, 0.7f, 0.95f, 0.9f);
    public Vector4 descriptionPosition = new Vector4(0.4f, 0.1f, 0.95f, 0.65f);
    
    [Header("Card Lookup")]
    public CardEffectDatabase cardEffectDatabase;
    
    private CanvasGroup _canvasGroup;
    private bool _isDisplaying = false;
    private RectTransform _canvasRect;
    
    // Store previous values to detect changes
    private Vector3 _lastTopLeft, _lastTopRight, _lastBottomLeft, _lastBottomRight, _lastRotation;
    private Vector4 _lastImagePos, _lastNamePos, _lastDescPos;
    
    // Store UI element references for repositioning
    private RectTransform _imageRect, _nameRect, _descRect;
    
    private void Start()
    {
        // Create monitor canvas
        CreateMonitorCanvas();
        
        // Setup canvas group for fade effects
        _canvasGroup = monitorCanvas.GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
        {
            _canvasGroup = monitorCanvas.gameObject.AddComponent<CanvasGroup>();
        }
        
        // Always visible but start with default monitor content
        _canvasGroup.alpha = 1f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        
        // Show default monitor screen
        ShowDefaultMonitorScreen();
    }
    
    private void CreateMonitorCanvas()
    {
        // Create canvas 
        GameObject canvasObj = new GameObject("MonitorCanvas");
        canvasObj.transform.SetParent(monitorObject.transform); // Parent to monitor for proper positioning
        
        monitorCanvas = canvasObj.AddComponent<Canvas>();
        monitorCanvas.renderMode = RenderMode.WorldSpace;
        monitorCanvas.worldCamera = Camera.main;
        
        // Position and scale the canvas to match monitor screen
        _canvasRect = canvasObj.GetComponent<RectTransform>();
        PositionCanvasToMonitorScreen(_canvasRect);
        
        // Initialize last values for change detection
        UpdateLastValues();
        
        // Add graphic raycaster
        canvasObj.AddComponent<GraphicRaycaster>();
        
        // Create UI elements
        CreateUIElements(canvasObj);
    }
    
    private void PositionCanvasToMonitorScreen(RectTransform canvasRect)
    {
        // Calculate canvas position and size based on corner points
        Vector3 center = (topLeftCorner + bottomRightCorner) * 0.5f;
        Vector3 size = new Vector3(
            Vector3.Distance(topLeftCorner, topRightCorner),
            Vector3.Distance(topLeftCorner, bottomLeftCorner),
            0.01f
        );
        
        // Set local position relative to monitor
        canvasRect.localPosition = center;
        canvasRect.localRotation = Quaternion.Euler(screenRotation);
        canvasRect.sizeDelta = new Vector2(size.x * 1000f, size.y * 1000f); // Make it large enough to be visible
        canvasRect.localScale = Vector3.one * 0.001f; // Then scale it down
        
    }
    
    private void Update()
    {
        // Update if values have changed 
        if (_canvasRect != null && HasValuesChanged())
        {
            PositionCanvasToMonitorScreen(_canvasRect);
            UpdateUIElementPositions();
            UpdateLastValues();
        }
    }
    
    private bool HasValuesChanged()
    {
        return topLeftCorner != _lastTopLeft ||
               topRightCorner != _lastTopRight ||
               bottomLeftCorner != _lastBottomLeft ||
               bottomRightCorner != _lastBottomRight ||
               screenRotation != _lastRotation ||
               imagePosition != _lastImagePos ||
               namePosition != _lastNamePos ||
               descriptionPosition != _lastDescPos;
    }
    
    private void UpdateLastValues()
    {
        _lastTopLeft = topLeftCorner;
        _lastTopRight = topRightCorner;
        _lastBottomLeft = bottomLeftCorner;
        _lastBottomRight = bottomRightCorner;
        _lastRotation = screenRotation;
        _lastImagePos = imagePosition;
        _lastNamePos = namePosition;
        _lastDescPos = descriptionPosition;
    }
    
    private void CreateUIElements(GameObject canvasObj)
    {
        // Create background panel that fills the entire canvas
        GameObject panel = new GameObject("Background");
        panel.transform.SetParent(canvasObj.transform, false);
        
        Image panelImage = panel.AddComponent<Image>();
        panelImage.color = new Color(1.0f, 0.0f, 0.0f, 0.8f); // Red with transparency
        
        RectTransform panelRect = panel.GetComponent<RectTransform>();
        // Fill the entire canvas area
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;
        
        // Create card image
        cardImage = CreateImageElement("CardImage", panel.transform);
        _imageRect = cardImage.GetComponent<RectTransform>();
        
        // Create card name text
        cardNameText = CreateTextElement("CardName", panel.transform, "Card Name", 36, Color.white);
        _nameRect = cardNameText.GetComponent<RectTransform>();
        
        // Create card description text 
        cardDescriptionText = CreateTextElement("CardDescription", panel.transform, "Card Description", 24, Color.white);
        _descRect = cardDescriptionText.GetComponent<RectTransform>();
        
        // Apply initial positioning
        UpdateUIElementPositions();
    }
    
    private void UpdateUIElementPositions()
    {
        if (_imageRect != null)
        {
            _imageRect.anchorMin = new Vector2(imagePosition.x, imagePosition.y);
            _imageRect.anchorMax = new Vector2(imagePosition.z, imagePosition.w);
        }
        
        if (_nameRect != null)
        {
            _nameRect.anchorMin = new Vector2(namePosition.x, namePosition.y);
            _nameRect.anchorMax = new Vector2(namePosition.z, namePosition.w);
        }
        
        if (_descRect != null)
        {
            _descRect.anchorMin = new Vector2(descriptionPosition.x, descriptionPosition.y);
            _descRect.anchorMax = new Vector2(descriptionPosition.z, descriptionPosition.w);
        }
    }
    
    private Image CreateImageElement(string name, Transform parent)
    {
        GameObject imageObj = new GameObject(name);
        imageObj.transform.SetParent(parent, false);
        
        Image image = imageObj.AddComponent<Image>();
        image.color = Color.white;
        
        RectTransform rect = image.GetComponent<RectTransform>();
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        
        return image;
    }
    
    private Text CreateTextElement(string name, Transform parent, string defaultText, int fontSize, Color color)
    {
        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(parent, false);
        
        Text text = textObj.AddComponent<Text>();
        text.text = defaultText;
        
        // Try to get a built-in font that actually works
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (text.font == null)
        {
            text.font = Resources.FindObjectsOfTypeAll<Font>().FirstOrDefault();
        }
        
        text.fontSize = fontSize;
        text.color = color;
        text.alignment = TextAnchor.MiddleCenter;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Truncate;
        
        RectTransform rect = text.GetComponent<RectTransform>();
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        
        return text;
    }
    
    public void DisplayCardInfo(CardEffect cardEffect, int duration = 0)
    {
        if (!cardEffect) return;
        
        cardNameText.text = GetCardName(cardEffect);
        cardDescriptionText.text = GetCardDescription(cardEffect);
        
        UpdateCardImage(cardEffect);
        
        // Show card info 
        _isDisplaying = true;
        cardImage.gameObject.SetActive(true);
        cardNameText.gameObject.SetActive(true);
        cardDescriptionText.gameObject.SetActive(true);
    }
    
    public void HideCardInfo()
    {
        if (_isDisplaying)
        {
            _isDisplaying = false;
            // Hide card info
            ShowDefaultMonitorScreen();
        }
    }
    
    private void ShowDefaultMonitorScreen()
    {
        // Hide card info elements but show placeholder text
        cardImage.gameObject.SetActive(false);
        cardNameText.gameObject.SetActive(true);
        cardDescriptionText.gameObject.SetActive(true);
        
        // Show placeholder text
        cardNameText.text = "Monitor Ready";
        cardDescriptionText.text = "Hover over objects to see card info";
    }
    
    private string GetCardName(CardEffect cardEffect)
    {
        var entry = cardEffectDatabase.GetEntry(cardEffect);
        if (entry != null && !string.IsNullOrEmpty(entry.overrideName))
            return entry.overrideName;

        return !string.IsNullOrEmpty(cardEffect.cardName) ? cardEffect.cardName : "Unknown Card";
    }
    
    private string GetCardDescription(CardEffect cardEffect)
    {
        var entry = cardEffectDatabase.GetEntry(cardEffect);
        if (entry != null && !string.IsNullOrEmpty(entry.overrideDescription))
            return entry.overrideDescription;

        if (!string.IsNullOrEmpty(cardEffect.cardDescription))
            return cardEffect.cardDescription;

        return GenerateDescriptionFromEffects(cardEffect);
    }
    
    
    private string GenerateDescriptionFromEffects(CardEffect cardEffect)
    {
        // Generate description based on the effects
        string description = "This card affects your attributes:\n";
        
        foreach (var effect in cardEffect.cardEffectConfig)
        {
            if (effect is CardSystem.CardEffect.Effect.AttributeChangeEffect attrEffect)
            {
                description += GetAttributeEffectDescription(attrEffect);
            }
        }
        
        return description;
    }
    
    private string GetAttributeEffectDescription(CardSystem.CardEffect.Effect.AttributeChangeEffect attrEffect)
    {
        string desc = "";
        foreach (var config in attrEffect.attributeChangeConfigs)
        {
            string attributeName = config.attributeToOperate.ToString();
            string operation = GetOperationSymbol(config.operationToDo);
            desc += $"• {attributeName}: {operation} {config.value}\n";
        }
        return desc;
    }
    
    private string GetOperationSymbol(CardSystem.CardEffect.Effect.AttributeChangeEffect.CalculationType operation)
    {
        return operation switch
        {
            CardSystem.CardEffect.Effect.AttributeChangeEffect.CalculationType.Add => "+",
            CardSystem.CardEffect.Effect.AttributeChangeEffect.CalculationType.Minus => "-",
            CardSystem.CardEffect.Effect.AttributeChangeEffect.CalculationType.Multiply => "×",
            CardSystem.CardEffect.Effect.AttributeChangeEffect.CalculationType.Divide => "÷",
            CardSystem.CardEffect.Effect.AttributeChangeEffect.CalculationType.Set => "=",
            _ => "?"
        };
    }
    
    private void UpdateCardImage(CardEffect cardEffect)
    {
        var entry = cardEffectDatabase.GetEntry(cardEffect);
        if (entry != null && entry.cardSprite)
        {
            cardImage.sprite = entry.cardSprite;
        }
    }
    
    private Sprite GetSpriteForCardEffect(CardEffect cardEffect)
    {
        // Check if this is CodeRefactoring card
        if (IsCodeRefactoringCard(cardEffect))
        {
            return codeRefactoringSprite;
        }
        
        // Check if this is LockTfIn card
        if (IsLockTfInCard(cardEffect))
        {
            return lockTfInSprite;
        }
        
        return null;
    }
    

    
    private bool IsCodeRefactoringCard(CardEffect cardEffect)
    {
        // You can implement this based on the card effect properties
        // For now, we'll use a simple check - you might want to add a card name property
        return cardEffect.name.Contains("CodeRefactoring") || cardEffect.name.Contains("CR_CE");
    }
    
    private bool IsLockTfInCard(CardEffect cardEffect)
    {
        // You can implement this based on the card effect properties
        return cardEffect.name.Contains("LockTfIn") || cardEffect.name.Contains("LTI");
    }
} 