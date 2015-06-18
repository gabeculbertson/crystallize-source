using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlowLayoutGroup : LayoutGroup {

    [SerializeField]
    protected float m_HorizontalSpacing = 0;
    public float horizontalSpacing { get { return m_HorizontalSpacing; } set { SetProperty(ref m_HorizontalSpacing, value); } }

    [SerializeField]
    protected float m_VerticalSpacing = 0;
    public float verticalSpacing { get { return m_VerticalSpacing; } set { SetProperty(ref m_VerticalSpacing, value); } }

    [SerializeField]
    protected float m_Width = 0;
    public float width { get { return m_Width; } set { SetProperty(ref m_Width, value); } }

    [SerializeField]
    protected bool m_ChildForceExpandWidth = true;
    public bool childForceExpandWidth { get { return m_ChildForceExpandWidth; } set { SetProperty(ref m_ChildForceExpandWidth, value); } }

    protected FlowLayoutGroup()
    {

    }

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();
        SetLayoutInputForAxis(width, width, width, 0);
    }

    public override void CalculateLayoutInputVertical()
    {
        float tmp;
        SetChildrenAlongAxis(1, false, out tmp);
        SetLayoutInputForAxis(tmp, tmp, tmp, 1);
    }

    public override void SetLayoutHorizontal()
    {
        float tmp;
        SetChildrenAlongAxis(0, true, out tmp);
    }

    public override void SetLayoutVertical()
    {
        float tmp;
        SetChildrenAlongAxis(1, true, out tmp);
    }

    protected void SetChildrenAlongAxis(int axis, bool set, out float height)
    {
        float sizeH = rectTransform.rect.size[0];
        //float sizeV = rectTransform.rect.size[1];

        float posV = 0;
        float maxHeight = 0;

        float posH = padding.left;
        if (GetTotalFlexibleSize(0) == 0 && GetTotalPreferredSize(0) < sizeH)
            posH = GetStartOffset(0, GetTotalPreferredSize(0) - padding.horizontal);

        for (int i = 0; i < rectChildren.Count; i++)
        {
            RectTransform child = rectChildren[i];
            float childWidth = LayoutUtility.GetPreferredSize(child, 0);

            float requiredHeight = LayoutUtility.GetPreferredSize(child, 1);
            float startOffset = GetStartOffset(1, requiredHeight);

            if (requiredHeight > maxHeight)
            {
                maxHeight = requiredHeight;
            }

            if (posH + childWidth > sizeH)
            {
                posH = padding.left;
                if (GetTotalFlexibleSize(0) == 0 && GetTotalPreferredSize(0) < sizeH)
                    posH = GetStartOffset(0, GetTotalPreferredSize(0) - padding.horizontal);

                posV += maxHeight + verticalSpacing;
                maxHeight = 0;
            }

            if (set)
            {
                if (axis == 0)
                {
                    SetChildAlongAxis(child, 0, posH, childWidth);
                }
                else
                {
                    SetChildAlongAxis(child, 1, startOffset + posV, requiredHeight);
                }
            }
            posH += childWidth + horizontalSpacing;
        }

        height = posV + maxHeight + padding.bottom;
    }

}
