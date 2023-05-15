using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace Temporalio.Activities
{
    /// <summary>
    /// Definition of an activity.
    /// </summary>
    public class ActivityDefinition
    {
        private readonly Func<object?[], object?> invoker;

        private ActivityDefinition(
            string name,
            Type returnType,
            IReadOnlyCollection<Type> parameterTypes,
            int requiredParameterCount,
            Func<object?[], object?> invoker)
        {
            Name = name;
            ReturnType = returnType;
            ParameterTypes = parameterTypes;
            RequiredParameterCount = requiredParameterCount;
            this.invoker = invoker;
        }

        /// <summary>
        /// Gets the activity name.
        /// </summary>
        public string Name { get; private init; }

        public Type ReturnType { get; private init; }

        public IReadOnlyCollection<Type> ParameterTypes { get; private init; }

        public int RequiredParameterCount { get; private init; }

        // TODO(cretz): Document that this awaits if return is a task. Document that
        // for no-result Task, Task<ValueTuple> is returned
        public async Task<object?> InvokeAsync(object?[] parameters)
        {
            // Have to unwrap and re-throw target invocation exception if present
            object? result;
            try
            {
                result = invoker.Invoke(parameters);
            }
            catch (TargetInvocationException e)
            {
                ExceptionDispatchInfo.Capture(e.InnerException!).Throw();
                // Unreachable
                throw new InvalidOperationException("Unreachable");
            }
            // If the result is a task, we need to await on it and use that result
            if (result is Task resultTask)
            {
                await resultTask.ConfigureAwait(false);
                // We have to use reflection to extract value if it's a Task<>
                var resultTaskType = resultTask.GetType();
                if (resultTaskType.IsGenericType)
                {
                    result = resultTaskType.GetProperty("Result")!.GetValue(resultTask);
                }
                else
                {
                    result = ValueTuple.Create();
                }
            }
            return result;
        }

        public static ActivityDefinition Create(Delegate del)
        {
            if (del.Method == null)
            {
                throw new ArgumentException("Activities must have accessible methods");
            }
            var attr = del.Method.GetCustomAttribute<ActivityAttribute>(false) ??
                throw new ArgumentException($"{del.Method} missing Activity attribute");
            var parms = del.Method.GetParameters();
            return Create(
                NameFromAttributed(del.Method, attr),
                del.Method.ReturnType,
                parms.Select(p => p.ParameterType).ToArray(),
                parms.Count(p => !p.HasDefaultValue),
                parameters => del.DynamicInvoke(ParametersWithDefaults(parms, parameters)));
        }

        public static ActivityDefinition Create(
            string name,
            Type returnType,
            IReadOnlyCollection<Type> parameterTypes,
            int requiredParameterCount,
            Func<object?[], object?> invoker)
        {
            if (requiredParameterCount > parameterTypes.Count)
            {
                throw new ArgumentException(
                    "Cannot have more required parameters than parameters",
                    nameof(requiredParameterCount));
            }
            return new(name, returnType, parameterTypes, requiredParameterCount, invoker);
        }

        public static IReadOnlyCollection<ActivityDefinition> CreateAll(Type type, object? instance)
        {
            var ret = new List<ActivityDefinition>();
            foreach (var method in type.GetMethods(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
            {
                var attr = method.GetCustomAttribute<ActivityAttribute>(true);
                if (attr == null)
                {
                    continue;
                }
                if (!method.IsStatic && instance == null)
                {
                    throw new InvalidOperationException(
                        $"Instance not provided, but activity method {method} is non-static");
                }
                var parms = method.GetParameters();
                ret.Add(Create(
                    NameFromAttributed(method, attr),
                    method.ReturnType,
                    parms.Select(p => p.ParameterType).ToArray(),
                    parms.Count(p => !p.HasDefaultValue),
                    parameters => method.Invoke(instance, ParametersWithDefaults(parms, parameters))));
            }
            if (ret.Count == 0)
            {
                throw new ArgumentException($"No activities found on {type}", nameof(type));
            }
            return ret;
        }

        private static object?[] ParametersWithDefaults(
            ParameterInfo[] paramInfos, object?[] parameters)
        {
            if (parameters.Length >= paramInfos.Length)
            {
                return parameters;
            }
            var ret = new List<object?>(parameters.Length);
            ret.AddRange(parameters);
            for (var i = parameters.Length; i < paramInfos.Length; i++)
            {
                ret.Add(paramInfos[i].DefaultValue);
            }
            return ret.ToArray();
        }

        private static string NameFromAttributed(MethodInfo method, ActivityAttribute attr)
        {
            var name = attr.Name;
            if (name != null)
            {
                return name;
            }
            // Build name from method name
            name = method.Name;
            // Local functions are in the form <parent>g__name|other, so we will try to
            // extract the name
            var localBegin = name.IndexOf(">g__");
            if (localBegin > 0)
            {
                name = name.Substring(localBegin + 4);
                var localEnd = name.IndexOf('|');
                if (localEnd == -1)
                {
                    throw new ArgumentException($"Cannot parse name from local function {method}");
                }
                name = name.Substring(0, localEnd);
            }
            // Lambdas will have >b__ on them, but we just check for the angle bracket to
            // disallow any similar form including local functions we missed
            if (name.Contains("<"))
            {
                throw new ArgumentException(
                    $"{method} appears to be a lambda which must have a name given on the attribute");
            }
            if (typeof(Task).IsAssignableFrom(method.ReturnType) &&
                name.Length > 5 && name.EndsWith("Async"))
            {
                name = name.Substring(0, name.Length - 5);
            }
            return name;
        }
    }
}