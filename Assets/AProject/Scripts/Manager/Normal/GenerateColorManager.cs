using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour {
    public static ColorManager instance;

    // Danh sách màu đang được sử dụng
    private List<Color> usedColors = new List<Color>();

    // Các thông số cấu hình
    [Header("Cấu hình màu sắc")]
    [Range(0, 1)] public float minSaturation = 0.6f;
    [Range(0, 1)] public float minBrightness = 0.6f;
    [Range(0, 1)] public float minColorDifference = 0.3f;
    [Range(0, 360)] public float hueStep = 15f;

    private float lastHue = 0f;

    void Awake() {
        if (instance == null) {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        //else {
        //    Destroy(gameObject);
        //}
    }

    // Tạo màu ngẫu nhiên với độ sáng và độ bão hòa tốt
    public Color GenerateRandomColor() {
        float hue = Random.Range(0f, 1f);
        float saturation = Random.Range(minSaturation, 1f);
        float brightness = Random.Range(minBrightness, 1f);

        return Color.HSVToRGB(hue, saturation, brightness);
    }

    // Tạo màu dựa trên hue có kiểm soát
    public Color GenerateDistinctColor() {
        lastHue = (lastHue + hueStep / 360f) % 1f;
        float saturation = Random.Range(minSaturation, 1f);
        float brightness = Random.Range(minBrightness, 1f);

        return Color.HSVToRGB(lastHue, saturation, brightness);
    }

    // Kiểm tra xem màu có đủ khác biệt với các màu đã dùng không
    private bool IsColorDistinct(Color newColor) {
        foreach (Color usedColor in usedColors) {
            if (ColorDifference(newColor, usedColor) < minColorDifference) {
                return false;
            }
        }
        return true;
    }

    // Tính độ khác biệt giữa hai màu
    private float ColorDifference(Color color1, Color color2) {
        // Chuyển đổi sang HSV để so sánh Hue
        Color.RGBToHSV(color1, out float h1, out float s1, out float v1);
        Color.RGBToHSV(color2, out float h2, out float s2, out float v2);

        // Tính khoảng cách hue (xử lý tính tuần hoàn của hue)
        float hueDiff = Mathf.Min(Mathf.Abs(h1 - h2), 1 - Mathf.Abs(h1 - h2));

        // Tính khoảng cách tổng thể
        float diff = hueDiff * 0.7f + Mathf.Abs(s1 - s2) * 0.2f + Mathf.Abs(v1 - v2) * 0.1f;

        return diff;
    }

    // Lấy một màu ngẫu nhiên đảm bảo khác biệt
    public Color GetDistinctRandomColor() {
        Color newColor;
        int attempts = 0;
        int maxAttempts = 100; // Tránh vòng lặp vô hạn

        do {
            if (attempts % 2 == 0) {
                newColor = GenerateDistinctColor();
            }
            else {
                newColor = GenerateRandomColor();
            }

            attempts++;

            if (attempts > maxAttempts) {
                Debug.LogWarning("Không thể tìm màu khác biệt sau " + maxAttempts + " lần thử");
                break;
            }
        }
        while (!IsColorDistinct(newColor) && usedColors.Count > 0);

        usedColors.Add(newColor);
        return newColor;
    }

    // Trả lại màu khi không sử dụng nữa
    public void ReturnColor(Color colorToReturn) {
        if (usedColors.Contains(colorToReturn)) {
            usedColors.Remove(colorToReturn);
        }
    }

    // Reset tất cả màu
    public void ResetAllColors() {
        usedColors.Clear();
        lastHue = 0f;
    }

    // Kiểm tra số lượng màu đang dùng
    public int GetUsedColorCount() {
        return usedColors.Count;
    }
}