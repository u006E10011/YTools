using UnityEngine;

namespace YTools
{
    public class AudioController : PersistentSingleton<AudioController>
    {
        private static ObjectPool<Audio> _pool;
        private static Transform _transform => Instance.transform;

        [RuntimeInitializeOnLoadMethod]
        public static void Init()
        {
            var data = Resources.Load<AudioData>(nameof(AudioData)).Init();
            var source = new GameObject().AddComponent<Audio>();
            var index = 0;

            _pool = new ObjectPool<Audio>(source, data.Preload, true, _transform).ForEach(x =>
            {
                x.Init($"Audio {index}", _pool, data, _transform);
                index++;
            });
        }

        public static Audio Get(System.Action<Audio> callback)
        {
            var source = _pool.Get(item => item.Init($"[Created] Audio", _pool, Resources.Load<AudioData>(nameof(AudioData)), _transform));
            callback?.Invoke(source);
            return source;
        }
    }
}