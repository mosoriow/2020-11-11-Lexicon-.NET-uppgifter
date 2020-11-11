using System;
using System.Collections.Generic;
using System.Text;

namespace StorageMaster
{
    //Assigning as child class of Product
    public class HardDrive:Product
    {
        //initialising Harddrive class
        public HardDrive()
        {
            //assigning weight to HadDrive
            this.Weight = 1;
        }
    }
}