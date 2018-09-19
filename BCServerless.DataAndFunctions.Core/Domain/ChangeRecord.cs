using Newtonsoft.Json;

namespace BCServerlessDemo.DataAndFunctions.Core.Domain
{
    public class ChangeRecord : BaseModel
    {
        public string action { get; set; }
        public string subjectId { get; set; }
        public string subjectType { get; set; }
        public string subjectDescription { get; set; }
        public object before { get; set; }
        public object after { get; set; }
    }
}
