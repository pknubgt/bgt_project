using System;
using System.Collections.Generic;
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
        [NotNull]
        public int PointNum_A { get; set; }

        [Column("PointNum_1")]
        [NotNull]
        public int PointNum_1 { get; set; }

        [Column("PointNum_2")]
        [NotNull]
        public int PointNum_2 { get; set; }

        [Column("PointNum_3")]
        [NotNull]
        public int PointNum_3 { get; set; }

        [Column("PointNum_4")]
        [NotNull]
        public int PointNum_4 { get; set; }

        [Column("PointNum_5")]
        [NotNull]
        public int PointNum_5 { get; set; }

        [Column("PointNum_6")]
        [NotNull]
        public int PointNum_6 { get; set; }

        [Column("PointNum_7")]
        [NotNull]
        public int PointNum_7 { get; set; }

        [Column("PointNum_8")]
        [NotNull]
        public int PointNum_8 { get; set; }

        [Column("totalpressure_A")]
        [NotNull]
        public float TotalPressure_A { get; set; }

        [Column("totalpressure_1")]
        [NotNull]
        public float TotalPressure_1 { get; set; }

        [Column("totalpressure_2")]
        [NotNull]
        public float TotalPressure_2 { get; set; }

        [Column("totalpressure_3")]
        [NotNull]
        public float TotalPressure_3 { get; set; }

        [Column("totalpressure_4")]
        [NotNull]
        public float TotalPressure_4 { get; set; }

        [Column("totalpressure_5")]
        [NotNull]
        public float TotalPressure_5 { get; set; }

        [Column("totalpressure_6")]
        [NotNull]
        public float TotalPressure_6 { get; set; }

        [Column("totalpressure_7")]
        [NotNull]
        public float TotalPressure_7 { get; set; }

        [Column("totalpressure_8")]
        [NotNull]
        public float TotalPressure_8 { get; set; }

        
        [Column("partpressure")]
        [NotNull]
        public float PartPressure_6 { get; set; }
    }
}
