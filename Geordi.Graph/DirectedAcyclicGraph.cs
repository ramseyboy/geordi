using System.Collections;

namespace Geordi.Graph
{
    /// <summary>
    /// Initial implementation based off https://github.com/ociaw/dagger
    /// </summary>
    public class DirectedAcyclicGraph<TKey, TData> : IGraph<TKey, TData> where TKey : notnull
    {
        private Dictionary<TKey, TData> Data { get; } = new();

        private Dictionary<TKey, HashSet<TKey>> IncomingEdges { get; } = new();

        private Dictionary<TKey, HashSet<TKey>> OutgoingEdges { get; } = new();

        /// <inheritdoc />
        public void AddNode(TKey key, TData data, IList<TKey> outgoing)
        {
            if (OutgoingEdges.ContainsKey(key))
            {
                throw new ArgumentException("Node with the provided key already exists.");
            }

            if (CausesCycle(key, outgoing))
            {
                throw new ArgumentException("Adding this node causes a cycle.");
            }

            Data.Add(key, data);
            AddEdges(key, outgoing);
        }

        /// <inheritdoc />
        public void RemoveNode(TKey key)
        {
            if (!OutgoingEdges.TryGetValue(key, out var value))
            {
                throw new ArgumentException("Node does not exist.");
            }

            Data.Remove(key);
            foreach (var edge in value)
            {
                IncomingEdges[edge].Remove(key);
            }
            OutgoingEdges.Remove(key);
        }

        /// <inheritdoc />
        public List<TKey> GetIncoming(TKey key)
        {
            return !IncomingEdges.TryGetValue(key, out var edges) ? [] : edges.ToList();
        }

        /// <inheritdoc />
        public List<TKey> GetOutgoing(TKey key)
        {
            return !OutgoingEdges.TryGetValue(key, out var edges) ? new List<TKey>() : edges.ToList();
        }

        /// <inheritdoc />
        public (List<List<TKey>> layers, List<TKey> detached) TopologicalSort()
        {
            var layers = new List<List<TKey>> { new List<TKey>() };
            var detached = new List<TKey>();
            foreach (var kvp in OutgoingEdges)
            {
                var key = kvp.Key;
                var destinations = OutgoingEdges[key];
                if (OutgoingEdges[key].Count == 0)
                {
                    layers[0].Add(key); // If a key has no outgoing edges, it's added to the first layer
                }
                else if (destinations.Any(dest => !OutgoingEdges.ContainsKey(dest)))
                {
                    detached.Add(key); // If a key has any outgoing edges that are not in the graph, it is considered detached.
                }
            }

            var satisfiedKeys = new HashSet<TKey>(layers[0]);
            var unsatisfiedKeys = new HashSet<TKey>();

            while (layers[^1].Count > 0)
            {
                var candidates =
                    layers[^1]
                    .SelectMany(previous => IncomingEdges.TryGetValue(previous, out var value) ? value : [])
                    .Where(key => OutgoingEdges.ContainsKey(key))
                    .Concat(unsatisfiedKeys)
                    .Distinct();

                unsatisfiedKeys.Clear();

                var currentLevel = new List<TKey>();
                foreach (var candidate in candidates)
                {
                    var satisfied = OutgoingEdges[candidate].All(outgoing => satisfiedKeys.Contains(outgoing));

                    if (!satisfied)
                    {
                        unsatisfiedKeys.Add(candidate);
                        continue;
                    }

                    currentLevel.Add(candidate);
                }

                layers.Add(currentLevel);
                foreach (var key in currentLevel)
                {
                    satisfiedKeys.Add(key);
                }
            }

            layers.RemoveAt(layers.Count - 1);
            detached.AddRange(unsatisfiedKeys);
            return (layers, detached);
        }

        private bool CausesCycle(TKey key, IList<TKey> outgoing)
        {
            if (outgoing.Contains(key))
            {
                return true; // Self cycle
            }

            if (!IncomingEdges.TryGetValue(key, out var incoming) || incoming.Count == 0)
            {
                return false; // No incoming edges, so we can't have a cycle.
            }

            // If a path exists from any outgoing edge to any incoming edge, adding the node will cause a cycle.
            return outgoing
                .Any(start => incoming
                    .Any(end => PathExists(start, end)));
        }

        private bool PathExists(TKey start, TKey end)
        {
            var tested = new HashSet<TKey> { start };
            var queued = new Queue<TKey>(tested);

            while (queued.Count > 0)
            {
                var current = queued.Dequeue();
                if (current != null && current.Equals(end)) // A path exists
                {
                    return true;
                }


                if (!OutgoingEdges.TryGetValue(current, out var destinations))
                {
                    continue; // No out edges for current
                }

                foreach (var destination in destinations.Where(destination => !tested.Contains(destination)))
                {
                    tested.Add(destination);
                    queued.Enqueue(destination);
                }
            }

            return false;
        }

        private void AddEdges(TKey key, IList<TKey> outgoing)
        {
            OutgoingEdges.Add(key, new HashSet<TKey>(outgoing));
            foreach (var dest in outgoing)
            {
                if (!IncomingEdges.TryGetValue(dest, out var incoming))
                {
                    IncomingEdges[dest] = [key];
                }
                else
                {
                    incoming.Add(key);
                }
            }
        }

        public IEnumerator<KeyValuePair<TKey, TData>> GetEnumerator() => Data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
