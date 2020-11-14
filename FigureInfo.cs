using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace BGTviewer
{
    [Table("FigureInfo")]
    public class FigureInfo
    {
        
        [Column("id")]
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [Column("pointsnum")]
        public int PointNum_A { get; set; }

        [Column("PointNum_1")]
        public int PointNum_1 { get; set; }

        [Column("PointNum_2")]
        public int PointNum_2 { get; set; }

        [Column("PointNum_3")]
        public int PointNum_3 { get; set; }

        [Column("PointNum_4")]
        public int PointNum_4 { get; set; }

        [Column("PointNum_5")]
        public int PointNum_5 { get; set; }

        [Column("PointNum_6")]
        public int PointNum_6 { get; set; }

        [Column("PointNum_7")]
        public int PointNum_7 { get; set; }

        [Column("PointNum_8")]
        public int PointNum_8 { get; set; }

        [Column("totalpressure_A")]
        public float TotalPressure_A { get; set; }

        [Column("totalpressure_1")]
        public float TotalPressure_1 { get; set; }

        [Column("totalpressure_2")]
        public float TotalPressure_2 { get; set; }

        [Column("totalpressure_3")]
        public float TotalPressure_3 { get; set; }

        [Column("totalpressure_4")]
        public float TotalPressure_4 { get; set; }

        [Column("totalpressure_5")]
        public float TotalPressure_5 { get; set; }

        [Column("totalpressure_6")]
        public float TotalPressure_6 { get; set; }

        [Column("totalpressure_7")]
        public float TotalPressure_7 { get; set; }

        [Column("totalpressure_8")]
        public float TotalPressure_8 { get; set; }


        [Column("partpressure")]
        public float PartPressure_6 { get; set; }
    }
}