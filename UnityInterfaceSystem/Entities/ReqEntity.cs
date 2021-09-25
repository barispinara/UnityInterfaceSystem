using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityInterfaceSystem.Entities
{
    public class ReqEntity
    {
        public int order_number { get; set; }
        public string Main_Piece { get; set; }
        public string Req_Piece { get; set; }

        public ReqEntity(int order_NUMBER, string main_piece, string req_piece)
        {
            this.order_number = order_NUMBER;
            this.Main_Piece = main_piece;
            this.Req_Piece = req_piece;
        }
    }
}
