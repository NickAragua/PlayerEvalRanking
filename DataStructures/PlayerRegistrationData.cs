using System;
using System.Collections.Generic;
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

        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        public string Key
        {
            get
            {
                return FullName.ToLower();
            }
        }
    }
}
