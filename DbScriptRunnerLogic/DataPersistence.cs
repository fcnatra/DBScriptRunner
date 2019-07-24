using DbScriptRunnerLogic.Entities;
using DbScriptRunnerLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DbScriptRunnerLogic
{
    public class DataPersistence<T> where T: INamed, new()
    {
        public IRepository Repository { get; set; }

        public List<INamed> Items { get; set; } = new List<INamed>();

        public List<INamed> Load()
        {
            string data = Repository.Load();
            Items.AddRange(DeserializeItems(data));
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
            var dataRows = data
                .Split(new string[] { Environment.NewLine }, StringSplitOptions.None)
                .Where(row => !string.IsNullOrEmpty(row) && !row.StartsWith("\0"));

            var items = new List<INamed>();
            foreach (var dataRow in dataRows)
            {
                if (!string.IsNullOrEmpty(dataRow)) 
                    items.Add(new T { Name = dataRow });
            }
            return items;
        }
    }
}
