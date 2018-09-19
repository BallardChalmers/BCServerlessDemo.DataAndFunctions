using BCServerlessDemo.DataAndFunctions.Core.Domain;
using BCServerlessDemo.DataAndFunctions.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Core.Data.Base
{
    public static class BaseMetadata
    {
        public static Metadata metaData => new Metadata()
        {
            id = Metadata.MetadataId,
            name = "Metadata",
            checklistItems = new List<LookupString>()
            {
                new LookupString() { id = "78d4598c-03fa-42a3-a23b-8e3ac2e4a632", name = "Joining instructions sent"},
                new LookupString() { id = "9e8b8688-2ef1-4037-8654-a758c8e1c2bf", name = "Journey materials printed"},
                new LookupString() { id = "0886e450-f4b4-4310-979b-e32da3dbc730", name = "Venue confirmed"},
            },
            ethnicities = new List<LookupString>()
            {
                new LookupString() { id = "7d78f831-17c3-4e56-b053-0e03aedc5a7e", name = "None Provided"},
                new LookupString() { id = "beb61d48-615b-4c02-9425-9eb65b339ef1", name = "01 English/Welsh/Scottish/Northern Irish/British"},
                new LookupString() { id = "447b6279-3567-4397-91a8-ec105d4d6eb6", name = "02 Irish"},
                new LookupString() { id = "47cd823a-d8d0-4017-bfd4-d523b3109f5b", name = "03 Gypsy or Irish Traveller"},
                new LookupString() { id = "71c8315f-de8b-4de3-9102-ff190d223c74", name = "04 Any other White background"},
                new LookupString() { id = "ddbf8a12-50b5-42b4-a5d9-54b9031614c8", name = "05 White and Black Caribbean"},
                new LookupString() { id = "11120d36-9846-4afa-a27f-7ba746acd984", name = "06 White and Black African"},
                new LookupString() { id = "12bb6966-cf83-4d91-9391-508fca3e1fe2", name = "07 White and Asian"},
                new LookupString() { id = "75bcdb54-7056-47fd-978b-e70ece0e1345", name = "08 Any other Mixed/multiple ethnic background"},
                new LookupString() { id = "f0b38ae5-05c9-4e2e-8c92-7c991b724a60", name = "09 Indian"},
                new LookupString() { id = "b3eedd11-af2e-4e0c-8ca9-431732a4a921", name = "10 Pakistani"},
                new LookupString() { id = "6c051aa0-2b3f-4132-89fb-d5c1f80c1c1b", name = "11 Bangladeshi"},
                new LookupString() { id = "1fca1774-6f6c-4620-9345-e60b8b5fed06", name = "12 Chinese"},
                new LookupString() { id = "ac32d80c-57ea-488a-adb4-bbcd2d043f9a", name = "13 Any other Asian background"},
                new LookupString() { id = "3c158ed0-3837-4b88-bbf9-898ca74ee4a1", name = "14 African"},
                new LookupString() { id = "12e0ee7c-eb9e-45e6-ba1a-4e511be29d61", name = "15 Caribbean"},
                new LookupString() { id = "82898635-cc0e-49e2-bdca-44b0da66a66a", name = "16 Any other Black/African/Caribbean background"},
                new LookupString() { id = "db69d592-6cb3-4dff-90f4-8ad8561a3bc7", name = "17 Arab"},
                new LookupString() { id = "96f7133a-1a1d-45f8-b92f-ff74992c3b6c", name = "18 Any other ethnic group"},
                new LookupString() { id = "59eb281e-a708-4bab-80b4-2e9a8230a43d", name = "19 White"},
                new LookupString() { id = "26b8708e-2cc5-4142-ae1e-b3c1b0d81bff", name = "20 Chinese"},
                new LookupString() { id = "b60559d8-adcf-40de-b97d-639bb7e3dfb1", name = "21 Irish Traveller"},
                new LookupString() { id = "710b5797-dc24-40ec-8f2f-e5a861d0768e", name = "22 Indian"},
                new LookupString() { id = "b721bb6c-842d-418e-b3a6-198b01e172c7", name = "23 Pakistani"},
                new LookupString() { id = "6baf49f8-9894-4eac-99b1-0d13044e4891", name = "24 Bangladeshi"},
                new LookupString() { id = "c7b6c305-0a23-4f72-aad0-a558b98b8d5e", name = "25 Black Caribbean"},
                new LookupString() { id = "5832715b-fdb7-4288-b8fb-e98562403b6c", name = "26 Black African"},
                new LookupString() { id = "8be7a45b-5c2e-43d8-9180-9bdfb660ec10", name = "27 Black other"},
                new LookupString() { id = "8870729f-ba58-49d1-84a7-bc68b2cfc3da", name = "28 Mixed ethnic group"},
                new LookupString() { id = "25f6c50c-7e85-409e-b4fb-6ecee6edbd46", name = "29 Any other ethnic group"},
                new LookupString() { id = "8e34020b-1a74-417c-bb3b-5e7378dc6873", name = "30 Scottish"},
                new LookupString() { id = "83a435df-898a-4c13-b992-a577061e4962", name = "31 British"},
                new LookupString() { id = "40f84f0b-3ace-4fa9-9ab8-dc09ff710e75", name = "32 Irish"},
                new LookupString() { id = "8b6fbaf4-37ce-45f2-9019-5dd88e8e8ccf", name = "33 Any other White background"},
                new LookupString() { id = "dd77a3a3-c91f-4f23-a3fc-11653ffd5ace", name = "34 Mixed"},
                new LookupString() { id = "1e7ac028-8944-47c7-92ae-5d90859e8366", name = "35 Indian"},
                new LookupString() { id = "4fc2e04a-8909-4287-8140-8264c9c9950f", name = "36 Pakistani"},
                new LookupString() { id = "c654130d-95d6-40cd-9f8b-7130da87deac", name = "37 Bangladeshi"},
                new LookupString() { id = "09075f82-15d1-4a4b-b42b-bb36fd05edcf", name = "38 Chinese"},
                new LookupString() { id = "2432db1a-4f32-4122-9f44-e759ecca7619", name = "39 Any other Asian background, Black, Black Scottish or Black British"},
                new LookupString() { id = "451c5ce9-1b45-4c72-8b66-02b4212f65c0", name = "40 Caribbean"},
                new LookupString() { id = "27fb9c0b-2481-4e45-b6d0-2458041a4133", name = "41 African"},
                new LookupString() { id = "4cdc9811-df4d-458f-ae14-a36307655e9e", name = "42 Any other Black background"},
                new LookupString() { id = "d46f4973-920a-4774-823a-22a78a4c4893", name = "43 Any other ethnic group"},
            },
            learnerRatingQuestions = new List<LookupString>()
            {
                new LookupString() { id = "7aacc414-333c-4720-9545-9ffa34825c84", name = "Clear and easy"},
                new LookupString() { id = "e06dc92f-0d61-45eb-b242-85dc3d18485a", name = "Existing knowledge"},
                new LookupString() { id = "05d4a4a6-a7a6-4752-80bc-d12801c8df70", name = "Driver style"},
                new LookupString() { id = "30ee21a9-ac1b-4842-8357-4a14f1613091", name = "Journey objectives"},
            },
            riskHazardTypes = new List<LookupString>()
            {
            new LookupString() { id = "b942b876-1f29-4009-9f0d-fd315e226e68", name = "Fire and Serious / Imminent Danger (e.g. from gas explosion etc.)"},
            new LookupString() { id = "fcfe0691-ac92-4be9-b10d-3bbfb0f18c41", name = "Electricity – item itself"},
            new LookupString() { id = "0fe16fa1-8097-466c-951c-fe24a12e8e1b", name = "Trailing leads, bags"},
            new LookupString() { id = "ea9ee34e-2d38-4784-a76f-c336d7126c5c", name = "Changes in floor level / steps – slips and trips"},
            new LookupString() { id = "5ea034c4-0100-4a0c-8fbc-5cc76b1db903", name = "Weather"},
            new LookupString() { id = "f75d4598-c73f-44c0-aab2-7ab9a89dcafa", name = "Vehicle Parking"},
            new LookupString() { id = "0225f340-7d2c-41bf-b08f-72154ac484ac", name = "Traffic"},
            new LookupString() { id = "0a48bced-a253-4698-ba94-1e5f0dd85a58", name = "Hazardous Substances"},
            new LookupString() { id = "13e1ff97-3280-4c8b-a935-32883590f5c5", name = "Availability of Emergency Telephone /communication established"},
            new LookupString() { id = "10ee9a59-bd87-4bbb-b616-d730d27aec06", name = "Other"},
            },
            riskPeople = new List<LookupString>()
            {
            new LookupString() { id = "ccf66ed8-4709-44dd-8c80-1deed4d3ae15", name = "Learner"},
            new LookupString() { id = "20ee5f52-b40e-4c9f-b279-8848560ee70e", name = "Learner/Public"},
            new LookupString() { id = "ff4838a4-a996-463b-bff3-d1b7205b1a67", name = "Driver"},
            new LookupString() { id = "7a4298e0-8d2c-4b81-9074-c3c50cbaa7dd", name = "Other"},
            }
        };
    }
}
