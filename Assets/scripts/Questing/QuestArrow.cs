
using UnityEngine;

public class QuestArrow : MonoBehaviour {

    private Vector3 targetPosition;

    public Vector3 viewportPoint;

    public void Init(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
        UpdateArrow();
    }

    void OnGUI()
    {
        UpdateArrow();
    }

    private void UpdateArrow()
    {

        gameObject.transform.position = ArrowPosition();
        gameObject.transform.rotation = ArrowRotation();
    }

    private Vector3 ArrowPosition()
    {
        Camera camera = Camera.main;

        if (CameraCanSeePoint(camera))
        {
            return new Vector3(-1000, -1000, -1000);
        }
        else
        {
            Vector3 playerPos = GameObject.Find("Player").gameObject.transform.position;
            Vector3 point = new Vector3(targetPosition.x, camera.transform.position.y, targetPosition.z);

            viewportPoint = camera.WorldToViewportPoint(point);

            Vector3 arrowPos = new Vector3(playerPos.x, playerPos.y + 4, playerPos.z);
            float arrowMaxView = 1f;
            if (viewportPoint.x < 1 && viewportPoint.x > 0)
            {
                arrowPos = camera.ViewportToWorldPoint(
                    new Vector3(
                        viewportPoint.x,
                        viewportPoint.y < 0 ? 1 - arrowMaxView : arrowMaxView,
                        viewportPoint.z
                    )
                );
            }
            else if (viewportPoint.y < 1 && viewportPoint.y > 0)
            {
                arrowPos = camera.ViewportToWorldPoint(
                    new Vector3(
                        viewportPoint.x < 0 ? 1 - arrowMaxView : arrowMaxView,
                        viewportPoint.y,
                        viewportPoint.z
                    )
                );
            }
            else
            {
                arrowPos = camera.ViewportToWorldPoint(
                    new Vector3(
                        viewportPoint.x < 0 ? 1 - arrowMaxView : arrowMaxView,
                        viewportPoint.y < 0 ? 1 - arrowMaxView : arrowMaxView,
                        viewportPoint.z
                    )
                );
            }

            arrowPos.y = 4;
            return arrowPos;
        }

    }

    private Quaternion ArrowRotation()
    {
        Vector3 playerPos = GameObject.Find("Player").gameObject.transform.position;
        Vector3 arrowPos = new Vector3(playerPos.x, playerPos.y + 4, playerPos.z);

        Vector3 targetDir = targetPosition - playerPos;

        float angle =
            Vector2.SignedAngle(
                new Vector2(targetDir.x, targetDir.z),
                new Vector2(-1, 0)
            );

        return Quaternion.Euler(0, angle, 0);
    }

    bool CameraCanSeePoint(Camera camera)
    {
        Vector3 point = targetPosition;
        Vector3 viewportPoint = camera.WorldToViewportPoint(point);
        return (viewportPoint.z > 0 && (new Rect(0, 0, 1, 1)).Contains(viewportPoint));
    }

}
