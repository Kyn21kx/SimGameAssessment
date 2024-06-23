/// THIS FILE WAS REUSED FROM OTHER PROJECTS.
/// ALL IMPLEMENTATIONS ARE MY OWN UNLESS CITED THE CONTRARY.

using System;
using UnityEngine;

namespace Auxiliars {
    public static class SpartanMath {

        public static void Clamp(ref float n, float min, float max)
        {
            n = n > max ? max : n;
            n = n < min ? min : n;
        }

        public static float Clamp(float n, float min, float max)
        {
            n = n > max ? max : n;
            return n < min ? min : n;
        }

        public static float RoundToSignificantFigures(float n, int digits)
        {
            if (n == 0) return 0;

            float scale = Mathf.Pow(10, Mathf.Floor(Mathf.Log10(Mathf.Abs(n))) + 1);
            return scale * MathF.Round(n / scale, digits);
        }

        public static Vector3 RemapUpToForward(in Vector3 toRemap, bool preserveZ = false)
        {
            float zValue = preserveZ ? toRemap.z : 0f;
            return new Vector3(toRemap.x, zValue, toRemap.y);
        }

        public static Vector3 RemapUpToForward(in Vector2 toRemap) => new Vector3(toRemap.x, 0f, toRemap.y);

        public static float Exp(float power) => (float)System.Math.Exp(power);

        public static float Ln(float value) => (float)System.Math.Log(value);

        public static float Log(float value, float n) => Ln(value) / Ln(n);

        public static float FastPow(float value, float power) => Exp(power * Ln(value));

        public static float SmoothStart(float from, float to, float t, float n)
        {
            float powerValue = FastPow(t, n);
            return Lerp(from, to, powerValue);
        }
        public static Vector3 SmoothStart(Vector3 from, Vector3 to, float t, float n)
        {
            float powerValue = FastPow(t, n);
            Clamp(ref powerValue, 0f, 1f);
            return new Vector3(
                LerpUnclamped(from.x, to.x, powerValue),
                LerpUnclamped(from.y, to.y, powerValue),
                LerpUnclamped(from.z, to.z, powerValue)
            );
        }

        public static float SmoothStop(float from, float to, float t, float n)
        {
            float powerValue = 1f - FastPow(1f - t, n);
            return Lerp(from, to, powerValue);
        }

        public static Vector3 SmoothStop(Vector3 from, Vector3 to, float t, float n)
        {
            float powerValue = 1f - FastPow(1f - t, n);
            Clamp(ref powerValue, 0f, 1f);
            return new Vector3(
                LerpUnclamped(from.x, to.x, powerValue),
                LerpUnclamped(from.y, to.y, powerValue),
                LerpUnclamped(from.z, to.z, powerValue)
            );
        }

        public static float Lerp(float from, float to, float t)
        {
            Clamp(ref t, 0f, 1f);
            return from + (to - from) * t;
        }


        public static Vector3 Lerp(Vector3 from, Vector3 to, float t)
        {
            Clamp(ref t, 0f, 1f);
            return new Vector3(
                LerpUnclamped(from.x, to.x, t),
                LerpUnclamped(from.y, to.y, t),
                LerpUnclamped(from.z, to.z, t)
            );
        }

        public static float LerpUnclamped(float from, float to, float t)
        {
            return from + (to - from) * t;
        }

        public static Vector3 LerpUnclamped(Vector3 from, Vector3 to, float t)
        {
            return new Vector3(
                LerpUnclamped(from.x, to.x, t),
                LerpUnclamped(from.y, to.y, t),
                LerpUnclamped(from.z, to.z, t)
            );
        }

        public static float InverseLerp(float from, float to, float t)
        {
            Clamp(ref t, 0f, 1f);
            return (t - from) / to - from;
        }

        /// <summary>
        /// Exclusive Between operation
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="from">Exclusive starting point</param>
        /// <param name="to">Exclusive ending point</param>
        /// <returns>True if it's neither limit, but rather is between them</returns>
        public static bool BetweenEx(float value, float from, float to)
        {
            return value > from && value < to;
        }

        public static int Sign(float x) => x >= 0f ? 1 : -1;


        public static float DistanceSqr(Vector3 a, Vector3 b)
        {
            //So, let's substract, right?
            Vector3 towards = a - b;
            return towards.sqrMagnitude;
        }

        public static bool ArrivedAt(Vector3 from, Vector3 to, float threshold = 0.2f)
        {
            //Get the distance
            float sqrDis = DistanceSqr(from, to);
            if (sqrDis <= threshold * threshold)
            {
                return true;
            }
            return false;
        }

        public static bool ArrivedAt(float from, float to, float threshold = 0.2f)
        {
            //Get the distance
            float dis = Mathf.Abs(to - from);
            return dis <= threshold;
        }

        public static bool IsInLayerMask(int value, LayerMask mask)
        {
            return (mask & 1 << value) == 1 << value;
        }

        public static Quaternion LookTowardsDirection(Vector3 forward, Vector3 direction, float offsetAngle = 90f)
        {
            float alpha = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - offsetAngle;
            Quaternion result = Quaternion.AngleAxis(alpha, forward);
            return result;
        }

    }
}