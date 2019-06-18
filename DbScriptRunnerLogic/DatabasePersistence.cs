using DbScriptRunnerLogic.Entities;
using DbScriptRunnerLogic.Interfaces;
using System;
using System.Collections.Generic;

namespace DbScriptRunnerLogic
{
    public class DatabasePersistence
    {
        public IRepository Repository { get; set; }

        public List<INamed> Items { get; set; } = new List<INamed>();

        public List<INamed> Load()
        {
            string data = Repository.Load();
            Items = DeserializeItems(data);
            return Items;
        }

        public void Save()
        {
            var data = SerializeItems();
            Repository.Save(data);
        }

        private string SerializeItems()
        {
            string data = String.Empty;
            foreach (var item in Items)
            {
                data += item.Name + Environment.NewLine;
            }
            return data;
        }

        private List<INamed> DeserializeItems(string data)
        {
            var dataRows = data.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            var items = new List<INamed>();
            foreach (var dataRow in dataRows)
            {
                if (!string.IsNullOrEmpty(dataRow)) 
                    items.Add(new Entities.Database { Name = dataRow });
            }
            return items;
        }
    }
}
