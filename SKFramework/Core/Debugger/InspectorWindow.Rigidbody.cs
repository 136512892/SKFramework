using UnityEngine;

namespace SK.Framework.Debugger
{
    [ComponentInspector(typeof(Rigidbody))]
    public class RigidbodyInspector : ComponentInspector
    {
        protected override void OnDraw(Component component)
        {
            Rigidbody rigidbody = component as Rigidbody;

            rigidbody.mass = DrawFloat("Mass", rigidbody.mass, 125f, GUILayout.ExpandWidth(true));

            rigidbody.drag = DrawFloat("Drag", rigidbody.drag, 125f, GUILayout.ExpandWidth(true));

            rigidbody.angularDrag = DrawFloat("Angular Drag", rigidbody.angularDrag, 125f, GUILayout.ExpandWidth(true));

            rigidbody.useGravity = DrawToggle("Use Gravity", rigidbody.useGravity, 125f);

            rigidbody.isKinematic = DrawToggle("Is Kinematic", rigidbody.isKinematic, 125f);

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Interpolate", GUILayout.Width(125f));
                if (GUILayout.Toggle(rigidbody.interpolation == RigidbodyInterpolation.None, "None"))
                    rigidbody.interpolation = RigidbodyInterpolation.None;
                if (GUILayout.Toggle(rigidbody.interpolation == RigidbodyInterpolation.Interpolate, "Interpolate"))
                    rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
                if (GUILayout.Toggle(rigidbody.interpolation == RigidbodyInterpolation.Extrapolate, "Extrapolate"))
                    rigidbody.interpolation = RigidbodyInterpolation.Extrapolate;
            }
            GUILayout.EndHorizontal();

            GUILayout.Label("Collision Detection");
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(20f);
                if (GUILayout.Toggle(rigidbody.collisionDetectionMode == CollisionDetectionMode.Discrete , "Discrete"))
                    rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
                if (GUILayout.Toggle(rigidbody.collisionDetectionMode == CollisionDetectionMode.Continuous, "Continuous"))
                    rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;

            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(20f);
                if (GUILayout.Toggle(rigidbody.collisionDetectionMode == CollisionDetectionMode.ContinuousDynamic, "Continuous Dynamic"))
                    rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                if (GUILayout.Toggle(rigidbody.collisionDetectionMode == CollisionDetectionMode.ContinuousSpeculative, "Continuous Speculative"))
                    rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            }
            GUILayout.EndHorizontal();

            GUILayout.Label("Constraints");
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(20f);
                if (GUILayout.Toggle(rigidbody.constraints == RigidbodyConstraints.FreezePositionX, "FreezePositionX"))
                    rigidbody.constraints = RigidbodyConstraints.FreezePositionX;
                if (GUILayout.Toggle(rigidbody.constraints == RigidbodyConstraints.FreezePositionY, "FreezePositionY"))
                    rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
                if (GUILayout.Toggle(rigidbody.constraints == RigidbodyConstraints.FreezePositionZ, "FreezePositionZ"))
                    rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(20f);
                if (GUILayout.Toggle(rigidbody.constraints == RigidbodyConstraints.FreezeRotationX, "FreezeRotationX"))
                    rigidbody.constraints = RigidbodyConstraints.FreezeRotationX;
                if (GUILayout.Toggle(rigidbody.constraints == RigidbodyConstraints.FreezeRotationY, "FreezeRotationY"))
                    rigidbody.constraints = RigidbodyConstraints.FreezeRotationY;
                if (GUILayout.Toggle(rigidbody.constraints == RigidbodyConstraints.FreezeRotationZ, "FreezeRotationZ"))
                    rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(20f);
                if (GUILayout.Toggle(rigidbody.constraints == RigidbodyConstraints.FreezePosition, "FreezePosition"))
                    rigidbody.constraints = RigidbodyConstraints.FreezePosition;
                if (GUILayout.Toggle(rigidbody.constraints == RigidbodyConstraints.FreezeRotation, "FreezeRotation"))
                    rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                if (GUILayout.Toggle(rigidbody.constraints == RigidbodyConstraints.FreezeAll, "FreezeAll"))
                    rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            }
            GUILayout.EndHorizontal();

            GUILayout.Label("Info");
            GUI.enabled = false;
            DrawText(20f, "Velocity", rigidbody.velocity.ToString());
            DrawText(20f, "Inertia Tensor", rigidbody.inertiaTensor.ToString());
            DrawText(20f, "Inertia Tensor Rotation", rigidbody.inertiaTensorRotation.ToString());
            DrawText(20f, "Local Center of Mass", rigidbody.centerOfMass.ToString());
            DrawText(20f, "World Center of Mass", rigidbody.worldCenterOfMass.ToString());
            DrawText(20f, "Is Sleep", rigidbody.IsSleeping().ToString());
            GUI.enabled = true;
        }
    }
}