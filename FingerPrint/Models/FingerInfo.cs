using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace FingerPrint.Models
{
    [DataContract]
    public class FingerInfo
    {
        [DataMember(Name = "subject")]
        public byte[] subject { get; set; }
        [DataMember(Name = "person")]
        public byte[] person { get; set; }
        [DataMember(Name = "user")]
        public string user { get; set; }

        [DataMember(Name = "score")]
        public string score { get; set; }
    }
    [DataContract]
    public class FingerInfoTemplateOneToOne
    {
        [DataMember(Name = "subject")]
        public string subject { get; set; }
        [DataMember(Name = "person")]
        public string person { get; set; }
        [DataMember(Name = "score")]
        public string score { get; set; }
        [DataMember(Name = "result")]
        public string result { get; set; }
    }

    [DataContract]
    public class Authentication
    {
        [DataMember(Name = "subject")]
        public byte[] subject { get; set; }

        [DataMember(Name = "subject1")]
        public byte[] subject1 { get; set; }

        [DataMember(Name = "subject2")]
        public byte[] subject2 { get; set; }

        [DataMember(Name = "subject3")]
        public byte[] subject3 { get; set; }

        [DataMember(Name = "subject4")]
        public byte[] subject4 { get; set; }

        [DataMember(Name = "subject5")]
        public byte[] subject5 { get; set; }

        [DataMember(Name = "user")]
        public string user { get; set; }

        [DataMember(Name = "score")]
        public string score { get; set; }

        [DataMember(Name = "control")]
        public string control { get; set; }

        [DataMember(Name = "fingerCount")]
        public string fingerCount { get; set; }

    }

    [DataContract]
    public class Many
    {
        [DataMember(Name = "subject")]
        public byte[] subject { get; set; }
        [DataMember(Name = "persons")]
        public string persons { get; set; }
        [DataMember(Name = "user")]
        public string user { get; set; }

    }

    [DataContract]
    public class Template
    {
        [DataMember(Name = "base_64")]
        public byte[] base_64 { get; set; }

        [DataMember(Name = "templateData")]
        public string templateData { get; set; }

        [DataMember(Name = "result")]
        public string result { get; set; }
        [DataMember(Name = "user")]
        public string user { get; set; }
    }

    [DataContract]
    public class ManyControl
    {
        [DataMember(Name = "subject")]
        public byte[] subject { get; set; }

        [DataMember(Name = "user")]
        public string user { get; set; }

        [DataMember(Name = "id")]
        public int id { get; set; }

        [DataMember(Name = "control")]
        public string control { get; set; }

        [DataMember(Name = "fingerCount")]
        public string fingerCount { get; set; }

        [DataMember(Name = "template")]
        public byte[] template { get; set; }

        [DataMember(Name = "template1")]
        public string template1 { get; set; }

        [DataMember(Name = "template2")]
        public string template2 { get; set; }

        [DataMember(Name = "template3")]
        public string template3 { get; set; }

        [DataMember(Name = "template4")]
        public string template4 { get; set; }
    }

    [DataContract]
    public class template
    {
        [DataMember(Name = "subject")]
        public byte[] subject { get; set; }

        [DataMember(Name = "user")]
        public string user { get; set; }

        [DataMember(Name = "result")]
        public string result { get; set; }

        [DataMember(Name = "control")]
        public string control { get; set; }

        [DataMember(Name = "fingerCount")]
        public string fingerCount { get; set; }

    }

}