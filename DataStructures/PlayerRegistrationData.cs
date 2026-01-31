using System.ComponentModel;

namespace WYSAPlayerRanker.DataStructures
{
    public class PlayerRegistrationData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int GradeLevel { get; set; }

        public string PreviousTeam { get; set; }

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
