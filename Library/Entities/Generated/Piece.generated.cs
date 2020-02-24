//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
//
//     Produced by Entity Framework Visual Editor v1.3.0.12
//     Source:                    https://github.com/msawczyn/EFDesigner
//     Visual Studio Marketplace: https://marketplace.visualstudio.com/items?itemName=michaelsawczyn.EFDesigner
//     Documentation:             https://msawczyn.github.io/EFDesigner/
//     License (MIT):             https://github.com/msawczyn/EFDesigner/blob/master/LICENSE
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Zebra.StandardLibrary
{
   /// <summary>
   /// Represents a Piece Entity
   /// </summary>
   public partial class Piece
   {
      partial void Init();

      /// <summary>
      /// Default constructor. Protected due to required properties, but present because EF needs it.
      /// </summary>
      protected Piece()
      {
         Sheet = new System.Collections.Generic.List<global::Zebra.StandardLibrary.Sheet>();
         SetlistItem = new System.Collections.Generic.List<global::Zebra.StandardLibrary.SetlistItem>();

         Init();
      }

      /// <summary>
      /// Public constructor with required data
      /// </summary>
      /// <param name="name">Name of the Piece</param>
      public Piece(string name)
      {
         if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
         this.Name = name;

         this.Sheet = new System.Collections.Generic.List<global::Zebra.StandardLibrary.Sheet>();
         this.SetlistItem = new System.Collections.Generic.List<global::Zebra.StandardLibrary.SetlistItem>();

         Init();
      }

      /// <summary>
      /// Static create function (for use in LINQ queries, etc.)
      /// </summary>
      /// <param name="name">Name of the Piece</param>
      public static Piece Create(string name)
      {
         return new Piece(name);
      }

      /*************************************************************************
       * Properties
       *************************************************************************/

      /// <summary>
      /// Identity, Indexed, Required
      /// ID of the Piece Entity
      /// </summary>
      [Key]
      [Required]
      public int PieceID { get; protected set; }

      /// <summary>
      /// Backing field for Name
      /// </summary>
      protected string _Name;
      /// <summary>
      /// When provided in a partial class, allows value of Name to be changed before setting.
      /// </summary>
      partial void SetName(string oldValue, ref string newValue);
      /// <summary>
      /// When provided in a partial class, allows value of Name to be changed before returning.
      /// </summary>
      partial void GetName(ref string result);

      /// <summary>
      /// Required, Max length = 35
      /// Name of the Piece
      /// </summary>
      [Required]
      [MaxLength(35)]
      [StringLength(35)]
      public string Name
      {
         get
         {
            string value = _Name;
            GetName(ref value);
            return (_Name = value);
         }
         set
         {
            string oldValue = _Name;
            SetName(oldValue, ref value);
            if (oldValue != value)
            {
               _Name = value;
            }
         }
      }

      /// <summary>
      /// Backing field for Arranger
      /// </summary>
      protected string _Arranger;
      /// <summary>
      /// When provided in a partial class, allows value of Arranger to be changed before setting.
      /// </summary>
      partial void SetArranger(string oldValue, ref string newValue);
      /// <summary>
      /// When provided in a partial class, allows value of Arranger to be changed before returning.
      /// </summary>
      partial void GetArranger(ref string result);

      /// <summary>
      /// Max length = 35
      /// Name of the Arranger
      /// </summary>
      [MaxLength(35)]
      [StringLength(35)]
      public string Arranger
      {
         get
         {
            string value = _Arranger;
            GetArranger(ref value);
            return (_Arranger = value);
         }
         set
         {
            string oldValue = _Arranger;
            SetArranger(oldValue, ref value);
            if (oldValue != value)
            {
               _Arranger = value;
            }
         }
      }

      /*************************************************************************
       * Navigation properties
       *************************************************************************/

      /// <summary>
      /// One Piece has many Sheets
      /// </summary>
      public virtual ICollection<global::Zebra.StandardLibrary.Sheet> Sheet { get; protected set; }

      /// <summary>
      /// One Piece has multiple SetlistItems
      /// </summary>
      public virtual ICollection<global::Zebra.StandardLibrary.SetlistItem> SetlistItem { get; protected set; }

   }
}
