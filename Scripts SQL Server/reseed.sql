delete from Ayuda where IdAyuda > 100;

DBCC CHECKIDENT ('Ayuda', RESEED, 100);

select * from Ayuda;