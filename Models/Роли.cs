//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PrimeTrack.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Роли
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Роли()
        {
            this.Пользователь_Роль = new HashSet<Пользователь_Роль>();
        }
    
        public int ID_Роли { get; set; }
        public string Название_Роли { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Пользователь_Роль> Пользователь_Роль { get; set; }
    }
}
