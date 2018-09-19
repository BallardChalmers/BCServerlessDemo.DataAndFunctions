using EnumsNET;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Core.Domain
{
    //Note - The AsDocumentQuery() method takes no notice of the Json Serializer settings so produces broken queries unless the object properties are lower case!
    public class BaseModel
    {
        public string id { get; set; }
        public virtual string name { get; set; }

        public string type { get; private set; }
        public DateTime changedOn { get; set; }
        public string changedById { get; set; }
        public string changedByName { get; set; }
        public bool deleted { get; set; }
        
        //Doesn't take into account bank holidays
        public int WorkingDays20 => DateTime.Now.DayOfWeek == DayOfWeek.Sunday ? 27 : (DateTime.Now.DayOfWeek == DayOfWeek.Saturday ? 26 : 28);

        public BaseModel()
        {
            type = this.GetType().Name;
        }
    }
}
