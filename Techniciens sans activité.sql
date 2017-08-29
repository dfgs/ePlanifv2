Update EtraliCommon.dbo.Technicien set Valide=0 where NumTech in
(
select Technicien.NumTech
from 
EtraliCommon.dbo.Technicien as Technicien left join 
(select * from ePlanif.dbo.Activite where Annee=2017) as Activite on  Technicien.NumTech=Activite.NumTech
where NumActivite is null and Technicien.Valide=1
)
