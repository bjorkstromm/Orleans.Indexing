using Orleans.Streams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orleans.Indexing
{
    /// <summary>
    /// This class represents the result of a query.
    ///
    /// OrleansQueryResultStream is actually a stream of results that can be observed by its client.
    /// </summary>
    /// <typeparam name="TIGrain">type of grain for query result</typeparam>
    [Serializable]
    [GenerateSerializer]
    public class OrleansQueryResultStream<TIGrain> : IOrleansQueryResultStream<TIGrain> where TIGrain : IIndexableGrain
    {
        // TODO: Currently, the whole result is stored here, but it is just a simple implementation. This implementation should
        // be replaced with a more sophisticated approach to asynchronously read the results on demand

        [Id(0)]
        protected IAsyncStream<TIGrain>? _stream;

        // Accept a queryResult instance which we shall observe
        public OrleansQueryResultStream(IAsyncStream<TIGrain> stream) => this._stream = stream;

        public IOrleansQueryResultStream<TOGrain> Cast<TOGrain>() where TOGrain : IIndexableGrain
            => new OrleansQueryResultStreamCaster<TIGrain, TOGrain>(this);

        public void Dispose() => this._stream = null;

        public Task OnCompletedAsync() => this._stream?.OnCompletedAsync() ?? Task.CompletedTask;

        public Task OnErrorAsync(Exception ex) => this._stream?.OnErrorAsync(ex) ?? Task.CompletedTask;

        public virtual Task OnNextAsync(TIGrain item, StreamSequenceToken? token = null)
            => this._stream?.OnNextAsync(item, token) ?? Task.CompletedTask;

        public virtual Task OnNextAsync(IList<SequentialItem<TIGrain>> batch)
            => Task.WhenAll(batch.Select(item => this._stream?.OnNextAsync(item.Item, item.Token) ?? Task.CompletedTask));

        public Task<StreamSubscriptionHandle<TIGrain>> SubscribeAsync(IAsyncBatchObserver<TIGrain> observer)
            => this._stream?.SubscribeAsync(observer) ?? Task.FromResult<StreamSubscriptionHandle<TIGrain>>(null!);

        public Task<StreamSubscriptionHandle<TIGrain>> SubscribeAsync(IAsyncBatchObserver<TIGrain> observer, StreamSequenceToken? token)
            => this._stream?.SubscribeAsync(observer, token) ?? Task.FromResult<StreamSubscriptionHandle<TIGrain>>(null!);
    }
}
