using System.IO;
using UnityEngine;

namespace DefaultNamespace
{
    public class SaveRepository
    {
        private JsonSaveData<SavedData> _data;
        private string _path;
        private const string _folderName = "Save";
        private const string _fileName = "save.json";

        public void Initialization()
        {
            _data = new JsonSaveData<SavedData>();
            _path = Path.Combine(Application.dataPath, _folderName);           
        }

        public void Save()
        {
            if (!Directory.Exists(Path.Combine(_path)))
            {
                Directory.CreateDirectory(_path);
            }
            ;
            var savedData = new SavedData()
            {
                RewardSave = new RewardSaveModel()
            };            

            _data.Save(savedData, Path.Combine(_path, _fileName));
            Debug.Log("Save");
        }

        public void Load()
        {
            var file = Path.Combine(_path, _fileName);

            if (!File.Exists(file))
            {
                return;
                //throw new DataException($"File {file} not found");
            }

            var savedData = _data.Load(file);
            savedData.RewardSave.InitSaveData();
            Debug.Log("Load");
        }
    }
}