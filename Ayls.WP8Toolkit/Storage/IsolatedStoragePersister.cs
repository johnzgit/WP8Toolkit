using System.IO;
using System.IO.IsolatedStorage;
using System.Threading;
using Newtonsoft.Json;

namespace Ayls.WP8Toolkit.Storage
{
    public class IsolatedStoragePersister<TObject, TData> where TObject : IStoragePersistable<TData>
    {
        private readonly TObject _persistedObject;
        private readonly IsolatedStorageFile _appData;
        private readonly string _filename;
        private readonly Mutex _mutex;

        public IsolatedStoragePersister(TObject persistedObject, IsolatedStorageFile appData, string filename)
        {
            _persistedObject = persistedObject;
            _appData = appData;
            _filename = filename;
            _mutex = new Mutex(false, _filename);
        }

        public void Persist()
        {
            var data = _persistedObject.DataToPersist;
            var serializedData = JsonConvert.SerializeObject(data);

            _mutex.WaitOne();

            using (var sw = new StreamWriter(_appData.OpenFile(_filename, FileMode.Create)))
            {
                sw.Write(serializedData);
                sw.Close();
            }
            
            _mutex.ReleaseMutex();
        }

        public void Hydrate()
        {
            if (!_appData.FileExists(_filename))
            {
                return;
            }

            string serializedData;
            _mutex.WaitOne();

            using (var sr = new StreamReader(_appData.OpenFile(_filename, FileMode.Open)))
            {
                serializedData = sr.ReadToEnd();
                sr.Close();
            }

            _mutex.ReleaseMutex();

            try
            {
                var data = JsonConvert.DeserializeObject<TData>(serializedData);
                _persistedObject.InitializeFromPersistedData(data);
            }
            catch (JsonReaderException)
            {
            }
        }
    }
}