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
using UnityEditor;
using UnityEngine;

namespace PluginMaster
{
    [InitializeOnLoad]
    public static class SelectionManager
    {
        private static GameObject[] _topLevelSelection = new GameObject[0];
        private static GameObject[] _selection = new GameObject[0];
        public static Action selectionChanged;
        static SelectionManager() => Selection.selectionChanged += UpdateSelection;
        private static void UpdateSelection(List<GameObject> list, bool _filteredByTopLevel)
        {
            var newSet = new HashSet<GameObject>(Selection.GetFiltered<GameObject>(SelectionMode.Editable | SelectionMode.ExcludePrefab | (_filteredByTopLevel ? SelectionMode.TopLevel : SelectionMode.Unfiltered)));
            if (newSet.Count == 0)
            {
                list.Clear();
                return;
            }
            var unselectedSet = new HashSet<GameObject>(list);
            unselectedSet.ExceptWith(newSet);
            foreach (var obj in unselectedSet) list.Remove(obj);
            newSet.ExceptWith(list);
            foreach (var obj in newSet) list.Add(obj);
        }
        public static void UpdateSelection()
        {
            var selectionOrderedTopLevel = new List<GameObject>();
            var selectionOrdered = new List<GameObject>();
            UpdateSelection(selectionOrderedTopLevel, true);
            UpdateSelection(selectionOrdered, false);
            _selection = selectionOrdered.ToArray();
            _topLevelSelection = selectionOrderedTopLevel.ToArray();
            if(selectionChanged != null) selectionChanged();
        }
        public static GameObject[] GetSelection(bool filteredByTopLevel) => filteredByTopLevel ? _topLevelSelection : _selection;
        public static GameObject[] topLevelSelection => _topLevelSelection;
        public static GameObject[] selection => _selection;

        public static GameObject[] GetSelectionPrefabs()
        {
            var result = new List<GameObject>();
            foreach (var obj in topLevelSelection)
            {
                if (obj == null) continue;
                if (PrefabUtility.GetPrefabInstanceStatus(obj) == PrefabInstanceStatus.NotAPrefab) continue;
                if (!PrefabUtility.IsAnyPrefabInstanceRoot(obj)) continue;
                var prefab = PrefabUtility.GetCorrespondingObjectFromSource(obj);
                result.Add(prefab);
            }
            return result.ToArray();
        }
    }
}
