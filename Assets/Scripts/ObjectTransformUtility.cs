using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public static class ObjectTransformUtility
{
    public static async Task MoveObjectLerp(GameObject targetObject, float x, float y, float z, float time)
    {
        targetObject.GetComponent<MonoBehaviour>().StartCoroutine(MoveLerp(targetObject.transform, new Vector3(x, y, z), time));
        await Task.Delay((int)(1000 * time));
    }

    public static async Task RotateObjectLerp(GameObject targetObject, float x, float y, float z, float time)
    {
        targetObject.GetComponent<MonoBehaviour>().StartCoroutine(RotateLerp(targetObject.transform, new Quaternion(x, y, z, 1), time));
        await Task.Delay((int)(1000 * time));
    }

    public static async Task ScaleObjectLerp(GameObject targetObject, float x, float y, float z, float time)
    {
        targetObject.GetComponent<MonoBehaviour>().StartCoroutine(ScaleLerp(targetObject.transform, new Vector3(x, y, z), time));
        await Task.Delay((int)(1000 * time));
    }

    public static async Task MoveObjectSmooth(GameObject targetObject, float x, float y, float z, float time)
    {
        targetObject.GetComponent<MonoBehaviour>().StartCoroutine(MoveSmooth(targetObject.transform, new Vector3(x, y, z), time));
        await Task.Delay((int)(1000 * time));
    }

    public static async Task RotateObjectSmooth(GameObject targetObject, float x, float y, float z, float time)
    {
        targetObject.GetComponent<MonoBehaviour>().StartCoroutine(RotateSmooth(targetObject.transform, new Quaternion(x, y, z, 1), time));
        await Task.Delay((int)(1000 * time));
    }

    public static async Task ScaleObjectSmooth(GameObject targetObject, float x, float y, float z, float time)
    {
        targetObject.GetComponent<MonoBehaviour>().StartCoroutine(ScaleSmooth(targetObject.transform, new Vector3(x, y, z), time));
        await Task.Delay((int)(1000 * time));
    }

    public static async Task TransitionObjectLerp(GameObject targetObject, Transform start, Transform end, float time)
    {
        targetObject.GetComponent<MonoBehaviour>().StartCoroutine(TransitionLerp(targetObject.transform, start, end, time));
        await Task.Delay((int)(1000 * time));
    }

    public static async Task TransitionObjectSmooth(GameObject targetObject, Transform start, Transform end, float time)
    {
        targetObject.GetComponent<MonoBehaviour>().StartCoroutine(TransitionSmooth(targetObject.transform, start, end, time));
        await Task.Delay((int)(1000 * time));
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

    private static IEnumerator TransitionLerp(Transform target, Transform start, Transform end, float time)
    {
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            target.position = Vector3.Lerp(start.position, end.position, (elapsedTime / time));
            target.rotation = Quaternion.Lerp(start.rotation, end.rotation, (elapsedTime / time));
            target.localScale = Vector3.Lerp(start.localScale, end.localScale, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        target.position = end.position;
        target.rotation = end.rotation;
        target.localScale = end.localScale;
    }

    private static IEnumerator TransitionSmooth(Transform target, Transform start, Transform end, float time)
    {
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            target.position = SmoothstepVector(start.position, end.position, (elapsedTime / time));
            target.rotation = SmoothstepQuaternion(start.rotation, end.rotation, (elapsedTime / time));
            target.localScale = SmoothstepVector(start.localScale, end.localScale, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        target.position = end.position;
        target.rotation = end.rotation;
        target.localScale = end.localScale;
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
