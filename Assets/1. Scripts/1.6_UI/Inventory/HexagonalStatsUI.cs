using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public class HexagonalStatsUI : Graphic
{
    public float radius = 100f;
    public int sides = 6;

    private float[] values; // Giá trị hiện tại
    private float[] targetValues; // Giá trị mục tiêu

    public float smoothSpeed = 5f; // Tốc độ mượt
    private bool isUpdating = false; // Trạng thái cập nhật

    public void SetFloat(float[] targetValues)
    {
        if (targetValues == null || targetValues.Length != sides)
        {
            Debug.LogError("Target values must match the number of sides.");
            return;
        }

        if (this.targetValues == null || this.targetValues.Length != targetValues.Length)
        {
            this.targetValues = new float[targetValues.Length];
            values = new float[targetValues.Length];
        }

        for (int i = 0; i < targetValues.Length; i++)
        {
            this.targetValues[i] = targetValues[i];
        }

        isUpdating = true; // Bắt đầu cập nhật
    }

    private void Update()
    {
        if (!isUpdating) return; // Không cập nhật nếu đã xong

        bool allReached = true;

        for (int i = 0; i < values.Length; i++)
        {
            if (Mathf.Abs(values[i] - targetValues[i]) > 0.01f) // Chưa đạt mục tiêu
            {
                values[i] = Mathf.Lerp(values[i], targetValues[i], Time.deltaTime * smoothSpeed);
                allReached = false;
            }
            else
            {
                values[i] = targetValues[i]; // Đảm bảo giá trị cuối cùng chính xác
            }
        }

        if (allReached)
        {
            isUpdating = false; // Dừng cập nhật khi tất cả đã đạt mục tiêu
        }

        SetVerticesDirty(); // Yêu cầu vẽ lại UI
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        Vector2 center = Vector2.zero; // Tâm lục giác
        vh.AddVert(center, color, Vector2.zero);

        // Tạo các điểm trên lục giác
        for (int i = 0; i < sides; i++)
        {
            float angle = i * Mathf.PI * 2 / sides;
            float value = values != null && i < values.Length ? values[i] : 1f;

            Vector2 point = new Vector2(
                Mathf.Cos(angle) * radius * value,
                Mathf.Sin(angle) * radius * value
            );

            vh.AddVert(point, color, Vector2.zero);
        }

        // Kết nối các điểm để tạo tam giác
        for (int i = 1; i <= sides; i++)
        {
            int next = i == sides ? 1 : i + 1;
            vh.AddTriangle(0, i, next);
        }
    }
}
