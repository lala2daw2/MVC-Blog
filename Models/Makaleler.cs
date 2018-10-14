//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KuvarBlog.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Makaleler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Makaleler()
        {
            this.Yorumlars = new HashSet<Yorumlar>();
        }
    
        public int ID { get; set; }
        public Nullable<int> KullaniciID { get; set; }
        public Nullable<int> KategoriID { get; set; }
        [Required(ErrorMessage ="Başlık boş geçilemez"),StringLength(49,ErrorMessage ="Bu alan en fazla 49 karakter olabilir.")]
        public string Baslik { get; set; }
        [Required(ErrorMessage ="İçerik boş geçilemez.")]
        public string İcerik { get; set; }
        [DataType(DataType.Upload)]
        public string Foto { get; set; }
        public Nullable<System.DateTime> Tarih { get; set; }
    
        public virtual Kategoriler Kategoriler { get; set; }
        public virtual Kullanicilar Kullanicilar { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Yorumlar> Yorumlars { get; set; }
    }
}
