using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Person.Models;

namespace TeleMedBot.Models
{
    public class Request
    {
        public Properties properties { get; set; }
        public string type { get; set; }
    }

    public class OverviewRequest
    {
        public OverviewProps properties { get; set; }
        public string type { get; set; }
    }

    public class Properties
    {
        public AppName appname { get; set; }
        public Tenant tenant { get; set; }
        public Doctor doctor { get; set; }
    }

    public class OverviewProps
    {
        public Person.Models.Person requestPerson { get; set; }
    }

    public class AppName
    {
        public string value { get; set; }
        public string type { get; set; }
    }

    public class Tenant
    {
        public string value { get; set; }
        public string type { get; set; }
    }

    public class Doctor
    {
        public string value { get; set; }
        public string type { get; set; }
    }

    public class PatientID
    {
        public string value { get; set; }
        public string type { get; set; }
    }

    public class FName
    {
        public string value { get; set; }
        public string type { get; set; }
    }

    public class LName
    {
        public string value { get; set; }
        public string type { get; set; }
    }

    public class DOB
    {
        public DateTime value { get; set; }
        public DateTime type { get; set; }
    }

    public class NextAppt
    {
        public DateTime value { get; set; }
        public DateTime type { get; set; }
    }

    public class Height
    {
        public string value { get; set; }
        public string type { get; set; }
    }

    public class Weight
    {
        public Int32 value { get; set; }
        public Int32 type { get; set; }
    }
}