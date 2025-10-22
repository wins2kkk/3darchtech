using UnityEngine;

public class ScrollContent : MonoBehaviour
{
    #region Public Properties

    public float ItemSpacing { get { return actualItemSpacing; } }
    public float HorizontalMargin { get { return horizontalMargin; } }
    public float VerticalMargin { get { return verticalMargin; } }
    public bool Horizontal { get { return horizontal; } }
    public bool Vertical { get { return vertical; } }
    public float Width { get { return actualWidth; } }
    public float Height { get { return actualHeight; } }
    public float ChildWidth { get { return actualChildWidth; } }
    public float ChildHeight { get { return actualChildHeight; } }

    #endregion

    #region Private Members

    private RectTransform rectTransform;
    private RectTransform[] rtChildren;

    private float width, height;
    private float childWidth, childHeight;

     // độ rộng và chiều cao thực tế khi đưa lên màn hình, trường hợp canvas bị scale xuống hoặc scale lên thì width và height thực tế sẽ khác
    private float actualWidth, actualHeight;
    private float actualItemSpacing;
    private float actualChildWidth, actualChildHeight;

    [SerializeField] private RectTransform _mainCanvas;
    [SerializeField] private float itemSpacing;

    [SerializeField] private int maxItemVisible;
    [SerializeField] private float horizontalMargin, verticalMargin;
    [SerializeField] private bool horizontal, vertical;

    #endregion

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        rtChildren = new RectTransform[rectTransform.childCount];

        for (int i = 0; i < rectTransform.childCount; i++)
        {
            rtChildren[i] = rectTransform.GetChild(i) as RectTransform;
        }

        // Subtract the margin from both sides.
        width = rectTransform.rect.width - (2 * horizontalMargin);

        // Subtract the margin from the top and bottom.
        height = rectTransform.rect.height - (2 * verticalMargin);

        childWidth = rtChildren[0].rect.width;
        childHeight = rtChildren[0].rect.height;

        horizontal = !vertical;
        if (vertical)
            InitializeContentVertical();
        else
            InitializeContentHorizontal();

        CalculateActualParameter();
    }

    /// <summary>
    /// Initializes the scroll content if the scroll view is oriented horizontally.
    /// </summary>
    private void InitializeContentHorizontal()
    {
        itemSpacing = (width - (horizontalMargin * 2) - (childWidth * maxItemVisible)) / (maxItemVisible - 1);
        float originX = 0 - (width * 0.5f);
        float posOffset = childWidth * 0.5f;
        for (int i = 0; i < rtChildren.Length; i++)
        {
            Vector2 childPos = rtChildren[i].localPosition;
            childPos.x = originX + posOffset + i * (childWidth + itemSpacing);
            rtChildren[i].localPosition = childPos;
        }
    }

    /// <summary>
    /// Initializes the scroll content if the scroll view is oriented vertically.
    /// </summary>
    private void InitializeContentVertical()
    {
        itemSpacing = (height - (verticalMargin * 2) - (childHeight * maxItemVisible)) / (maxItemVisible - 1);
        float originY = 0 - (height * 0.5f);
        float posOffset = childHeight * 0.5f;
        for (int i = 0; i < rtChildren.Length; i++)
        {
            Vector2 childPos = rtChildren[i].localPosition;
            childPos.y = originY + posOffset + i * (childHeight + itemSpacing);
            rtChildren[i].localPosition = childPos;
        }
    }

    private void CalculateActualParameter()
    {
        Vector3 canvasScale = _mainCanvas.localScale;

        actualChildWidth = childWidth * canvasScale.x;
        actualChildHeight = childHeight * canvasScale.y;

        actualHeight = height * canvasScale.x;
        actualWidth = width * canvasScale.x;
        
        if(vertical)
        {
            actualItemSpacing = itemSpacing * canvasScale.y; 
        }
        else 
        {
            actualItemSpacing = itemSpacing * canvasScale.x; 
        }
    }
}
