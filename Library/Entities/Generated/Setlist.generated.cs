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
   /// Represents a Setlist Entity
   /// </summary>
   public partial class Setlist
   {
      partial void Init();

      /// <summary>
      /// Default constructor
      /// </summary>
      public Setlist()
      {
         SetlistItem = new System.Collections.Generic.List<global::Zebra.StandardLibrary.SetlistItem>();

         Init();
      }

      /*************************************************************************
       * Properties
       *************************************************************************/

      /// <summary>
      /// Identity, Indexed, Required
      /// ID of the Setlist Entity
      /// </summary>
      [Key]
      [Required]
      public int SetlistID { get; protected set; }

      /*************************************************************************
       * Navigation properties
       *************************************************************************/

      /// <summary>
      /// One SetlistItem belongs to one Setlist
      /// </summary>
      public virtual ICollection<global::Zebra.StandardLibrary.SetlistItem> SetlistItem { get; protected set; }

   }
}

