using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    [AddComponentMenu("Layout/Image Fitter", 142)]
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Image))]
    [DisallowMultipleComponent]
    /// <summary>
    /// Resizes a RectTransform to fill its parent
    /// </summary>
    public class ImageFitter : MonoBehaviour
    {
        private RectTransform m_rectTransform;
        private Image m_image;
        private bool hasParent = false;
        [SerializeField] private bool update = false;

        // Field is never assigned warning
        #pragma warning disable 649
        private DrivenRectTransformTracker tracker;
        #pragma warning restore 649

        private RectTransform rectTransform
        {
            get
            {
                if (m_rectTransform == null)
                    m_rectTransform = GetComponent<RectTransform>();
                return m_rectTransform;
            }
        }

        private Image image
        {
            get
            {
                if (m_image == null)
                    m_image = GetComponent<Image>();
                return m_image;
            }
        }

        private void OnEnable()
        {
            hasParent = rectTransform.parent ? true : false;
            update = true;
        }

        private void OnDisable()
        {
            tracker.Clear();
        }

        private void OnTransformParentChanged()
        {
            hasParent = rectTransform.parent ? true : false;
            update = true;
        }

        private void Update()
        {
            if (update)
            {
                update = false;
                UpdateRect();
            }
        }

        public void UpdateRect()
        {
            if (!hasParent || !IsComponentValidOnObject())
                return;

            tracker.Clear();
            tracker.Add(this, rectTransform,
                        DrivenTransformProperties.Anchors |
                        DrivenTransformProperties.AnchoredPosition |
                        DrivenTransformProperties.SizeDeltaX |
                        DrivenTransformProperties.SizeDeltaY);

            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchoredPosition = Vector2.zero;

            if (!image.sprite)
            {
                rectTransform.sizeDelta = Vector2.zero;
                return;
            }

            Vector2 sizeDelta = Vector2.zero;
            Vector2 parentSize = GetParentSize();

            float aspectRatio = (float)image.sprite.texture.width / image.sprite.texture.height;

            if (parentSize.y * aspectRatio < parentSize.x)
            {
                sizeDelta.y = GetSizeDeltaToProduceSize(parentSize.x / aspectRatio, 1);
            }
            else
            {
                sizeDelta.x = GetSizeDeltaToProduceSize(parentSize.y * aspectRatio, 0);
            }

            rectTransform.sizeDelta = sizeDelta;
        }

        private float GetSizeDeltaToProduceSize(float size, int axis)
        {
            return size - GetParentSize()[axis] * (rectTransform.anchorMax[axis] - rectTransform.anchorMin[axis]);
        }

        private Vector2 GetParentSize()
        {
            RectTransform parent = rectTransform.parent as RectTransform;
            return !parent ? Vector2.zero : parent.rect.size;
        }

        public bool IsComponentValidOnObject()
        {
            Canvas canvas = gameObject.GetComponent<Canvas>();
            if (canvas && canvas.isRootCanvas && canvas.renderMode != RenderMode.WorldSpace)
            {
                return false;
            }
            return true;
        }

    }
}
