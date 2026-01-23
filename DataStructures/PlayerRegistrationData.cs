using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WYSAPlayerRanker.DataStructures
{
    public class PlayerRegistrationData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int GradeLevel { get; set; }

        [Browsable(false)]
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        [Browsable(false)]
        public string Key
        {
            get
            {
                return FullName.ToLower();
            }
        }
    }
}
