using UnityEngine;

namespace ChaseMacMillan.CurveDesigner
{
    public class CurveFollower : MonoBehaviour
    {
        public Curve3D curve;
        public float distanceAlongCurve = 0;
        public float speed = 1;

        private void Update()
        {
            if (curve != null && speed != 0.0f)
                SetDistanceAlongCurve(distanceAlongCurve + Time.deltaTime * speed);
        }

        public void SetDistanceAlongCurve(float distanceAlongCurve)
        {
            this.distanceAlongCurve = distanceAlongCurve;
            PointOnCurve point = curve.GetPointAtDistanceAlongCurve(distanceAlongCurve);
            transform.position = point.position;
            transform.rotation = Quaternion.LookRotation(point.tangent, point.reference);
        }
    }
}
