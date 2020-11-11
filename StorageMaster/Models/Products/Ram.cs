using System;
using System.Collections.Generic;
using System.Text;

namespace StorageMaster
{
    //Assigning as child class of Product
    public class Ram:Product
    {
        //initialising Ram class
        public Ram()
        {
            //assigning weight to Ram unit
            this.Weight = 0.1;
        }
    }
}