// **************************************************************** //
//
//   Copyright (c) RimuruDev. All rights reserved.
//   Contact me: 
//          - Gmail:    rimuru.dev@gmail.com
//          - GitHub:   https://github.com/RimuruDev
//          - LinkedIn: https://www.linkedin.com/in/rimuru/
//          - GitHub Organizations: https://github.com/Rimuru-Dev
//
// **************************************************************** //

using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RimuruDev
{
    public sealed class JsonToFileStorageService : IStorageService
    {
        public string GetDataPath { get; } = Application.persistentDataPath + "/Database";

        public void Save(string key, object data, Action<bool> onCallback = null)
        {
            var path = BuildPath(key);

            if (!Directory.Exists(GetDataPath))
                Directory.CreateDirectory(GetDataPath);

            var json = JsonConvert.SerializeObject(data);

            File.WriteAllText(path, json);

            onCallback?.Invoke(true);
        }

        public void Load<TData>(string key, Action<TData> onCallback)
        {
            var path = BuildPath(key);

            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                var data = JsonConvert.DeserializeObject<TData>(json);

                onCallback?.Invoke(data);
            }
            else
            {
                onCallback?.Invoke(default);
            }
        }

        public async Task LoadAsync<TData>(string key, Action<TData> onCallback) =>
            await Task.Run(() => Load(key, onCallback));

        private string BuildPath(string key) =>
            Path.Combine(GetDataPath, key);
    }
}
