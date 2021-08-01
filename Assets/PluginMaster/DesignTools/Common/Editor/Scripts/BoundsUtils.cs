/*
Copyright (c) 2021 Omar Duarte
Unauthorized copying of this file, via any medium is strictly prohibited.
Writen by Omar Duarte, 2021.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PluginMaster
{
    public static class BoundsUtils
    {
        private static readonly Vector3 MIN_VECTOR3 = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        private static readonly Vector3 MAX_VECTOR3 = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

        public enum ObjectProperty
        {
            BOUNDING_BOX,
            CENTER,
            PIVOT
        }

        public static Bounds GetBounds(Transform transform, ObjectProperty property = ObjectProperty.BOUNDING_BOX)
        {
            var renderer = transform.GetComponent<Renderer>();
            var rectTransform = transform.GetComponent<RectTransform>();

            if (rectTransform == null)
            {
                if (renderer == null || property == ObjectProperty.PIVOT) return new Bounds(transform.position, Vector3.zero);
                if (property == ObjectProperty.CENTER) return new Bounds(renderer.bounds.center, Vector3.zero);
                return renderer.bounds;
            }
            else
            {
                if (property == ObjectProperty.PIVOT) return new Bounds(rectTransform.position, Vector3.zero);
                return new Bounds(rectTransform.TransformPoint(rectTransform.rect.center), rectTransform.TransformVector(rectTransform.rect.size));
            }
        }

        public static Bounds GetBoundsRecursive(Transform transform, bool recursive = true, ObjectProperty property = ObjectProperty.BOUNDING_BOX)
        {
            if (!recursive) return GetBounds(transform, property);
            var children = transform.GetComponentsInChildren<Transform>(true);
            var min = MAX_VECTOR3;
            var max = MIN_VECTOR3;
            var emptyHierarchy = true;
            foreach (var child in children)
            {
                if (child.GetComponent<Renderer>() == null && child.GetComponent<RectTransform>() == null) continue;
                emptyHierarchy = false;
                var bounds = GetBounds(child, property);
                min = Vector3.Min(bounds.min, min);
                max = Vector3.Max(bounds.max, max);
            }
            if (emptyHierarchy) return new Bounds(transform.position, Vector3.zero);
            var size = max - min;
            var center = min + size / 2f;
            return new Bounds(center, size);
        }

        public static Bounds GetSelectionBounds(GameObject[] selection, bool recursive = true, BoundsUtils.ObjectProperty property = BoundsUtils.ObjectProperty.BOUNDING_BOX)
        {
            var max = MIN_VECTOR3;
            var min = MAX_VECTOR3;
            foreach (var obj in selection)
            {
                if (obj == null) continue;
                var bounds = GetBoundsRecursive(obj.transform, recursive, property);
                max = Vector3.Max(bounds.max, max);
                min = Vector3.Min(bounds.min, min);
            }
            var size = max - min;
            var center = min + size / 2f;
            return new Bounds(center, size);
        }

        public static Bounds GetBounds(Transform transform, Quaternion rotation)
        {
            var rectTransform = transform.GetComponent<RectTransform>();
            if (rectTransform != null) return new Bounds(rectTransform.TransformPoint(rectTransform.rect.center), rectTransform.TransformVector(rectTransform.rect.size));
            var renderer = transform.GetComponent<Renderer>();
            var meshFilter = transform.GetComponent<MeshFilter>();
            if (renderer == null || meshFilter == null || meshFilter.sharedMesh == null) return new Bounds(transform.position, Vector3.zero);
            var maxSqrDistance = MIN_VECTOR3;
            var minSqrDistance = MAX_VECTOR3;
            var center = GetBounds(transform).center;
            var right = rotation * Vector3.right;
            var up = rotation * Vector3.up;
            var forward = rotation * Vector3.forward;
            foreach (var vertex in meshFilter.sharedMesh.vertices)
            {
                var centerToVertex = transform.TransformPoint(vertex) - center;
                var rightProjection = Vector3.Project(centerToVertex, right);
                var upProjection = Vector3.Project(centerToVertex, up);
                var forwardProjection = Vector3.Project(centerToVertex, forward);
                var rightSqrDistance = rightProjection.sqrMagnitude * (rightProjection.normalized != right ? -1 : 1);
                var upSqrDistance = upProjection.sqrMagnitude * (upProjection.normalized != up ? -1 : 1);
                var forwardSqrDistance = forwardProjection.sqrMagnitude * (forwardProjection.normalized != forward ? -1 : 1);
                maxSqrDistance.x = Mathf.Max(maxSqrDistance.x, rightSqrDistance);
                maxSqrDistance.y = Mathf.Max(maxSqrDistance.y, upSqrDistance);
                maxSqrDistance.z = Mathf.Max(maxSqrDistance.z, forwardSqrDistance);
                minSqrDistance.x = Mathf.Min(minSqrDistance.x, rightSqrDistance);
                minSqrDistance.y = Mathf.Min(minSqrDistance.y, upSqrDistance);
                minSqrDistance.z = Mathf.Min(minSqrDistance.z, forwardSqrDistance);
            }
            var size = new Vector3(
                Mathf.Sqrt(Mathf.Abs(maxSqrDistance.x)) * Mathf.Sign(maxSqrDistance.x) - Mathf.Sqrt(Mathf.Abs(minSqrDistance.x)) * Mathf.Sign(minSqrDistance.x),
                Mathf.Sqrt(Mathf.Abs(maxSqrDistance.y)) * Mathf.Sign(maxSqrDistance.y) - Mathf.Sqrt(Mathf.Abs(minSqrDistance.y)) * Mathf.Sign(minSqrDistance.y),
                Mathf.Sqrt(Mathf.Abs(maxSqrDistance.z)) * Mathf.Sign(maxSqrDistance.z) - Mathf.Sqrt(Mathf.Abs(minSqrDistance.z)) * Mathf.Sign(minSqrDistance.z));
            return new Bounds(center, size);
        }

        private static void GetDistanceFromCenter(Transform transform, Quaternion rotation, Vector3 center, out Vector3 min, out Vector3 max)
        {
            min = max = Vector3.zero;
            var vertices = new List<Vector3>();
            var rectTransform = transform.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                vertices.Add(rectTransform.rect.min);
                vertices.Add(rectTransform.rect.max);
                vertices.Add(new Vector2(rectTransform.rect.min.x, rectTransform.rect.max.y));
                vertices.Add(new Vector2(rectTransform.rect.max.x, rectTransform.rect.min.y));
            }
            else
            {
                var renderer = transform.GetComponent<Renderer>();
                if (renderer == null) return;
                if (renderer is SpriteRenderer)
                {
                    var sprite = (renderer as SpriteRenderer).sprite;
                    if (sprite == null) return;
                    var spriteSize = sprite.rect.size / sprite.pixelsPerUnit;
                    vertices.Add(Vector3.Scale(spriteSize, new Vector3(-0.5f, -0.5f, 0f)));
                    vertices.Add(Vector3.Scale(spriteSize, new Vector3(-0.5f, 0.5f, 0f)));
                    vertices.Add(Vector3.Scale(spriteSize, new Vector3(0.5f, -0.5f, 0f)));
                    vertices.Add(Vector3.Scale(spriteSize, new Vector3(0.5f, 0.5f, 0f)));
                }
                else if (renderer is MeshRenderer)
                {
                    var meshFilter = transform.GetComponent<MeshFilter>();
                    if (meshFilter == null || meshFilter.sharedMesh == null) return;
                    vertices.AddRange(meshFilter.sharedMesh.vertices);
                }
                else if (renderer is SkinnedMeshRenderer)
                {
                    var mesh = (renderer as SkinnedMeshRenderer).sharedMesh;
                    if (mesh == null) return;
                    vertices.AddRange(mesh.vertices);
                }
            }
            var maxSqrDistance = MIN_VECTOR3;
            var minSqrDistance = MAX_VECTOR3;
            var right = rotation * Vector3.right;
            var up = rotation * Vector3.up;
            var forward = rotation * Vector3.forward;

            foreach (var vertex in vertices)
            {
                var centerToVertex = transform.TransformPoint(vertex) - center;
                var rightProjection = Vector3.Project(centerToVertex, right);
                var upProjection = Vector3.Project(centerToVertex, up);
                var forwardProjection = Vector3.Project(centerToVertex, forward);
                var rightSqrDistance = rightProjection.sqrMagnitude * (rightProjection.normalized != right ? -1 : 1);
                var upSqrDistance = upProjection.sqrMagnitude * (upProjection.normalized != up ? -1 : 1);
                var forwardSqrDistance = forwardProjection.sqrMagnitude * (forwardProjection.normalized != forward ? -1 : 1);

                maxSqrDistance.x = Mathf.Max(maxSqrDistance.x, rightSqrDistance);
                maxSqrDistance.y = Mathf.Max(maxSqrDistance.y, upSqrDistance);
                maxSqrDistance.z = Mathf.Max(maxSqrDistance.z, forwardSqrDistance);

                minSqrDistance.x = Mathf.Min(minSqrDistance.x, rightSqrDistance);
                minSqrDistance.y = Mathf.Min(minSqrDistance.y, upSqrDistance);
                minSqrDistance.z = Mathf.Min(minSqrDistance.z, forwardSqrDistance);
            }

            min = new Vector3(
                Mathf.Sqrt(Mathf.Abs(minSqrDistance.x)) * Mathf.Sign(minSqrDistance.x),
                Mathf.Sqrt(Mathf.Abs(minSqrDistance.y)) * Mathf.Sign(minSqrDistance.y),
                Mathf.Sqrt(Mathf.Abs(minSqrDistance.z)) * Mathf.Sign(minSqrDistance.z));
            max = new Vector3(
               Mathf.Sqrt(Mathf.Abs(maxSqrDistance.x)) * Mathf.Sign(maxSqrDistance.x),
               Mathf.Sqrt(Mathf.Abs(maxSqrDistance.y)) * Mathf.Sign(maxSqrDistance.y),
               Mathf.Sqrt(Mathf.Abs(maxSqrDistance.z)) * Mathf.Sign(maxSqrDistance.z));
        }


        private static void GetDistanceFromCenterRecursive(Transform transform, Quaternion rotation, Vector3 center, out Vector3 minDistance, out Vector3 maxDistance)
        {
            var children = transform.GetComponentsInChildren<Transform>(true);
            var emptyHierarchy = true;
            maxDistance = MIN_VECTOR3;
            minDistance = MAX_VECTOR3;
            foreach (var child in children)
            {
                if (child.GetComponent<Renderer>() == null && child.GetComponent<RectTransform>() == null) continue;
                emptyHierarchy = false;

                Vector3 min, max;
                GetDistanceFromCenter(child, rotation, center, out min, out max);
                minDistance = Vector3.Min(min, minDistance);
                maxDistance = Vector3.Max(max, maxDistance);
            }
            if (emptyHierarchy) minDistance = maxDistance = Vector3.zero;
        }

        public static Bounds GetBoundsRecursive(Transform transform, Quaternion rotation)
        {
            var center = GetBoundsRecursive(transform).center;
            Vector3 maxDistance, minDistance;
            GetDistanceFromCenterRecursive(transform, rotation, center, out minDistance, out maxDistance);
            var size = maxDistance - minDistance;
            center += rotation * (minDistance + size / 2);
            return new Bounds(center, size);
        }

        public static Bounds GetSelectionBounds(GameObject[] selection, Quaternion rotation)
        {
            var max = MIN_VECTOR3;
            var min = MAX_VECTOR3;
            var center = GetSelectionBounds(selection).center;
            foreach (var obj in selection)
            {
                if (obj == null) continue;
                Vector3 minDistance, maxDistance;
                GetDistanceFromCenterRecursive(obj.transform, rotation, center, out minDistance, out maxDistance);
                max = Vector3.Max(maxDistance, max);
                min = Vector3.Min(minDistance, min);
            }
            var size = max - min;
            center += rotation * (min + size / 2);
            return new Bounds(center, size);
        }

        public static Vector3[] GetVertices(Transform transform)
        {
            var vertices = new List<Vector3>();
            var meshFilters = transform.GetComponentsInChildren<MeshFilter>();
            foreach (var filter in meshFilters)
            {
                if (filter.sharedMesh == null) continue;
                vertices.AddRange(filter.sharedMesh.vertices);
            }
            return vertices.ToArray();
        }

        public static Vector3[] GetBottomVertices(Transform transform)
        {
            var vertices = new List<Vector3>();
            var meshFilters = transform.GetComponentsInChildren<MeshFilter>();
            var bounds = GetBoundsRecursive(transform, transform.rotation);
            var localMinY = transform.InverseTransformPoint(bounds.min).y;
            var threshold = localMinY + bounds.size.y * 0.01f;
            foreach (var filter in meshFilters)
            {
                if (filter.sharedMesh == null) continue;
                foreach (var vertex in filter.sharedMesh.vertices)
                {
                    var worldVertex = filter.transform.TransformPoint(vertex);
                    var localVertex = transform.InverseTransformPoint(worldVertex);
                    if (localVertex.y < threshold) vertices.Add(localVertex);
                }
            }
            return vertices.ToArray();
        }

        public static float GetBottomDistanceToSurface(Vector3[] bottomVertices, Matrix4x4 TRS, float maxDistance)
        {
            float distance = 0f;
            var down = TRS.rotation * Vector3.down;
            foreach (var vertex in bottomVertices)
            {
                var origin = TRS.MultiplyPoint(vertex);
                if (Physics.Raycast(origin, down, out RaycastHit hitInfo, maxDistance))
                    distance = Mathf.Max(hitInfo.distance, distance);
            }
            return distance;
        }
    }
}