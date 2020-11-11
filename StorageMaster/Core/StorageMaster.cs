using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageMaster.Core
{
    class StorageMaster
    {
        private List<Storage> storages;     // the storage registry list
        private List<Product> productPool; //the product list in the pool of a storage


        public string AddProduct(string type, double price)
        {
            Product newProduct; //creating new Product

            if (type == "Ram")
            {
                newProduct = new Ram();
                newProduct.Price = price;
                productPool.Add(newProduct);
                return $"Added {type} to pool";
            }
            else if (type == "Gpu")
            {
                newProduct = new Gpu();
                newProduct.Price = price;
                productPool.Add(newProduct);
                return $"Added {type} to pool";
            }
            else if (type == "HardDrive")
            {
                newProduct = new HardDrive();
                newProduct.Price = price;
                productPool.Add(newProduct);
                return $"Added {type} to pool";
            }
            else if (type == "SolidstateDrive")
            {
                newProduct = new SolidStateDrive();
                newProduct.Price = price;
                productPool.Add(newProduct);
                return $"Added {type} to pool";
            }
            throw new NotImplementedException("Invalid product type!");
        }

        public string RegisterStorage(string type, string name)
        {
            Storage newStorage;
            if (type == "AutomatedWarehouse")
            {
                newStorage = new AutomatedWarehouse(name);
                storages.Add(newStorage);
                return $"Registred {name}";
            }
            else if (type == "DistributionCenter")
            {
                newStorage = new DistributionCenter(name);
                storages.Add(newStorage);
                return $"Registred {name}";
            }
            else if (type == "Warehouse")
            {
                newStorage = new Warehouse(name);
                storages.Add(newStorage);
                return $"Registred {name}";
            }
            throw new NotImplementedException("Invalide storage type!");

        }

        public string SelectVehicle(string storageName, int garageSlot)
        {
            throw new NotImplementedException();
        }

        public string LoadVehicle(IEnumerable<string> productNames)
        {
            throw new NotImplementedException();
        }

        public string SendVehicleTo(string sourceName, int sourceGarageSlot, string destinationName)
        {
            throw new NotImplementedException();
        }

        public string UnloadVehicle(string storageName, int garageSlot)
        {
            for(int i = 0; i < storages.Count; i++) // goes thru the list of all storages
            {
                if (storageName == storages.ElementAt<Storage>(i).Name) // checks the one with the right name
                {
                    int unloadedProductsCount = storages.ElementAt<Storage>(i).UnloadVehicle(garageSlot);     // unloads products from vehicle and gets the total count
                    int productsInVehicle = 0;                                                                // <-- this will need to be checked
                    return $"Unloaded {unloadedProductsCount}/{productsInVehicle} products at {storageName}"; // returns the string with information
                }
            }
            return $"nothing unloaded at {storageName}"; // in case that there has been nothing unloaded
        }

        public string GetStorageStatus(string storageName)
        {
            string result = "";

            for (int i = 0; i < storages.Count; i++) // goes thru the list of all storages
            {
                if (storageName == storages.ElementAt<Storage>(i).Name) // checks the one with the right name
                {
                    Storage storage = storages.ElementAt<Storage>(i); // gets a reference to the storage

                    result = $"Stock ({storage.Weight}/{storage.Capacity}) ["; // gets the total weight and capacity and put it into result

                    //This section is ment to get a list of products and count them
                    List<Product> products = storage.Products().ToList<Product>();
                    var prod =
                        from product in products
                        group product by product.GetType() into productType // MISSING ORDER BY Type
                        select new
                        {
                            type = productType.Key,
                            count = productType.Count()
                        };

                    int j;
                    for (j = 0; j < prod.Count()-1; j++)
                    {
                        result = result + prod.ElementAt(j).type + " " + prod.ElementAt(j).count + ", ";
                    }
                    result = result + prod.ElementAt(j).type + " " + prod.ElementAt(j).count + "]\n"; // the last element goes without a ,

                    result = result + "Garage: [";
                    List<Vehicle> garage = new List<Vehicle>(storage.Garage());
                    for (j = 0; j < garage.Count; j++)
                    {
                        Vehicle vehicle = garage.ElementAt<Vehicle>(j);
                        if (vehicle != null)
                            result = result + vehicle.GetType();
                        else
                            result = result + "empty";
                        if (j < garage.Count - 1) // the last element isn't followed by a pipe
                            result = result + "|";
                    }
                    result = result + "]";

                    break; // comes out of the for, after processing the found storage
                }
            }

            return result;
        }

        public string GetSummary()
        {
            string result = "";

            foreach (Storage storage in storages) // goes thru all storages
            {
                double totalMoney = 0;
                foreach(Product product in storage.Products())
                    totalMoney += product.Price;               // counts the total worth

                result = result + $"{storage.Name}:\nStorage worth: ${totalMoney:F2}\n";
            }

            return result;
        }

    }
}
