# Zebra2

Zebra Sheet Music Management is an Application for Archiving printed Collections of Sheet Music for Orchestras and Bands. It is designed for automatically porting the printed Sheet Music to a Database via Barcode Recognition. All printed Sheets will be stored as PDF Files and can later be displayed and/or printed again from the Application.

##Idea
Most bands and orchestras still distribute their sheet music in printed form, wich causes a lot of effort for keeping the original sheets in a safe place, making enough copies for the whole orchestra etc. Also, over time many orchestras and bands collect a huge amount of printed sheet music, which all has to be stored in physical archives that have to be kept up to date. The idea of Zebra is to create a digital Sheet Music Archive where all sheets can be found and printed quickly and easily without having to search all these old big archive cabinets.

##Model
The base model comprises the 3 Entities Piece, Sheet and Part.
When you buy sheet music for an orchestra, you buy the notes for a specific piece, which can be e.g. Toto-Africa. This specific Piece you buy comes with notes for specific Parts, which are e.g. the score, Flute, Trumpet, Violin etc. Thus, a Sheet is the printed copy of a specific Part for a specific Piece, e.g. 1st Trumpet for Toto-Africa, which in the end will be on the musician's stand.

##Specs
The Application is build on .NET Core 3.1 and uses Entity Framework Core for Database access. The main idea is to create a console application for testing and administration and a Windows Forms or WPF application for a fancy UI. Later, the database shall be made accessible by an Android/iOS App and/or a ASP.NET Web App.
