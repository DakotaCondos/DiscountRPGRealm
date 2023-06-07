using System.Collections;
using UnityEngine;

public static class ObjectTransformUtility
{
    public static void MoveObjectLerp(GameObject targetObject, float x, float y, float z, float time)
    {
        targetObject.GetComponent<MonoBehaviour>().StartCoroutine(MoveLerp(targetObject.transform, new Vector3(x, y, z), time));
    }

    public static void RotateObjectLerp(GameObject targetObject, float x, float y, float z, float time)
    {
        targetObject.GetComponent<MonoBehaviour>().StartCoroutine(RotateLerp(targetObject.transform, new Quaternion(x, y, z, 1), time));
    }

    public static void ScaleObjectLerp(GameObject targetObject, float x, float y, float z, float time)
    {
        targetObject.GetComponent<MonoBehaviour>().StartCoroutine(ScaleLerp(targetObject.transform, new Vector3(x, y, z), time));
    }

    public static void MoveObjectSmooth(GameObject targetObject, float x, float y, float z, float time)
    {
        targetObject.GetComponent<MonoBehaviour>().StartCoroutine(MoveSmooth(targetObject.transform, new Vector3(x, y, z), time));
    }

    public static void RotateObjectSmooth(GameObject targetObject, float x, float y, float z, float time)
    {
        targetObject.GetComponent<MonoBehaviour>().StartCoroutine(RotateSmooth(targetObject.transform, new Quaternion(x, y, z, 1), time));
    }

    public static void ScaleObjectSmooth(GameObject targetObject, float x, float y, float z, float time)
    {
        targetObject.GetComponent<MonoBehaviour>().StartCoroutine(ScaleSmooth(targetObject.transform, new Vector3(x, y, z), time));
    }

    private static IEnumerator MoveLerp(Transform target, Vector3 endPos, float time)
    {
        return Move(target, endPos, time, Vector3.Lerp);
    }

    private static IEnumerator RotateLerp(Transform target, Quaternion endRot, float time)
    {
        return Rotate(target, endRot, time, Quaternion.Lerp);
    }

    private static IEnumerator ScaleLerp(Transform target, Vector3 endScale, float time)
    {
        return Scale(target, endScale, time, Vector3.Lerp);
    }

    private static IEnumerator MoveSmooth(Transform target, Vector3 endPos, float time)
    {
        return Move(target, endPos, time, SmoothstepVector);
    }

    private static IEnumerator RotateSmooth(Transform target, Quaternion endRot, float time)
    {
        return Rotate(target, endRot, time, SmoothstepQuaternion);
    }

    private static IEnumerator ScaleSmooth(Transform target, Vector3 endScale, float time)
    {
        return Scale(target, endScale, time, SmoothstepVector);
    }

    private static IEnumerator Move(Transform target, Vector3 endPos, float time, System.Func<Vector3, Vector3, float, Vector3> interpolate)
    {
        float elapsedTime = 0;
        Vector3 startingPos = target.position;
        while (elapsedTime < time)
        {
            target.position = interpolate(startingPos, endPos, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        target.position = endPos;
    }

    private static IEnumerator Rotate(Transform target, Quaternion endRot, float time, System.Func<Quaternion, Quaternion, float, Quaternion> interpolate)
    {
        float elapsedTime = 0;
        Quaternion startingRot = target.rotation;
        while (elapsedTime < time)
        {
            target.rotation = interpolate(startingRot, endRot, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        target.rotation = endRot;
    }

    private static IEnumerator Scale(Transform target, Vector3 endScale, float time, System.Func<Vector3, Vector3, float, Vector3> interpolate)
    {
        float elapsedTime = 0;
        Vector3 startingScale = target.localScale;
        while (elapsedTime < time)
        {
            target.localScale = interpolate(startingScale, endScale, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        target.localScale = endScale;
    }

    private static float Smoothstep(float x)
    {
        return x * x * (3 - 2 * x);
    }

    private static Vector3 SmoothstepVector(Vector3 start, Vector3 end, float x)
    {
        float smoothX = Smoothstep(x);
        return Vector3.Lerp(start, end, smoothX);
    }

    private static Quaternion SmoothstepQuaternion(Quaternion start, Quaternion end, float x)
    {
        float smoothX = Smoothstep(x);
        return Quaternion.Lerp(start, end, smoothX);
    }
}
