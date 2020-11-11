using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace StorageMaster
{
    class Storage
    {
        private string name;     // storage's name
        private int capacity;    // maximum weight of products the storage can handle
        private int weight;         // weight used in the storage
        private int garageSlots; // number of slots available in the storage's garage
        private Vehicle[] vehicles;  // vehicles in the storage
        private List<Product> products;  // products in the storage

        public string Name { get { return this.name; } }
        public int Capacity { get { return this.capacity; } }
        public int GarageSlots { get { return this.garageSlots; } }

        public int Weight { get { return this.weight; } }

        public bool IsFull()
        {
            if (weight >= capacity) // si full in case the weight is equal then the capacity. >= is used to be safe
                return true;
            else
                return false;
        }
        public IReadOnlyCollection<Vehicle> Garage() // return all vehicles
        {
            return this.vehicles;
        }
        public IReadOnlyCollection<Product> Products() // return all products
        {
            return this.products;
        }

        public Storage(string name, int capacity, int garageSlots, params Vehicle[] vehicles) // constructor
        {
            this.name = name;
            this.capacity = capacity;
            this.garageSlots = garageSlots;

            this.vehicles = new Vehicle[capacity];
            for(int i = 0; i < vehicles.Length; i++) // pupulates the vehicles internal array with the comming vehicles thru the constructor
            {
                this.vehicles[i] = vehicles[i];
            }

            products = new List<Product>(); // the warehouse is inicialized with no products inside.
        }

        public Vehicle GetVehicle(int garageSlot) // returns the vehicle from a specific slot
        {
            if ((garageSlot < 0) || (garageSlot >= this.garageSlots)) // out of range
            {
                throw new InvalidOperationException("Invalid garage slot!");
            }
            if (vehicles.ElementAt<Vehicle>(garageSlot) == null) // there is no vehicle in that slot
            {
                throw new InvalidOperationException("No vehicle in this garage slot!");
            }
            return vehicles.ElementAt<Vehicle>(garageSlot); // returns the vehicle
        }

        private int FreeSlot() // private method that search an available slot for a vehicle
        {
            for(int i = 0; i < this.capacity; i++)
            {
                if (vehicles.ElementAt<Vehicle>(i) == null)
                    return i;
            }
            return -1; // no slot available
        }

        private void AddVehicle(Vehicle vehicle,int slot) // adds a vehicle to a specific slot
        {
            this.vehicles[slot] = vehicle;
        }

        public int SendVehicleTo(int garageSlot, Storage deliveryLocation) // sends a vehicle to deliveryLocation
        {
            Vehicle vehicle;
            try
            {
                vehicle = this.GetVehicle(garageSlot); // trys to get a vehicle from a specific slot
            }
            catch
            {
                throw;
            }

            int freeSlot = deliveryLocation.FreeSlot(); // check if there is a slot available in the delivery location
            if (freeSlot < 0)
            {
                throw new InvalidOperationException("No room in garage!");
            }

            this.vehicles[garageSlot] = null;               //takes the vehicle out of the garage (it has been saved in vehicle)
            deliveryLocation.AddVehicle(vehicle, freeSlot); //adds it to the delivery location
            return freeSlot;                                //returns the slot that is now taking at the new location 
        }

        public int UnloadVehicle(int garageSlot) //unload the goods from a vehicle located in a specific garage slot
        {
            double totalWeight = 0;

            Vehicle currentVehicle = this.vehicles[garageSlot]; // get the vehicle
            if (currentVehicle == null) // if the vehicle doesn't exists
            {
                throw new InvalidOperationException("No vehicle in that slot");
            }

            foreach (Product product in currentVehicle.Trunk) // goes thru all products
            {
                totalWeight += product.Weight; // add the weight of each product to the total weight of that specific freight
            }
            if (this.capacity == this.weight) // if there is no capacity
            {
                throw new InvalidOperationException("Storage is full!");
            }
            if(totalWeight+(double)this.weight > (double)this.capacity) // if the vehicle has more than the storage can handle
            {
                throw new InvalidOperationException("Storage is full!");
            }

            int totalUnloadedProducts = 0;
            while (!currentVehicle.IsEmpty()) // goes thru all products in the vehicle
            {
                Product unloadedProduct = currentVehicle.Unload(); // unload the product
                this.products.Append<Product>(unloadedProduct); // add it to storage
                totalUnloadedProducts++; // increments the amount of products moved from the vehicle to the storage
            }
            return totalUnloadedProducts; // returns the total amount of products moved to the storage
        }
    }
}
