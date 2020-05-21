using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath; 

/*
 * NOT WORKING PROPERLY
 * 
 * NEEDS NAMES TO BE FIXED AS THEY DISPLAY INCORRECTLY BELOW ICON WHEN PRINTED
 *      seperate names text names dont work... 
 *          id works so why not text?
 * NEEDS PROPER VALUE VEIW
 * NEEDS MORE KVP / XML FEILDS
 * 
 * OLD  -> NOTE: REMOVED DICTIONARIES AS THEY ARE NOT BEING USED
 *    MIGHT BE USED LATER FOR WRITE, BETTER METHOD IS PROBABLY AVAILABLE
 *          IE. PATH BUILDER TO CREATE EDITED XML ELEMENT, THEN SIMPLE: OPEN, FIND, REPLACE, CLOSE.
 *          
 * NOTE: NEED DICTIONARY'S, DATA NEEDS TO BE STORED AND COMPLETE XML FILE NEEDS TO BE REWRITTEN WITH THE DATA BY THIS PROGRAM
    */

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public const string file = @"..\\..\\INPUT\\xmlid.xml";

        public string name;
        public string key;
        public string value;

        public string id;
        public string text;
        public string location;

        public string feild;

        public string test;

        public int x = 0;
        public int y = 0;
        public int z = 0;

        public string namePath;
        public string keyPath;
        public string valuePath;

        public string allNamePath;
        public string allKeyPath;
        public string allValuePath;

        public string nameID = "nametochange";
        public string keyID = "keytochange";
        public string valueID = "valuetochange";

        public string countPass;

        XPathNavigator nav;
        XPathDocument docNav;

        public Form1() { InitializeComponent(); }

        public void Main()
        {

        }

        public void Set()
        {
            docNav = new XPathDocument(file);
            nav = docNav.CreateNavigator();
        }

        public void GenerateNameControls()
        {
            Label NameLabel = new Label
            {
                Name = text,
                Text = text,
                Tag = "ClearName",
                Size = new Size(50, 15),
                Location = new Point(50 + (x * 125), 175)
            };
            Controls.Add(NameLabel);

            PictureBox NameImgBox = new PictureBox
            {
                Name = id,
                ImageLocation = location,
                Tag = "ClearName",
                Size = new Size(100, 100),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Location = new Point(50 + (x * 125), 50)
            };
            Controls.Add(NameImgBox);
            NameImgBox.Click += new EventHandler(this.ToKey_Click);
        }

        public void GenerateKeyControls()
        {
            Label KeyLabel = new Label
            {
                Name = text,
                Text = text,
                Tag = "ClearKey",
                Size = new Size(50, 15),
                Location = new Point(50 + (y * 125), 175)
            };
            Controls.Add(KeyLabel);

            PictureBox KeyImgBox = new PictureBox
            {
                Name = id,
                ImageLocation = location,
                Tag = "ClearKey",
                Size = new Size(100, 100),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Location = new Point(50 + (y * 125), 50)
            };
            Controls.Add(KeyImgBox);
            KeyImgBox.Click += new EventHandler(this.ToValue_Click);
        }

        public void GenerateValueControls()
        {
            Label ValueLabel = new Label
            {
                Name = text,
                Text = text,
                Tag = "ClearValue",
                Size = new Size(50, 15),
                Location = new Point(50 + (z * 125), 175)
            };
            Controls.Add(ValueLabel);

            PictureBox ValueImgBox = new PictureBox
            {
                Name = id,
                ImageLocation = location,
                Tag = "ClearValue",
                Size = new Size(100, 100),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Location = new Point(50 + (z * 125), 50)
            };
            Controls.Add(ValueImgBox);
            ValueImgBox.Click += new EventHandler(this.ValueView_Click);
        }

        public void LoadName(object sender, EventArgs e)
        {
            x = 0;

            XDocument doc = XDocument.Load(file);
            var name = doc.Descendants("name")
                                 .Select(x => new
                                 {
                                     id = x.Attribute("id").Value,
                                     text = x.Attribute("text").Value,
                                     location = x.Attribute("location").Value
                                 }).ToArray();

            foreach (var all in name)
            {
                x += 1;

                id = all.id;
                text = all.text;
                location = all.location;

                GenerateNameControls();

            }

        }

        public void LoadKey(object sender, EventArgs e)
        {
            y = 0;
            /*
             dont load file?
             push keyPath into doc. from PathSet method
             https://support.microsoft.com/en-au/help/308333/how-to-query-xml-with-an-xpath-expression-by-using-visual-c
             https://docs.microsoft.com/en-us/previous-versions/dotnet/netframework-4.0/ms256086(v=vs.100)

             */

            var doc = XDocument.Load(file);

            string countValues = "count(//name[@id=\"" + nameID + "\"]/key)";
            countPass = nav.Evaluate(countValues).ToString();
            Int32.TryParse(countPass, out int keyCount);

            int i = 0;

            for (i = 1; i <= keyCount; i++)
            {
                IEnumerable att = (IEnumerable)doc.XPathEvaluate("//name[@id=\"" + nameID + "\"]/key[" + i + "]/@*");

                string[] matchingValues = att.Cast<XAttribute>().Select(x => x.Value).ToArray();

                id = matchingValues[0];
                text = matchingValues[1];
                location = matchingValues[2];

                y = i;

                GenerateKeyControls();

                Console.WriteLine(id);
                Console.WriteLine(text);
                Console.WriteLine(location);

            }

            #region back button
            PictureBox KeyBack = new PictureBox
            {
                Name = "NameBack",
                ImageLocation = "..\\..\\INPUT\\back.png",
                Tag = "ClearKey",
                Size = new Size(100, 100),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Location = new Point(50 + ((y + 1) * 125), 50)
            };
            Controls.Add(KeyBack);
            KeyBack.Click += new EventHandler(this.ToName_Click);
            #endregion 

            keyCount = 0;
            i = 1;
        }

        public void LoadValue(object sender, EventArgs e)
        {
            z = 0;

            var doc = XDocument.Load(file);

            string countValues = "count(//name[@id=\"" + nameID + "\"]/key[@id=\"" + keyID + "\"]/value)";
            countPass = nav.Evaluate(countValues).ToString();
            Int32.TryParse(countPass, out int valueCount);

            int i = 0;

            for (i = 1; i <= valueCount; i++)
            {
                IEnumerable att = (IEnumerable)doc.XPathEvaluate("//name[@id=\"" + nameID + "\"]/key[@id=\"" + keyID +
                    "\"]/value[" + i + "]/@*");

                string[] matchingValues = att.Cast<XAttribute>().Select(x => x.Value).ToArray();

                id = matchingValues[0];
                text = matchingValues[1];
                location = matchingValues[2];

                z = i;

                GenerateValueControls();

                Console.WriteLine(id);
                Console.WriteLine(text);
                Console.WriteLine(location);

            }

            #region value back button

            PictureBox ValueBack = new PictureBox
            {
                Name = "NameBack",
                ImageLocation = "..\\..\\INPUT\\back.png",
                Tag = "ClearValue",
                Size = new Size(100, 100),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Location = new Point(50 + ((z + 1) * 125), 50)
            };
            Controls.Add(ValueBack);
            ValueBack.Click += new EventHandler(this.ToKey_Click);

            #endregion

            valueCount = 0;
            i = 1;


        }

        public void LoadValueView(object sender, EventArgs e)
        {
            var doc = XDocument.Load(file);

            IEnumerable att = (IEnumerable)doc.XPathEvaluate("//name[@id=\"" + nameID + "\"]/key[@id=\"" + keyID + "\"]" +
                "/value[@id=\"" + valueID + "\"]/@*");

            string[] matchingValues = att.Cast<XAttribute>().Select(x => x.Value).ToArray();

            id = matchingValues[0];
            text = matchingValues[1];
            location = matchingValues[2];
            feild = matchingValues[3];

            Console.WriteLine("\n----------");
            Console.WriteLine(id);
            Console.WriteLine(text);
            Console.WriteLine(location);
            Console.WriteLine(feild);
            Console.WriteLine("----------\n");

            PictureBox ValueBack = new PictureBox
            {
                Name = "NameBack",
                ImageLocation = "..\\..\\INPUT\\back.png",
                Tag = "ClearKey", //tag to be set properly at later stage
                Size = new Size(100, 100),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Location = new Point(200, 100)
            };
            Controls.Add(ValueBack);
            ValueBack.Click += new EventHandler(this.ToValue_Click);
        }

        #region clear functions
        public void ClearAll()
        {
            for (int i = Controls.Count - 1; i >= 0; i--)
            {
                if (
                    (string)Controls[i].Tag == "ClearName" ||
                    (string)Controls[i].Tag == "ClearKey" ||
                    (string)Controls[i].Tag == "ClearValue" ||
                    (string)Controls[i].Tag == "NameBack"
                    )
                {
                    Controls.RemoveAt(i);
                }
            }
        } //name, key, value

        public void ClearName()
        {
            //x=0;=>
            for (int i = Controls.Count - 1; i >= 0; i--)
            {
                if ((string)Controls[i].Tag == "ClearName")
                {
                    Controls.RemoveAt(i);
                }
            }

            //DictName.Clear();
        }

        public void ClearKey()
        {
            //x=0;=>
            for (int i = Controls.Count - 1; i >= 0; i--)
            {
                if ((string)Controls[i].Tag == "ClearKey")
                {
                    Controls.RemoveAt(i);
                }
            }

            //DictKey.Clear();
        }

        public void ClearValue()
        {
            //x=0;=>
            for (int i = Controls.Count - 1; i >= 0; i--)
            {
                if ((string)Controls[i].Tag == "ClearValue")
                {
                    Controls.RemoveAt(i);
                }
            }

            //DictValue.Clear();
        }

        #endregion

        #region buttons and runs
        private void ClearButton_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            ClearAll();
            Set();
            LoadName(sender, e);

        }

        private void ToName_Click(object sender, EventArgs e)
        {
            //ClearAll(); //update clear all to include all variables?

            ClearKey();
            LoadName(sender, e);
            Console.WriteLine("\n----------name finished----------\n");

        }

        private void ToKey_Click(object sender, EventArgs e)
        {
            ClearName();
            ClearValue();

            PictureBox NameImgBox = (PictureBox)sender;

            if (NameImgBox.Name != "NameBack")
            {
                nameID = NameImgBox.Name;
            }


            LoadKey(sender, e);

            Console.WriteLine("\n----------key finished----------\n");

        }

        private void ToValue_Click(object sender, EventArgs e)
        {
            ClearKey();

            PictureBox KeyImgBox = (PictureBox)sender;

            if (KeyImgBox.Name != "NameBack")
            {
                keyID = KeyImgBox.Name;
            }

            LoadValue(sender, e);

            Console.WriteLine("\n----------value finished----------\n");

        }

        #endregion

        private void ValueView_Click(object sender, EventArgs e)
        {
            ClearAll();

            PictureBox ValueImgBox = (PictureBox)sender;

            if (ValueImgBox.Name != "NameBack")
            {
                valueID = ValueImgBox.Name;
            }

            LoadValueView(sender, e);
        }
    }
}
