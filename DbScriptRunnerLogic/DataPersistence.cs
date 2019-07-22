using DbScriptRunnerLogic.Entities;
using DbScriptRunnerLogic.Interfaces;
using System;
using System.Collections.Generic;

namespace DbScriptRunnerLogic
{
    public class DataPersistence
    {
        public IRepository Repository { get; set; }

        public IArrangeableList<INamed> Items { get; set; } = new ArrangeableList<INamed>();

        public IArrangeableList<INamed> Load()
        {
            string data = Repository.Load();
            Items.InitializeWith(DeserializeItems(data));
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

        private ArrangeableList<INamed> DeserializeItems(string data)
        {
            var dataRows = data.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            var items = new ArrangeableList<INamed>();
            foreach (var dataRow in dataRows)
            {
                if (!string.IsNullOrEmpty(dataRow)) 
                    items.Add(new Entities.Database { Name = dataRow });
            }
            return items;
        }
    }
}
