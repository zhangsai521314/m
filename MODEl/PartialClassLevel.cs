using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MODEl
{
    [MetadataType(typeof(MyClassLevel))]
    public partial class SYS_ClassLevel
    {
        public SYS_ClassLevel()
        {
            CreateDate = DateTime.Now;
            IsValid = true;
        }

    }

    class MyClassLevel
    {
        public int ID { get; set; }
        public string LevelName { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool? IsValid { get; set; }


    }
}
