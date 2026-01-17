select * from ayuda where estado = 'Rechazada'

DELETE FROM Ayuda 
WHERE IdAyuda IN (

SELECT
A.IdAyuda
    FROM Ayuda AS A
    WHERE A.Estado = 'Rechazada'
    AND EXISTS (
        SELECT 1 
        FROM OPENJSON(A.ListaItems)
        WITH (CantidadEntregada decimal '$.CantidadEntregada') AS j
        WHERE j.CantidadEntregada > 0
    ));



SELECT 
    IdAyuda, j.IdItem, j.Nombre, SUM(j.CantidadEntregada)
FROM Ayuda as A
cross apply openjson(A.ListaItems)
with(
    IdItem int '$.ItemLista.IdItem',
    Nombre VarChar(75) '$.ItemLista.Nombre',
    CantidadSolicitada decimal '$.CantidadSolicitada',
    CantidadEntregada decimal '$.CantidadEntregada'
    )
as j
where
A.Estado = 'Rechazada'
group by a.IdAyuda, j.IdItem, j.Nombre
Having SUM(j.CantidadEntregada) > 0
order by a.IdAyuda DESC
