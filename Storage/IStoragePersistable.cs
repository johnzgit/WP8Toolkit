namespace Ayls.WP8Toolkit.Storage
{
    public interface IStoragePersistable<T>
    {
        T DataToPersist { get; }
        void InitializeFromPersistedData(T obj);
    }
}