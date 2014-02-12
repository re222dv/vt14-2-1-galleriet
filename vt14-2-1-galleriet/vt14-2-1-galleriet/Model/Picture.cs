using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vt14_2_1_galleriet.Model {
    public class Picture {
        public String Name {
            get;
            set;
        }

        public String Path {
            get {
                return String.Format("Content/Images/{0}", Name);
            }
        }

        public String Thumb {
            get {
                return String.Format("Content/Images/Thumb/{0}", Name);
            }
        }
    }
}