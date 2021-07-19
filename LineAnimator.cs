using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineAnimator : MonoBehaviour
{
    [SerializeField] private float animationDuration = 5f;
    private PolygonCollider2D poly;
    private Vector2[] path;

    private LineRenderer lineRenderer;
    private Vector3[] linePoints;
    private int pointsCount;
    public void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        pointsCount = lineRenderer.positionCount;
        linePoints = new Vector3[pointsCount];
        for (int i = 0; i < pointsCount; i++)
        {
            linePoints[i] = lineRenderer.GetPosition(i);
        }
        path = new Vector2[2 * pointsCount];
        poly = GetComponent<PolygonCollider2D>();
        StartCoroutine(AnimateLine());
    }
    private IEnumerator AnimateLine()
    {
        float segmentDuration = animationDuration / pointsCount;
        for (int i = 0; i < pointsCount - 1; i++)
        {
            float startTime = Time.time;
            Vector3 startPosition = linePoints[i];
            Vector3 endPosition = linePoints[i + 1];
            Vector3 pos = startPosition;
            while (pos != endPosition)
            {
                float t = (Time.time - startTime) / segmentDuration;
                pos = Vector3.Lerp(startPosition, endPosition, t);
                for (int j = i + 1; j < pointsCount; j++)
                    lineRenderer.SetPosition(j, pos);
                yield return null;

                int pathp = 0;
                for (int p = 0; p < pointsCount; p++)
                {
                    path[pathp++] = lineRenderer.GetPosition(p);
                }
                for (int p = pointsCount - 1; p >= 0; p--)
                {
                    path[pathp++] = lineRenderer.GetPosition(p) - Vector3.up * .2f;
                }
                poly.SetPath(0, path);
								
								Collider2D collider = Physics2D.OverlapCircle(lineRenderer.GetPosition(pointsCount - 1), 1);
								if (collider != null) {
									Debug.Log("Collided with " + collider.gameObejct.name);
								}
            }
        }
    }
}

