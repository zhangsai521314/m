using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEl
{
    [MetadataType(typeof(MyClass))]
    public partial class Class
    {

    }

    class MyClass
    {
        public int ID { get; set; }
        public string ClassName { get; set; }

        public int? ClassLevel { get; set; }
        public int? ClassMaxCount { get; set; }
        public int? ClassNewCount { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }

        
    }

}
