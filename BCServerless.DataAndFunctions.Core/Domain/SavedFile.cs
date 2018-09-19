using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Core.Domain
{
    public class SavedFile : BaseModel
    {
        public string documentId { get; set; }
        public string fileName { get; set; }
        public string attachmentId { get; set; }
    }
}