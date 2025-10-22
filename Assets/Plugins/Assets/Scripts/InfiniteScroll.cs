using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfiniteScroll : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IScrollHandler
{
    #region Private Members

    /// <summary>
    /// The ScrollContent component that belongs to the scroll content GameObject.
    /// </summary>
    [SerializeField] private ScrollContent scrollContent;

    /// <summary>
    /// How far the items will travel outside of the scroll view before being repositioned.
    /// </summary>
    [SerializeField] private float outOfBoundsThreshold;

    [SerializeField] private float minVelocityApplyAutoScroll = 20f;

    /// <summary>
    /// The ScrollRect component for this GameObject.
    /// </summary>
    private ScrollRect scrollRect;

    /// <summary>
    /// The last position where the user has dragged.
    /// </summary>
    private Vector2 lastDragPosition;

    /// <summary>
    /// Nếu user kéo scrollview qua bên phải, positiveDrag = true, content sẽ đi về bên trái
    /// </summary>
    private bool positiveDrag;

    private Transform _firstItem;

    private GameObject _lastItem;
    private GameObject _currentItem {get; set;}

    private Coroutine _autoScrollRoutine;
    
    public UnityEvent<GameObject> OnSelectMiddleItem;
    public UnityEvent<GameObject> OnUnSelectMiddleItem;

    #endregion

    private void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        // scrollRect.vertical = scrollContent.Vertical;
        // scrollRect.horizontal = scrollContent.Horizontal;
        scrollRect.movementType = ScrollRect.MovementType.Unrestricted;

        GetMiddleTransform();
    }

    private void OnEnable() 
    {
        if(scrollRect != null) GetMiddleTransform();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        lastDragPosition = eventData.position;
        StopAutoScroll();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (scrollContent.Vertical)
        {
            positiveDrag = eventData.position.y > lastDragPosition.y;
        }
        else if (scrollContent.Horizontal)
        {
            positiveDrag = eventData.position.x > lastDragPosition.x;
        }

        lastDragPosition = eventData.position;
    }

    public void OnScroll(PointerEventData eventData)
    {
        if (scrollContent.Vertical)
        {
            positiveDrag = eventData.scrollDelta.y > 0;
        }
        else
        {
            // Scrolling up on the mouse wheel is considered a negative scroll, but I defined
            // scrolling downwards (scrolls right in a horizontal view) as the positive direciton,
            // so I check if the if scrollDelta.y is less than zero to check for a positive drag.
            positiveDrag = eventData.scrollDelta.y < 0;
        }
    }

    public void OnViewScroll()
    {
        if (scrollContent.Vertical)
        {
            HandleVerticalScroll();
        }
        else
        {
            HandleHorizontalScroll();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        AutoScrollToCurrentItem();
    }

    public void ScrollToItem(GameObject item)
    {
        StartCoroutine(SrollToItem(item));
    }

    public IEnumerator SrollToItem(GameObject item)
    {
        yield return new WaitUntil(()=> _currentItem != null);
        if (scrollContent.Vertical)
        {
            positiveDrag = item.transform.position.y < _currentItem.transform.position.y;
        }
        else if (scrollContent.Horizontal)
        {
            //item ở bên phải so với vị trí giữa, thì sẽ lướt content qua trái
            //khi lướt content qua trái positiveDrag = true;
            positiveDrag = item.transform.position.x < _currentItem.transform.position.x;
        }
        StopAutoScroll();
        AutoScrollToTargettItem(item);
    }

    private void HandleVerticalScroll()
    {
        int currItemIndex = positiveDrag ? scrollRect.content.childCount - 1 : 0;
        var currItem = scrollRect.content.GetChild(currItemIndex);

        if (!ReachedThreshold(currItem))
        {
            return;
        }

        int endItemIndex = positiveDrag ? 0 : scrollRect.content.childCount - 1;
        Transform endItem = scrollRect.content.GetChild(endItemIndex);
        Vector2 newPos = endItem.position;

        if (positiveDrag)
        {
            newPos.y = endItem.position.y - scrollContent.ChildHeight - scrollContent.ItemSpacing;
        }
        else
        {
            newPos.y = endItem.position.y + scrollContent.ChildHeight + scrollContent.ItemSpacing;
        }

        currItem.position = newPos;
        currItem.SetSiblingIndex(endItemIndex);

        GetMiddleTransform();
    }

    private void HandleHorizontalScroll()
    {
        //nếu positiveDrag = true là chuột đi qua phải -> content đi qua bên trái
        int currItemIndex = positiveDrag ? scrollRect.content.childCount - 1 : 0;
        var currItem = scrollRect.content.GetChild(currItemIndex);
    
        if (!ReachedThreshold(currItem))
        {
            return;
        }

        int endItemIndex = positiveDrag ? 0 : scrollRect.content.childCount - 1;
        Transform endItem = scrollRect.content.GetChild(endItemIndex);
        Vector2 newPos = endItem.position;

        if (positiveDrag)
        {
            newPos.x = endItem.position.x - scrollContent.ChildWidth - scrollContent.ItemSpacing;
        }
        else
        {
            newPos.x = endItem.position.x + scrollContent.ChildWidth + scrollContent.ItemSpacing;
        }

        currItem.position = newPos;
        currItem.SetSiblingIndex(endItemIndex);

        GetMiddleTransform();
    }

    private bool ReachedThreshold(Transform item)
    {          
        if (scrollContent.Vertical)
        {
            float posYThreshold = transform.position.y + scrollContent.Height * 0.5f + outOfBoundsThreshold;
            float negYThreshold = transform.position.y - scrollContent.Height * 0.5f - outOfBoundsThreshold;
            return positiveDrag ? item.position.y - scrollContent.ChildHeight * 0.5f > posYThreshold :
                item.position.y + scrollContent.ChildHeight * 0.5f < negYThreshold;
        }
        else
        {
            float posXThreshold = transform.position.x + scrollContent.Width * 0.5f + outOfBoundsThreshold;
            float negXThreshold = transform.position.x - scrollContent.Width * 0.5f - outOfBoundsThreshold;
            return positiveDrag ? item.position.x - scrollContent.ChildWidth * 0.5f > posXThreshold :
                item.position.x + scrollContent.ChildWidth * 0.5f < negXThreshold;
        }
    }

    private void GetMiddleTransform()
    {
        int nearestIndex = GetIndexByPosition(scrollRect.transform.position); // vị trí chính giữa của scroll
        GameObject midItem = scrollRect.content.GetChild(nearestIndex).gameObject;
        SetCurrentItem(midItem);
    }

    private int GetIndexByPosition(Vector2 position)
    {
        _firstItem = scrollRect.content.GetChild(0);
        if(scrollContent.Vertical)
        {
            return Mathf.RoundToInt((position.y - _firstItem.position.y) / (scrollContent.ChildHeight + scrollContent.ItemSpacing));
        }
        else
        {
            return Mathf.RoundToInt((position.x - _firstItem.position.x) / (scrollContent.ChildWidth + scrollContent.ItemSpacing));
        }
    }

    private void AutoScrollToCurrentItem()
    {
       _autoScrollRoutine = StartCoroutine(SmoothScrollToPosition());
    }

    private void AutoScrollToTargettItem(GameObject target)
    {
       _autoScrollRoutine = StartCoroutine(SmoothScrollToPosition(target));
    }

    private void StopAutoScroll()
    {
        if(_autoScrollRoutine != null)
        {
            StopCoroutine(_autoScrollRoutine);
        }
    }

    private IEnumerator SmoothScrollToPosition(GameObject target = null)
    {
        float duration = 0.25f;
        float elapsedTime = 0f;

        if(scrollContent.Vertical)
        {
            yield return new WaitUntil(() => Mathf.Abs(scrollRect.velocity.y) < minVelocityApplyAutoScroll);
        }
        else 
        {
            yield return new WaitUntil(() => Mathf.Abs(scrollRect.velocity.x) < minVelocityApplyAutoScroll);
        }

        scrollRect.velocity = Vector2.zero;
        Vector2 targetPosition;
        if(target != null)
        {
            targetPosition = GetTargetPosition(target);
        }
        else 
        {
            targetPosition = GetTargetPosition(_currentItem);
        }
        Vector2 startPosition = scrollRect.content.position;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            scrollRect.content.position = Vector2.Lerp(startPosition, targetPosition, elapsedTime / duration);
            yield return null;
        }

        scrollRect.content.position = targetPosition;
    }

    private Vector2 GetTargetPosition(GameObject target)
    {
        Vector2 targetPosition = scrollRect.content.position;
        Vector2 contentCenter = scrollRect.transform.position;

        if (scrollContent.Horizontal)
        {
            float itemCenterX = target.transform.position.x;
            float scrollOffsetX = contentCenter.x - itemCenterX;
            targetPosition.x += scrollOffsetX;
        }
        else if (scrollContent.Vertical)
        {
            float itemCenterY = target.transform.position.y;
            float scrollOffsetY = contentCenter.y - itemCenterY;
            targetPosition.y += scrollOffsetY;
        }

        return targetPosition;
    }

    private void SetCurrentItem(GameObject item, bool callSelect = true, bool callUnSelect = true)
    {
        if(_currentItem == item)
            return;

        _lastItem = _currentItem;
        _currentItem = item;

        if(callSelect) OnSelectMiddleItem?.Invoke(_currentItem);
        if(callUnSelect) OnUnSelectMiddleItem?.Invoke(_lastItem);
    }
}
