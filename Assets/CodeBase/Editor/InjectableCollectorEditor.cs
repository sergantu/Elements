using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CodeBase.Infrastructure;
using VContainer;
using VContainer.Unity;

[CustomEditor(typeof(GameBootstrapper))]
public class InjectableCollectorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Collect Injectables")) {
            CollectInjectables();
        }
    }

    private void CollectInjectables()
    {
        MonoBehaviour[] allMonoBehaviours = FindObjectsOfType<MonoBehaviour>(true);
        var injectableComponents = (from mb in allMonoBehaviours
                                    let fields = mb.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                    where fields.Any(field => field.GetCustomAttribute<InjectAttribute>() != null)
                                    select mb).ToList();

        var bootstrapper = (GameBootstrapper) target;

        Debug.Log($"Found {injectableComponents.Count} MonoBehaviour(s) with [Inject]:");
        foreach (MonoBehaviour injectable in injectableComponents) {
            Debug.Log(injectable.GetType().Name);

            var lifetimeScope = bootstrapper.GetComponent<LifetimeScope>();
            if (lifetimeScope == null) {
                continue;
            }
            FieldInfo autoInjectField = typeof(LifetimeScope).GetField("autoInjectGameObjects", BindingFlags.NonPublic | BindingFlags.Instance);
            if (autoInjectField == null) {
                continue;
            }
            var autoInjectList = (List<GameObject>) autoInjectField.GetValue(lifetimeScope);

            if (!autoInjectList.Contains(injectable.gameObject)) {
                autoInjectList.Add(injectable.gameObject);
            }
        }

        EditorUtility.SetDirty(bootstrapper);
    }
}