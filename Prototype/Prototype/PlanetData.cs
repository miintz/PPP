using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype
{
    class PlanetData
    {
        public int Index { get; set; }
        public int Length { get; set; }

        private String Name { get; set; }

        private DateTime[] Dates { get; set; }

        private DateTime[] RiseAzDate { get; set; }

        private UInt32[] RizeAzAngle { get; set; }

        private DateTime[] TransitAltDate { get; set; }

        private UInt32[] TransitAltAngle { get; set; }

        private DateTime[] SetAzDate { get; set; }

        private UInt32[] SetAzAngle { get; set; }

        public PlanetData(String name, Int32 size)
        {
            this.Name = name;
            this.Dates = new DateTime[size];
            this.RiseAzDate = new DateTime[size];
            this.RizeAzAngle = new UInt32[size];
            this.TransitAltDate = new DateTime[size];
            this.TransitAltAngle = new UInt32[size];
            this.SetAzDate = new DateTime[size];
            this.SetAzAngle = new UInt32[size];

            this.Length = size;
        }

        public DateTime getDate()
        {
            return this.Dates[this.Index];
        }

        public DateTime getRiseAzDate()
        {
            return this.RiseAzDate[this.Index];
        }

        public UInt32 getRizeAzAngle()
        {
            return this.RizeAzAngle[this.Index];
        }

        public DateTime getTransitAltDate()
        {
            return this.TransitAltDate[this.Index];
        }

        public UInt32 getTransitAltAngle()
        {
            return this.TransitAltAngle[this.Index];
        }

        public DateTime getSetAzDate()
        {
            return this.SetAzDate[this.Index];
        }

        public UInt32 getAzAngle()
        {
            return this.SetAzAngle[this.Index];
        }


        public void setDate(DateTime d)
        {
            this.Dates[this.Index] = d;
        }

        public void setRiseAzDate(DateTime d)
        {
            this.RiseAzDate[this.Index] = d;
        }

        public void setRizeAzAngle(UInt32 i)
        {
            this.RizeAzAngle[this.Index] = i;
        }

        public void setTransitAltDate(DateTime d)
        {
            this.TransitAltDate[this.Index] = d;
        }

        public void setTransitAltAngle(UInt32 i)
        {
            this.TransitAltAngle[this.Index] = i;
        }

        public void setSetAzDate(DateTime d)
        {
            this.SetAzDate[this.Index] = d;
        }

        public void setSetAzAngle(UInt32 i)
        {
            this.SetAzAngle[this.Index] = i;
        }

    }
}
