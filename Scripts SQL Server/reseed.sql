delete from Ayuda where IdAyuda > 50;

DBCC CHECKIDENT ('Ayuda', RESEED, 50);

select * from Ayuda;