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
    
    public partial class Пользователь
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Пользователь()
        {
            this.Пользователь_Роль = new HashSet<Пользователь_Роль>();
        }
    
        public int ID_Пользователя { get; set; }
        public string Логин { get; set; }
        public byte[] Пароль_Hash { get; set; }
        public byte[] Пароль_Salt { get; set; }
        public System.DateTime Дата_Создания { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Пользователь_Роль> Пользователь_Роль { get; set; }
    }
}
