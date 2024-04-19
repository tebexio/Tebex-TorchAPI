using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TebexSpaceEngineersPlugin {
    public static class PatchController {
        public static object PluginInstance;

        public class TargetMethod : Attribute {
            public Type Type { get; set; }
            public string Method { get; set; }
        }

        public class PrefixMethod : Attribute {

        }

        public class PostFixMethod : Attribute {

        }

        public class PatchingClass : Attribute {

        }

        public static void PatchMethods(object Plugin) {
            VRage.Utils.MyLog.Default.WriteLineAndConsole("Patching methods...");
            PluginInstance = Plugin;
            var assembly = Assembly.GetExecutingAssembly();

            foreach (var PatchingClass in GetPatchingClassesAndInitalize(assembly)) {

                foreach (var method in PatchingClass.GetMethods().Where(x => x.GetCustomAttributes(typeof(PrefixMethod), false).FirstOrDefault() != null)) {
                    Patch(method, typeof(PrefixMethod));
                }

                foreach (var method in PatchingClass.GetMethods().Where(x => x.GetCustomAttributes(typeof(PostFixMethod), false).FirstOrDefault() != null)) {
                    Patch(method, typeof(PostFixMethod));
                }
            }
        }

        public static void Patch(MethodInfo newMethod, Type typeOfPatch) {
            /*
            var harmony = new Harmony("PatchController");
            TargetMethod TargetMethodData = (TargetMethod)newMethod.GetCustomAttribute(typeof(TargetMethod));
            VRage.Utils.MyLog.Default.WriteLineAndConsole($"Patching {TargetMethodData.Method} with {newMethod.Name} (Prefix)");

            if (typeOfPatch == typeof(PrefixMethod)) {
                harmony.Patch(TargetMethodData.Type.GetMethod(TargetMethodData.Method, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public), new HarmonyMethod(newMethod));

            }
            else {
                harmony.Patch(TargetMethodData.Type.GetMethod(TargetMethodData.Method, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public), null, new HarmonyMethod(newMethod));
            }*/
        }

        public static IEnumerable<Type> GetPatchingClassesAndInitalize(Assembly assembly) {
            foreach (Type type in assembly.GetTypes()) {
                if (type.GetCustomAttributes(typeof(PatchingClass), true).Length > 0) {

                    Activator.CreateInstance(type);
                    yield return type;
                }
            }
        }
    }
}
