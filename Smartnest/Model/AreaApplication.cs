//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Smartnest.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class AreaApplication
    {
        public int ID { get; set; }
        public int AreaID { get; set; }
        public int ApplicationID { get; set; }
    
        public virtual Application Application { get; set; }
        public virtual Area Area { get; set; }
    }
}
