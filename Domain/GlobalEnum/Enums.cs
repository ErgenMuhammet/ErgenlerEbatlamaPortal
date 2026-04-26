
using System.ComponentModel;


namespace Domain.GlobalEnum
{
    [Flags]
   public enum Category
        {
            [Description("Marangoz")]
            Carpenter = 1,

            [Description("Montajcı")]
            Assembler = 2,

            [Description("Ebatlamacı")]
            PanelSawyer = 4
        }
   }

