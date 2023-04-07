

namespace Kernel.Enums
{
    public class GeographicalRegion : Enumeration
    {
        public static GeographicalRegion Africa = new GeographicalRegion(id: 1, displayName: "Africa");
        public static GeographicalRegion SouthernAfrica = new GeographicalRegion(id: 2, displayName: "Southern Africa");
        public static GeographicalRegion SubSaharanAfrica = new GeographicalRegion(id: 3, displayName: "Sub- Saharan Africa");
        public static GeographicalRegion NorthernAfrica = new GeographicalRegion(id: 4, displayName: "Northern Africa");
        public static GeographicalRegion Europe = new GeographicalRegion(id: 5, displayName: "Europe");
        public static GeographicalRegion NorthernAmerica = new GeographicalRegion(id: 6, displayName: "Northern Americas");
        public static GeographicalRegion SouthernAmerica = new GeographicalRegion(id: 7, displayName: "Southern Americas");
        public static GeographicalRegion MiddleEast = new GeographicalRegion(id: 8, displayName: "Middle East");
        public static GeographicalRegion Asia = new GeographicalRegion(id: 9, displayName: "Asia");
        public static GeographicalRegion Oceania = new GeographicalRegion(id: 10, displayName: "Oceania (Australia, New Zealand etc.)");

        public GeographicalRegion()
        {           
        }
        private GeographicalRegion(int id, string displayName) : base(id, displayName)
        {            
        }
    }
}
