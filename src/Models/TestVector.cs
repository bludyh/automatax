using System.Collections.Generic;

namespace Automatax.Models
{
    public class TestVector
    {

        public TestVector()
        {
            Words = new Dictionary<string, bool?>();
        }

        public bool? IsDfa { get; set; }
        public Dictionary<string, bool?> Words { get; set; }
    }
}
