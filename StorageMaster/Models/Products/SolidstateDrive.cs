using System;
using System.Collections.Generic;
using System.Text;

namespace StorageMaster
{
    //Assigning as child class of Product
    public class SolidStateDrive:Product
    {
        //initialising SolidStateDrive class
        public SolidStateDrive()
        {
            //assigning weight to SolidStateDrive unit
            this.Weight = 0.2;
        }
    }
}