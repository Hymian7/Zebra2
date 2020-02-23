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

namespace Zebra.Library
{
   /// <summary>
   /// Helper Entity to model the N:M relationship between Setlist and Piece
   /// </summary>
   public partial class SetlistItem: ITimestamps
   {
      partial void Init();

      /// <summary>
      /// Default constructor. Protected due to required properties, but present because EF needs it.
      /// </summary>
      protected SetlistItem()
      {
         Init();
      }

      /// <summary>
      /// Public constructor with required data
      /// </summary>
      /// <param name="piece"></param>
      /// <param name="setlist"></param>
      public SetlistItem(global::Zebra.Library.Piece piece, global::Zebra.Library.Setlist setlist)
      {
         if (piece == null) throw new ArgumentNullException(nameof(piece));
         this.Piece = piece;

         if (setlist == null) throw new ArgumentNullException(nameof(setlist));
         this.Setlist = setlist;


         Init();
      }

      /// <summary>
      /// Static create function (for use in LINQ queries, etc.)
      /// </summary>
      /// <param name="piece"></param>
      /// <param name="setlist"></param>
      public static SetlistItem Create(global::Zebra.Library.Piece piece, global::Zebra.Library.Setlist setlist)
      {
         return new SetlistItem(piece, setlist);
      }

      /*************************************************************************
       * Properties
       *************************************************************************/

      /// <summary>
      /// Identity, Indexed, Required
      /// Helper Entity to model a N:M relationship betweet Setlist and Piece
      /// </summary>
      [Key]
      [Required]
      public int SetlistItemID { get; protected set; }

      /// <summary>
      /// DateTime of Entity Creation
      /// </summary>
      public DateTime? CreationDate { get; set; }

      /// <summary>
      /// DateTime of Last Update
      /// </summary>
      public DateTime? LastUpdate { get; set; }

      /*************************************************************************
       * Navigation properties
       *************************************************************************/

      /// <summary>
      /// Required{br/}
      /// One SetlistItem belongs to one Piece
      /// </summary>
      public virtual global::Zebra.Library.Piece Piece { get; set; }

      /// <summary>
      /// Required{br/}
      /// One Setlist has many Setlist Items
      /// </summary>
      public virtual global::Zebra.Library.Setlist Setlist { get; set; }

   }
}

