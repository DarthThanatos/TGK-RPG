
using UnityEngine;
using UnityEngine.UI;

public class NavigationUI : MonoBehaviour {

    private RectTransform navigationPanel;
    private Image upArrow, downArrow, leftArrow, rightArrow, leftUpArrow, rightUpArrow, leftDownArrow,rightDownArrow; 

    private Vector3 targetPosition;


    public void Init(Vector3 targetPosition)
    {
        navigationPanel = GetComponent<RectTransform>();

        upArrow = navigationPanel.transform.Find("UpArrow").GetComponent<Image>();
        downArrow = navigationPanel.transform.Find("DownArrow").GetComponent<Image>();
        leftArrow = navigationPanel.transform.Find("LeftArrow").GetComponent<Image>();
        rightArrow = navigationPanel.transform.Find("RightArrow").GetComponent<Image>();
        leftUpArrow = navigationPanel.transform.Find("LeftUpArrow").GetComponent<Image>();
        rightUpArrow = navigationPanel.transform.Find("RightUpArrow").GetComponent<Image>();
        rightDownArrow = navigationPanel.transform.Find("RightDownArrow").GetComponent<Image>();
        leftDownArrow = navigationPanel.transform.Find("LeftDownArrow").GetComponent<Image>();

        navigationPanel.gameObject.SetActive(true);
        this.targetPosition = targetPosition;

        UpdateArrow();
    }

    void OnGUI()
    {
        UpdateArrow();
    }


    private void UpdateArrow()
    {

        Camera camera = Camera.main;
        if (CameraCanSeePoint(camera))
        {
            DeactivateAllArrows();
        }
        else
        {

            RectTransform rectTransform = navigationPanel.GetComponent<RectTransform>();
            rectTransform.offsetMin = new Vector2(0, 0);
            rectTransform.offsetMax = new Vector2(0, 0);

            Vector3 playerPos = GameObject.Find("Player").gameObject.transform.position;
            Vector3 point = new Vector3(targetPosition.x, targetPosition.y, targetPosition.z);

            Vector3 viewportPoint = camera.WorldToViewportPoint(point);

            DeactivateAllArrows();

            if (viewportPoint.x < 1 && viewportPoint.x > 0)
            {
                SetupVerticalArrows(viewportPoint);
            }
            else if (viewportPoint.y < 1 && viewportPoint.y > 0)
            {
                SetupHorizontalArrows(viewportPoint);
            }
            else
            {
                SetupCornerArrows(viewportPoint);
            }

        }
    }

    private void DeactivateAllArrows()
    {
        for (int i = 0; i < navigationPanel.childCount; i++)
        {
            navigationPanel.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void SetupCornerArrows(Vector3 viewportPoint)
    {
        if(viewportPoint.x < 0 && viewportPoint.y < 0)
        {
            leftDownArrow.gameObject.SetActive(true);
        }
        if (viewportPoint.x < 0 && viewportPoint.y > 1)
        {
            leftUpArrow.gameObject.SetActive(true);
        }
        if(viewportPoint.x > 1 && viewportPoint.y < 0)
        {
            rightDownArrow.gameObject.SetActive(true);
        }
        if(viewportPoint.x > 1 && viewportPoint.y > 1)
        {
            rightUpArrow.gameObject.SetActive(true);
        }
    }

    private void SetupVerticalArrows(Vector3 viewportPoint)
    {
        float z = navigationPanel.transform.position.z;
        float x = viewportPoint.x * navigationPanel.GetComponent<RectTransform>().rect.width;

        if(viewportPoint.y > 1)
        {
            upArrow.gameObject.SetActive(true);
            RectTransform rectTransform = upArrow.GetComponent<RectTransform>();
            rectTransform.position = new Vector2(x, rectTransform.position.y);
        }
        else
        {
            downArrow.gameObject.SetActive(true);
            RectTransform rectTransform = downArrow.GetComponent<RectTransform>();
            rectTransform.position = new Vector2(x, rectTransform.position.y);
        }
    }

    private void SetupHorizontalArrows(Vector3 viewportPoint)
    {
        float z = navigationPanel.transform.position.z;
        float y = viewportPoint.y * navigationPanel.GetComponent<RectTransform>().rect.height;

        if (viewportPoint.x > 1)
        {
            rightArrow.gameObject.SetActive(true);
            RectTransform rectTransform = rightArrow.GetComponent<RectTransform>();
            rectTransform.position = new Vector2(rectTransform.position.x, y);
        }
        else
        {
            leftArrow.gameObject.SetActive(true);
            RectTransform rectTransform = leftArrow.GetComponent<RectTransform>();
            rectTransform.position = new Vector2(rectTransform.position.x, y);
        }
    }

    bool CameraCanSeePoint(Camera camera)
    {
        Vector3 point = targetPosition;
        Vector3 viewportPoint = camera.WorldToViewportPoint(point);
        return (viewportPoint.z > 0 && (new Rect(0, 0, 1, 1)).Contains(viewportPoint));
    }

}
