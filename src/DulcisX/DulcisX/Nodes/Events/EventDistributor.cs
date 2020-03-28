using DulcisX.Core.Enums;
using System;
using System.Collections.Generic;

namespace DulcisX.Nodes.Events
{
    /// <summary>
    /// Distributes native events between delegates given the <see cref="NodeTypes"/>. This is a wrapper around events.
    /// </summary>
    /// <typeparam name="TDelegate"></typeparam>
    public sealed class EventDistributor<TDelegate> where TDelegate : Delegate
    {
        private delegate void CallAction(dynamic action);

        private readonly Dictionary<int, Delegate> _cache;

        internal EventDistributor()
        {
            _cache = new Dictionary<int, Delegate>();
        }

        /// <summary>
        /// Hooks a callback method to the event.
        /// </summary>
        /// <param name="nodeTypes">The Node types on which to call the callback methods.</param>
        /// <param name="callback">The callback which should be called at the raise of the event.</param>
        public void Hook(NodeTypes nodeTypes, TDelegate callback)
        {
            DoByTypes(nodeTypes, i =>
            {
                if (!_cache.ContainsKey(i))
                {
                    _cache[i] = callback;
                }
                else
                {
                    _cache[i] = Delegate.Combine(_cache[i], callback);

                }
            });
        }

        /// <summary>
        /// Hooks callback methods to the event.
        /// </summary>
        /// <param name="nodeTypes">The Node types on which to call the callback methods.</param>
        /// <param name="callbacks">A list of callbacks which should be called at the raise of the event.</param>
        public void Hook(NodeTypes nodeTypes, params TDelegate[] callbacks)
        {
            DoByTypes(nodeTypes, i =>
            {
                if (!_cache.ContainsKey(i))
                {
                    _cache[i] = Delegate.Combine(callbacks);
                }
                else
                {
                    _cache[i] = Delegate.Combine(_cache[i], Delegate.Combine(callbacks));
                }
            });
        }

        /// <summary>
        /// UnHooks a callback method from the event.
        /// </summary>
        /// <param name="nodeTypes">The Node types on which to remove the callback method.</param>
        /// <param name="callback">The callback which should be removed from the event.</param>
        public void UnHook(NodeTypes nodeTypes, TDelegate callback)
        {
            DoByTypes(nodeTypes, i =>
            {
                if (_cache.ContainsKey(i))
                {
                    _cache[i] = Delegate.Remove(_cache[i], callback);
                }
            });
        }

        /// <summary>
        /// UnHooks callback methods from the event.
        /// </summary>
        /// <param name="nodeTypes">The Node types on which to remove the callback methods.</param>
        /// <param name="callbacks">The callbacks which should be removed from the event.</param>
        public void UnHook(NodeTypes nodeTypes, params TDelegate[] callbacks)
        {
            DoByTypes(nodeTypes, i =>
            {
                if (_cache.ContainsKey(i))
                {
                    var delegates = _cache[i];

                    for (int index = 0; index < callbacks.Length; index++)
                    {
                        delegates = Delegate.Remove(delegates, callbacks[i]);
                    }
                }
            });
        }

        /// <summary>
        /// UnHooks all callback methods, from all Node type, from the event.
        /// </summary>
        public void UnHookAll()
        {
            _cache.Clear();
        }

        /// <summary>
        /// UnHooks all callback methods from the event.
        /// </summary>
        /// <param name="nodeTypes">The Node types from which all callback methods should be removed.</param>
        public void UnHookAll(NodeTypes nodeTypes)
        {
            DoByTypes(nodeTypes, i =>
            {
                if (_cache.ContainsKey(i))
                {
                    _cache.Remove(i);
                }
            });
        }


        #region Invoke
        internal void Invoke(NodeTypes nodeTypes)
        {
            CallActions(nodeTypes, action => action());
        }

        internal void Invoke<T1>(NodeTypes nodeTypes, T1 t1)
        {
            CallActions(nodeTypes, action => action(t1));
        }

        internal void Invoke<T1, T2>(NodeTypes nodeTypes, T1 t1, T2 t2)
        {
            CallActions(nodeTypes, action => action(t1, t2));
        }

        internal void Invoke<T1, T2, T3>(NodeTypes nodeTypes, T1 t1, T2 t2, T3 t3)
        {
            CallActions(nodeTypes, action => action(t1, t2, t3));
        }

        internal void Invoke<T1, T2, T3, T4>(NodeTypes nodeTypes, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            CallActions(nodeTypes, action => action(t1, t2, t3, t4));
        }

        #endregion

        private void CallActions(NodeTypes nodeTypes, CallAction action)
        {
            DoByTypes(nodeTypes, i =>
            {
                if (_cache.ContainsKey(i))
                {
                    action.Invoke((dynamic)_cache[i]);
                }
            });
        }

        private void DoByTypes(NodeTypes nodeTypes, Action<int> action)
        {
            var flags = (int)nodeTypes;

            if ((flags & (flags - 1)) != 0)
            {
                for (int i = 1; i <= flags; i *= 2)
                {
                    if ((flags & i) != 0)
                    {
                        action.Invoke(i);
                    }
                }
            }
            else
            {
                action.Invoke(flags);
            }
        }
    }
}