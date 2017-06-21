using System;
using System.Collections.Generic;
using System.Text;

namespace Plants.Importer
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<PlantModel> Plants { get; set; } = new List<PlantModel>();
    }
}
