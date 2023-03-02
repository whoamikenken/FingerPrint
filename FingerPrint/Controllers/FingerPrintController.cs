using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FingerPrint.Models;
using SourceAFIS.Simple;
using System.Drawing;
using System.IO;
using System.Data;
using MySql.Data.MySqlClient;
using System.Web.Http.Cors;
using System.Text;
using System.Collections;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace FingerPrint.Controllers
{

    public class FingerPrintController : ApiController
    {
        static AfisEngine Afis;
        Bitmap newBitmap;
        string personbase64 = string.Empty;
        string subjectbase64 = string.Empty;
        byte[] subjecttemplate, persontemplate;
        string base64 = string.Empty;
        static string personAdding = "None";
        byte[] pic;
        static List<Person> allPersons = new List<Person>();
        static Person person2 = new Person();
        static Person person1 = new Person();

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpPost]
        [Route("api/OneToFive")]
        public Authentication Post([FromBody]Authentication data)
        {
            try
            {
                Afis = new AfisEngine();

                Fingerprint fp1 = new Fingerprint();
                Fingerprint subject1 = new Fingerprint();
                Fingerprint subject2 = new Fingerprint();
                Fingerprint subject3 = new Fingerprint();
                Fingerprint subject4 = new Fingerprint();
                Fingerprint subject5 = new Fingerprint();

                if (data.control == "add")
                {
                    person2.Fingerprints.Clear();
                    subject1.AsBitmap = ByteToImage(data.subject1);
                    subject2.AsBitmap = ByteToImage(data.subject2);
                    subject3.AsBitmap = ByteToImage(data.subject3);
                    subject4.AsBitmap = ByteToImage(data.subject4);
                    subject5.AsBitmap = ByteToImage(data.subject5);

                    person2.Fingerprints.Add(subject1);
                    person2.Fingerprints.Add(subject2);
                    person2.Fingerprints.Add(subject3);
                    person2.Fingerprints.Add(subject4);
                    person2.Fingerprints.Add(subject5);
                    Afis.Extract(person2);
                    return new Authentication
                    {
                        control = $"{data.control}",
                        fingerCount = $"{person2.Fingerprints.Count().ToString()}"
                    };
                }
                else if (data.control == "find")
                {
                    person1.Fingerprints.Clear();
                    fp1.AsBitmap = ByteToImage(data.subject);
                    person1.Fingerprints.Add(fp1);
                    Afis.Extract(person1);
                    float score = Afis.Verify(person1, person2);
                    bool match = (score > 20);
                    string total = score.ToString();
                    if (match)
                    {
                        return new Authentication
                        {
                            user = $"True",
                            score = $"{total}",
                            fingerCount = $"{person2.Fingerprints.Count().ToString()}"
                        };
                    }
                    else
                    {
                        return new Authentication
                        {
                            user = $"False",
                            score = $"{total}",
                            fingerCount = $"{person2.Fingerprints.Count().ToString()}"
                        };
                    }
                }
                else
                {
                    return new Authentication
                    {
                        user = $"ERROR"
                    };
                }
            }
            catch (Exception ex)
            {
                return new Authentication
                {
                    user = $"ERROR"
                };
            }
        }
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpPost]
        [Route("api/Template")]
        public Template Post([FromBody] Template data)
        {
            try
            {
                Afis = new AfisEngine();

                Person person = new Person();
                Fingerprint fp = new Fingerprint();

                fp.AsBitmap = ByteToImage(data.base_64);

                person.Fingerprints.Add(fp);
                Afis.Extract(person);
                allPersons.Add(person);

                byte[] template1 = fp.Template;

                string temp64 = Convert.ToBase64String(template1);

                return new Template
                {
                    result = $"{allPersons.Count.ToString()}",
                    templateData = $"{temp64}",
                    user = $"{data.user}"
                };
            }
            catch (Exception ex)
            {
                return new Template
                {
                    result = $"ERROR"
                };
            }
        }
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpPost]
        [Route("api/OneToOneTemplate")]
        // POST: api/FingerPrint
        public FingerInfoTemplateOneToOne Post([FromBody] FingerInfoTemplateOneToOne data)
        {
            Afis = new AfisEngine();

            Fingerprint fp1 = new Fingerprint();
            fp1.Template = Convert.FromBase64String(data.person);

            Fingerprint fp2 = new Fingerprint();
            fp2.Template = Convert.FromBase64String(data.subject);

            Person person1 = new Person();
            person1.Fingerprints.Add(fp1);

            Person person2 = new Person();
            person2.Fingerprints.Add(fp2);

            float score = Afis.Verify(person1, person2);
            bool match = (Convert.ToInt32(score) > 50);
            string total = score.ToString();
            if (match)
            {
                return new FingerInfoTemplateOneToOne
                {
                    result = $"True",
                    score = $"{total}"
                };
            }
            else
            {
                return new FingerInfoTemplateOneToOne
                {
                    result = $"False",
                    score = $"{total}",
                };
            }

        }
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpPost]
        [Route("api/OneToOne")]
        // POST: api/FingerPrint
        public FingerInfo Post([FromBody] FingerInfo data)
        {
            Afis = new AfisEngine();

            Fingerprint fp1 = new Fingerprint();
            fp1.AsBitmap = ByteToImage(data.person);

            Fingerprint fp2 = new Fingerprint();
            fp2.AsBitmap = ByteToImage(data.subject);

            Person person1 = new Person();
            person1.Fingerprints.Add(fp1);

            Person person2 = new Person();
            person2.Fingerprints.Add(fp2);

            Afis.Extract(person1);
            Afis.Extract(person2);

            float score = Afis.Verify(person1, person2);
            bool match = (score > 50);
            string total = score.ToString();
            if (match)
            {
                return new FingerInfo
                {
                    user = $"True",
                    score = $"{total}"
                };
            }
            else
            {
                return new FingerInfo
                {
                    user = $"False",
                    score = $"{total}",
                };
            }

        }
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpPost]
        [Route("api/IdentifyDatabase")]
        public Many Post([FromBody]Many data)
        {
            try
            {
                Afis = new AfisEngine();

                var i = 0;
                var personId = "None";
                if (Sqlconnection.conn.State == ConnectionState.Closed)
                {
                    Sqlconnection.conn.Open();
                    MySqlCommand login = new MySqlCommand("SELECT id,template FROM bio", Sqlconnection.conn);
                    MySqlDataReader read = login.ExecuteReader();
                    while (read.Read())
                    {
                        Person person = new Person();
                        person.Id = Convert.ToInt32(read[0]);
                        Fingerprint fp = new Fingerprint();

                        persontemplate = read[1] as byte[];
                        base64 = System.Text.Encoding.UTF8.GetString(persontemplate);
                        using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(base64)))
                        {
                            using (Bitmap bm2 = new Bitmap(ms))
                            {
                                fp.AsBitmap = bm2;
                                ms.Dispose();
                                bm2.Dispose();
                            }
                        }
                        person.Fingerprints.Add(fp);
                        Afis.Extract(person);
                        allPersons.Add(person);
                    }
                    read.Dispose();
                }

                Person unknownPerson = new Person();
                Fingerprint fingerprint = new Fingerprint();
                fingerprint.AsBitmap = ByteToImage(data.subject);
                unknownPerson.Fingerprints.Add(fingerprint);
                Afis.Extract(unknownPerson);

                var matches = Afis.Identify(unknownPerson, allPersons);

                var persons = matches as Person[] ?? matches.ToArray();

                foreach (var person in persons)
                {
                    personId = person.Id.ToString();
                }

                return new Many
                {
                    persons = $"{allPersons.Count.ToString()}",
                    user = $"{personId}"
                };
            }
            catch (Exception ex)
            {
                return new Many
                {
                    user = $"ERROR"
                };
            }
        }
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpPost]
        [Route("api/Identify")]
        public ManyControl Post([FromBody]ManyControl data)
        {
            try
            {
                Afis = new AfisEngine();

                if (data.control == "add")
                {
                    var personId = "None";
                    if (data.user != null)
                    {
                        personId = data.user;
                    }

                    Fingerprint fp1 = new Fingerprint();
                    Fingerprint fp2 = new Fingerprint();
                    Fingerprint fp3 = new Fingerprint();
                    Fingerprint fp4 = new Fingerprint();

                    Person person = new Person();

                    person.Id = Convert.ToInt32(data.user);

                    fp1.Template = Convert.FromBase64String(data.template1);

                    fp2.Template = Convert.FromBase64String(data.template2);
                    
                    fp3.Template = Convert.FromBase64String(data.template3);
                    
                    fp4.Template = Convert.FromBase64String(data.template4);

                    person.Fingerprints.Add(fp1);
                    person.Fingerprints.Add(fp2);
                    person.Fingerprints.Add(fp3);
                    person.Fingerprints.Add(fp4);

                    allPersons.Add(person);

                    if (data.user != null)
                    {
                        return new ManyControl
                        {
                            control = $"True",
                            fingerCount = $"{allPersons.Count.ToString()}",
                            user = $"{personId}"
                        };
                    }
                    else
                    {
                        return new ManyControl
                        {
                            control = $"False",
                            fingerCount = $"{allPersons.Count.ToString()}"
                        };
                    }
                }
                else if (data.control == "reset")
                {
                    allPersons.Clear();
                    return new ManyControl
                    {
                        control = $"Resetting",
                        fingerCount = $"{allPersons.Count.ToString()}"
                    };
                }
                else if (data.control == "info")
                {
                    return new ManyControl
                    {
                        control = $"info",
                        fingerCount = $"{allPersons.Count.ToString()}"
                    };
                }
                else if(data.control == "find")
                {
                    var personId = "None";
                    Person unknownPerson = new Person();
                    
                    Fingerprint fingerprint = new Fingerprint();
                    fingerprint.AsBitmap = ByteToImage(data.subject);
                    unknownPerson.Fingerprints.Add(fingerprint);
                    Afis.Extract(unknownPerson);
                    //.Where(person => person.Id == data.id)
                    var matches = Afis.Identify(unknownPerson, allPersons);

                    unknownPerson.Fingerprints.Clear();

                    var persons = matches as Person[] ?? matches.ToArray();

                    foreach (var person in persons)
                    {
                        personId = person.Id.ToString();
                        break;
                    }

                    return new ManyControl
                    {
                        control = $"find",
                        fingerCount = $"{allPersons.Count().ToString()}",
                        user = $"{personId}"
                    };
                }
                else
                {
                    return new ManyControl
                    {
                        control = $"Error",
                        fingerCount = $"{allPersons.Select(person => person.Id == 1).Count().ToString()}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ManyControl
                {
                    user = $"ERROR",
                    fingerCount = $"{allPersons.Count.ToString()}"
                };
            }
        }

        public static Bitmap ByteToImage(byte[] blob)
        {
            MemoryStream mStream = new MemoryStream();
            byte[] pData = blob;
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();
            return bm;
        }

        class Sqlconnection
        {
            public static MySqlConnection conn = new MySqlConnection("Data Source=192.168.2.167;Initial Catalog=OLFU_HRIS;User ID=admin;Password=passwrd");
        }
    }
}
