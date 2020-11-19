using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGTviewer
{
    //정규화
    public class Elaboration
    {
        public void CalTotalElaboration()
        {
            //임시 테스트 용
            float avg = 5;
            int ElabSum = 0;
            int psv = 0;

            /*if (strokeList.Count > 5)
            {
                ElabSum++;
            }*/

            if (ElabSum == 1)
            {
                psv = 1;
            }
            else if (ElabSum == 2)
            {
                psv = 7;

            }
            else if (ElabSum >= 3)
            {
                psv = 10;
            }
            else if (ElabSum == 0)
            {
                psv = 1;
            }
        }
    }
}
