namespace Geordi.Graph;

public interface IGraph<TKey, TData> : IEnumerable<KeyValuePair<TKey, TData>>
{
    /// <summary>
    /// Adds a node with the specified key, data, and outgoing edges to the graph.
    /// </summary>
    public void AddNode(TKey key, TData data, IList<TKey> outgoing);

    /// <summary>
    /// Removes a node and its outgoing edges.
    /// </summary>
    /// <param name="key"></param>
    public void RemoveNode(TKey key);

    /// <summary>
    /// Gets the incoming edges to a key, regardless if the node has been added or not.
    /// </summary>
    public List<TKey> GetIncoming(TKey key);

    /// <summary>
    /// Gets the incoming eges to a key, regardless if the node has been added or not.
    /// </summary>
    public List<TKey> GetOutgoing(TKey key);

    /// <summary>
    /// Returns the nodes topologically sorted into layers. Nodes with no outgoing edges are in the first layer,
    /// while nodes that only point to nodes in the first layer are in the second layer, and so on. Any node that
    /// points to a node that has not been added to the graph is considered detached.
    /// </summary>
    /// <returns>A tuple containing a list of layers and a list of detached keys.</returns>
    public (List<List<TKey>> layers, List<TKey> detached) TopologicalSort();
}
