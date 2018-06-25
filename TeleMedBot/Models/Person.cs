using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Microsoft.Azure.Documents;

namespace Person.Models
{
    public class Person
    {

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "FName")]
        public string FName { get; set; }

        [JsonProperty(PropertyName = "LName")]
        public string LastName { get; set; }

        //[JsonProperty(PropertyName = "MName")]
        //public string MidName { get; set; }

        //[JsonProperty(PropertyName = "Occupation")]
        //public string Occupation { get; set; }

        [JsonProperty(PropertyName = "Height")]
        public string Height { get; set; }

        [JsonProperty(PropertyName = "Weight")]
        public Int32 Weight { get; set; }

        [JsonProperty(PropertyName = "DOB")]
        public DateTime DOB { get; set; }

        [JsonProperty(PropertyName = "ChatHeight")]
        public string ChatHeight { get; set; }

        [JsonProperty(PropertyName = "ChatWeight")]
        public Int32 ChatWeight { get; set; }

        [JsonProperty(PropertyName = "ProfileImg")]
        public Uri ProfileImg { get; set; }

        [JsonProperty(PropertyName = "Active")]
        public bool Active { get; set; }

        [JsonProperty(PropertyName = "Meds")]
        public Medication[] Meds { get; set; }

        [JsonProperty(PropertyName = "Allergies")]
        public Allergy[] Allergies { get; set; }

        [JsonProperty(PropertyName = "History")]
        public MedicalHistory[] History { get; set; }

        /*Represents the type of object being stored.  Especially important for abstract objects like Person since a Person can be a
        a Patient, Subject, Doctor, etc. */
        [JsonProperty(PropertyName = "Type")]
        public string Type { get; set; }

        //Represents the tenant using the WebApi, should be on each API created
        [JsonProperty(PropertyName = "Tenant")]
        public string Tenant { get; set; }

        //Represents the App which the Api is being used by the Tenant since the same API can be used by the same tenat across multiple apps
        [JsonProperty(PropertyName = "App")]
        public string App { get; set; }

        public static implicit operator Person(Document v)
        {
            throw new NotImplementedException();
        }
    }

    public class Medication
    {
        [JsonProperty(PropertyName = "MedicationName")]
        public string MedicationName { get; set; }

        [JsonProperty(PropertyName = "Doseage")]
        public string Doseage { get; set; }

        [JsonProperty(PropertyName = "NumRefills")]
        public Int32 NumRefills { get; set; }

        [JsonProperty(PropertyName = "RefillExp")]
        public DateTime RefillExp { get; set; }

        [JsonProperty(PropertyName = "FirstPrescribed")]
        public DateTime FirstPrescribed { get; set; }

        [JsonProperty(PropertyName = "WFID")]
        public string WFID { get; set; }
    }

    public class Allergy
    {
        [JsonProperty(PropertyName = "AllergyName")]
        public string AllergyName { get; set; }

        [JsonProperty(PropertyName = "AllergySince")]
        public DateTime AllergySince { get; set; }
    }

    public class MedicalHistory
    {
        [JsonProperty(PropertyName = "Surgeries")]
        public string[] Surgeries { get; set; }

        [JsonProperty(PropertyName = "Problems")]
        public string[] Problems { get; set; }
    }

    /*    [JsonProperty(PropertyName="id")]
            public string Id { get; set; }

            [JsonProperty(PropertyName = "FName")]
            public string FName { get; set; }

            [JsonProperty(PropertyName = "LName")]
            public string LastName { get; set; }

            //[JsonProperty(PropertyName = "MName")]
            //public string MidName { get; set; }

            //[JsonProperty(PropertyName = "Occupation")]
            //public string Occupation { get; set; }

            [JsonProperty(PropertyName = "Height")]
            public string Height { get; set; }

            [JsonProperty(PropertyName = "Weight")]
            public Int32 Weight { get; set; }

            [JsonProperty(PropertyName = "DOB")]
            public DateTime DOB { get; set; }

            [JsonProperty(PropertyName = "Active")]
            public bool Active { get; set; }

            [JsonProperty(PropertyName = "NextAppt")]
            public DateTime NextAppt { get; set; }

            /*Represents the type of object being stored.  Especially important for abstract objects like Person since a Person can be a
            a Patient, Subject, Doctor, etc. */

    /*
    [JsonProperty(PropertyName = "Type")]
    public string Type { get; set; }

    //Represents the tenant using the WebApi, should be on each API created
    [JsonProperty(PropertyName = "Tenant")]
    public string Tenant { get; set; }

    //Represents the App which the Api is being used by the Tenant since the same API can be used by the same tenat across multiple apps
    [JsonProperty(PropertyName = "App")]
    public string App { get; set; } 


    public static implicit operator Person(Document v)
    {
        throw new NotImplementedException();
    }
}*/
}