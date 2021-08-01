/*
Copyright (c) 2020 Omar Duarte
Unauthorized copying of this file, via any medium is strictly prohibited.
Writen by Omar Duarte, 2020.

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
    [Serializable]
    public class SimulateGravityData
    {
        [SerializeField] private int _maxIterations = 1000;
        [SerializeField] private Vector3 _gravity = Physics.gravity;
        [SerializeField] private float _drag = 0f;
        [SerializeField] private float _angularDrag = 0.05f;
        [SerializeField] private float _maxSpeed = 100;
        private float _maxSpeedSquared = 10000;
        [SerializeField] private float _maxAngularSpeed = 10;
        private float _maxAngularSpeedSquared = 100;
        [SerializeField] private float _mass = 1f;
        [SerializeField] private bool _changeLayer = false;
        [SerializeField] private int _tempLayer = 0;

        public int maxIterations
        {
            get => _maxIterations;
            set
            {
                value = Mathf.Clamp(value, 1, 100000);
                if (_maxIterations == value) return;
                _maxIterations = value;
            }
        }
        public Vector3 gravity
        {
            get => _gravity;
            set
            {
                if (_gravity == value) return;
                _gravity = value;
            }
        }
        public float drag
        {
            get => _drag;
            set
            {
                value = Mathf.Max(value, 0f);
                if (_drag == value) return;
                _drag = value;
            }
        }
        public float angularDrag
        {
            get => _angularDrag;
            set
            {
                value = Mathf.Max(value, 0f);
                if (_angularDrag == value) return;
                _angularDrag = value;
            }
        }
        public float maxSpeed
        {
            get => _maxSpeed;
            set
            {
                value = Mathf.Max(value, 0f);
                if (_maxSpeed == value) return;
                _maxSpeed = value;
                _maxSpeedSquared = _maxSpeed * _maxSpeed;
            }
        }
        public float maxAngularSpeed
        {
            get => _maxAngularSpeed;
            set
            {
                value = Mathf.Max(value, 0f);
                if (_maxAngularSpeed == value) return;
                _maxAngularSpeed = value;
                _maxAngularSpeedSquared = _maxAngularSpeed * _maxAngularSpeed;
            }
        }
        public float maxSpeedSquared => _maxSpeedSquared;
        public float maxAngularSpeedSquared => _maxAngularSpeedSquared;
        public float mass
        {
            get => _mass;
            set
            {
                value = Mathf.Max(value, 1e-7f);
                if (_mass == value) return;
                _mass = value;
            }
        }
        public bool changeLayer
        {
            get => _changeLayer;
            set
            {
                if (_changeLayer == value) return;
                _changeLayer = value;
            }
        }
        public int tempLayer
        {
            get => _tempLayer;
            set
            {
                if (_tempLayer == value) return;
                _tempLayer = value;
            }
        }

        public void Copy(SimulateGravityData other)
        {
            _maxIterations = other._maxIterations;
            _gravity = other._gravity;
            _drag = other._drag;
            _angularDrag = other._angularDrag;
            _maxSpeed = other._maxSpeed;
            _maxSpeedSquared = other._maxSpeedSquared;
            _maxAngularSpeed = other._maxAngularSpeed;
            _maxAngularSpeedSquared = other._maxAngularSpeedSquared;
            _mass = other._mass;
            _changeLayer = other._changeLayer;
            _tempLayer = other._tempLayer;
        }
    }

    public static class GravityUtils
    {
        public static void SimulateGravity(GameObject[] selection, SimulateGravityData data, bool recordAction)
        {
            var originalGravity = Physics.gravity;
            Physics.gravity = data.gravity;
            var allBodies = GameObject.FindObjectsOfType<Rigidbody>();
            var originalBodies = new List<(Rigidbody body, bool useGravity, bool isKinematic,
                float drag, float angularDrag, float mass)>();
            foreach (var rigidBody in allBodies)
            {
                originalBodies.Add((rigidBody, rigidBody.useGravity, rigidBody.isKinematic,
                    rigidBody.drag, rigidBody.angularDrag, rigidBody.mass));
                rigidBody.useGravity = false;
                rigidBody.isKinematic = true;
                rigidBody.drag = data.drag;
                rigidBody.angularDrag = data.angularDrag;
                rigidBody.mass = data.mass;
            }

            var enabledCollidersSelected = new List<Collider>();
            var allMeshesSelected = new List<MeshFilter>();
            var tempBodies = new List<Rigidbody>();
            var simBodies = new List<Rigidbody>();
            var originalLayers = new List<int>();
            foreach (var obj in selection)
            {
                if (recordAction) UnityEditor.Undo.RecordObject(obj.transform, "Simulate Gravity");
                var enabledColliders = obj.GetComponentsInChildren<Collider>().Where(c => c.enabled);
                enabledCollidersSelected.AddRange(enabledColliders);
                var meshes = obj.GetComponentsInChildren<MeshFilter>();
                allMeshesSelected.AddRange(meshes);
                if (meshes.Count() == 0) continue;
                var rigidBody = obj.GetComponent<Rigidbody>();
                if (rigidBody == null)
                {
                    rigidBody = obj.AddComponent<Rigidbody>();
                    tempBodies.Add(rigidBody);
                }
                if (rigidBody == null) continue;
                if (data.changeLayer)
                {
                    originalLayers.Add(obj.layer);
                    obj.layer = data.tempLayer;
                }
                simBodies.Add(rigidBody);
                rigidBody.useGravity = true;
                rigidBody.isKinematic = false;
            }

            foreach (var collider in enabledCollidersSelected) collider.enabled = false;

            var tempColliders = new List<Collider>();


            foreach (var mesh in allMeshesSelected)
            {
                Collider collider = null;
                if (mesh.sharedMesh.name == "Sphere") collider = mesh.gameObject.AddComponent<SphereCollider>();
                else if (mesh.sharedMesh.name == "Capsule") collider = mesh.gameObject.AddComponent<CapsuleCollider>();
                else if (mesh.sharedMesh.name == "Cube") collider = mesh.gameObject.AddComponent<BoxCollider>();
                else
                {
                    collider = mesh.gameObject.AddComponent<MeshCollider>();
                    (collider as MeshCollider).convex = true;
                }
                tempColliders.Add(collider);
            }

            Physics.autoSimulation = false;
            for (int i = 0; i < data.maxIterations; ++i)
            {
                Physics.Simulate(Time.fixedDeltaTime);
                foreach (var body in simBodies)
                {
                    if (body.velocity.sqrMagnitude > data.maxSpeedSquared)
                        body.velocity = body.velocity.normalized * data.maxSpeed;
                    if (body.angularVelocity.sqrMagnitude > data.maxAngularSpeedSquared)
                        body.angularVelocity = body.angularVelocity.normalized * data.maxAngularSpeed;
                }
                if (simBodies.All(rb => rb.IsSleeping())) break;
            }
            Physics.autoSimulation = true;
            if (data.changeLayer)
            {
                for (int i = 0; i < simBodies.Count; ++i) simBodies[i].gameObject.layer = originalLayers[i];
            }
            foreach (var body in tempBodies) UnityEngine.Object.DestroyImmediate(body);
            foreach (var collider in tempColliders) UnityEngine.Object.DestroyImmediate(collider);
            foreach (var collider in enabledCollidersSelected) collider.enabled = true;

            foreach (var item in originalBodies)
            {
                item.body.useGravity = item.useGravity;
                item.body.isKinematic = item.isKinematic;
                item.body.drag = item.drag;
                item.body.angularDrag = item.angularDrag;
                item.body.mass = item.mass;
            }

            Physics.gravity = originalGravity;
        }
    }
}