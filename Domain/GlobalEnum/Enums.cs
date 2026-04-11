
using System.ComponentModel;


namespace Domain.GlobalEnum
{
   public enum Category
        {
            [Description("Marangoz")]
            Carpenter = 0,

            [Description("Montajcı")]
            Assembler = 1,

            [Description("Ebatlamacı")]
            PanelSawyer = 2,

            //[Description("Glue")]
            //Glue = 3,

            //[Description("Scraps")]
            //Scraps = 4,

            //[Description("Customer")]
            //Customer = 5,

            //[Description("Invoice")]
            //Invoice = 6,

            //[Description("Payments")]
            //Payments = 7,

            //[Description("Collections")]
            //Collections = 8,

        }
   }

