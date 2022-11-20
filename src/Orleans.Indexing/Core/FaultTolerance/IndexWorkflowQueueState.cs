using Orleans.Runtime;
using System;

namespace Orleans.Indexing
{
    /// <summary>
    /// The persistent unit for storing the information for an <see cref="IndexWorkflowQueueGrainService"/>
    /// </summary>
    /// <remarks>This requires GrainState instead of using StateStorageBridge, due to having to set the ETag for upsert.</remarks>
    [Serializable]
    [GenerateSerializer]
    internal class IndexWorkflowQueueState : IGrainState<IndexWorkflowQueueEntry>
    {
        [Id(0)]
        public IndexWorkflowQueueEntry State { get; set; } = new();

        [Id(1)]
        public string ETag { get; set; } = string.Empty;

        [Id(2)]
        public bool RecordExists { get; set; }
    }

    /// <summary>
    /// All the information stored for a single <see cref="IndexWorkflowQueueGrainService"/>
    /// </summary>
    [Serializable]
    [GenerateSerializer]
    internal class IndexWorkflowQueueEntry
    {
        // Updates that must be propagated to indexes.
        [Id(0)]
        internal IndexWorkflowRecordNode? WorkflowRecordsHead;

        public IndexWorkflowQueueEntry() => this.WorkflowRecordsHead = null;
    }
}
