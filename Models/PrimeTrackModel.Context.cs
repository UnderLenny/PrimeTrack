﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class kpEntities : DbContext
    {
        public kpEntities()
            : base("name=kpEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Клиент> Клиент { get; set; }
        public virtual DbSet<Партия> Партия { get; set; }
        public virtual DbSet<Пользователь> Пользователь { get; set; }
        public virtual DbSet<Продукт> Продукт { get; set; }
        public virtual DbSet<Продукт_Партия> Продукт_Партия { get; set; }
        public virtual DbSet<Роли> Роли { get; set; }
        public virtual DbSet<Склад> Склад { get; set; }
    }
}
