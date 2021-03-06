USE [STOCKDEP]
GO
/****** Object:  StoredProcedure [dbo].[AE_SP001_StockDepreciation]    Script Date: 5/12/2014 5:39:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--delete [@AE_STKDEP_REG] 

--[AE_SP001_StockDepreciation]'20141204'

ALTER PROCEDURE  [dbo].[AE_SP001_StockDepreciation] 
	@DateFrom			DateTime	
AS

BEGIN

DECLARE @LocCurr VARCHAR(4)
DECLARE @CompName NVARCHAR(MAX)
DECLARE @Currency VARCHAR(10)
Declare @CoRegNo NVARCHAR(40)
Declare @GSTRegNo NVARCHAR(40)
DECLARE @AgeBy INT /*0-Group By Document Date, 1-Group By Due Date */

---------------------------------
/* Table Creation */


IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
           WHERE TABLE_NAME = N'StockDep')
drop table StockDep

Create Table StockDep (

    [SNo] int identity(1,1),
    [Company] [nvarchar](200) NOT NULL,
	[ItemGroup] [nvarchar](50) NOT NULL,
	[Column1] [Decimal](19,4) NULL,
	[Column2] [Decimal](19,4) NULL,
	[Column3] [Decimal](19,4) NULL,
	[Column4] [Decimal](19,4) NULL,
	[Column5] [Decimal](19,4) NULL,
	[Column6] [Decimal](19,4) NULL,
	[Column7] [Decimal](19,4) NULL,
	[Depreciation1] [Decimal](19,4) NULL,
	[Depreciation2] [Decimal](19,4) NULL,
	[Depreciation3] [Decimal](19,4) NULL,
    [Depreciation4] [Decimal](19,4) NULL,
    [Depreciation5] [Decimal](19,4) NULL,
	[Depreciation6] [Decimal](19,4) NULL,
	[Depreciation7] [Decimal](19,4) NULL,
	[TotalDepreciation] [Decimal](19,4) NULL,
	[Header1] [nvarchar](100) NULL,
	[Header2] [nvarchar](100) NULL,
	[Header3] [nvarchar](100) NULL,
	[Header4] [nvarchar](100) NULL,
	[Header5] [nvarchar](100) NULL,
	[Header6] [nvarchar](100) NULL,
	[Header7] [nvarchar](100) NULL
	)

--------------------------------------



DECLARE @AgeColumn1 INT, @AgeColumn2 INT, @AgeColumn3 INT, @AgeColumn4 INT, @AgeColumn5 INT, 
@AgeColumn6 INT, @AgeColumn7 INT, @AgeColumn8 INT, @AgeColumn9 INT, @COUNTColumns INT

DECLARE @ColumnHeading1 nvarchar(100), @ColumnHeading2 nvarchar(100), @ColumnHeading3 nvarchar(100)
, @ColumnHeading4 nvarchar(100), @ColumnHeading5 nvarchar(100),
@ColumnHeading6 nvarchar(100), @ColumnHeading7 nvarchar(100)

SELECT @AgeColumn1 = isnull(substring(cast(T0.[U_Col_0] as varchar),charindex('-',T0.[U_Col_0])+1,len(T0.[U_Col_0])-charindex('-',T0.[U_Col_0])),0)
 FROM [dbo].[@AE_DEPN_MATRIX]  T0 WHERE T0.[Code] = 1

 SELECT @AgeColumn2 = isnull(substring(cast(T0.[U_Col_1] as varchar),charindex('-',T0.[U_Col_1])+1,len(T0.[U_Col_1])-charindex('-',T0.[U_Col_1])),0)
 FROM [dbo].[@AE_DEPN_MATRIX]  T0 WHERE T0.[Code] = 1

 SELECT @AgeColumn3 = isnull(substring(cast(T0.[U_Col_2] as varchar),charindex('-',T0.[U_Col_2])+1,len(T0.[U_Col_2])-charindex('-',T0.[U_Col_2])),0)
 FROM [dbo].[@AE_DEPN_MATRIX]  T0 WHERE T0.[Code] = 1

 SELECT @AgeColumn4 = isnull(substring(cast(T0.[U_Col_3] as varchar),charindex('-',T0.[U_Col_3])+1,len(T0.[U_Col_3])-charindex('-',T0.[U_Col_3])),0)
 FROM [dbo].[@AE_DEPN_MATRIX]  T0 WHERE T0.[Code] = 1

 SELECT @AgeColumn5 = isnull(substring(cast(T0.[U_Col_4] as varchar),charindex('-',T0.[U_Col_4])+1,len(T0.[U_Col_4])-charindex('-',T0.[U_Col_4])),0)
 FROM [dbo].[@AE_DEPN_MATRIX]  T0 WHERE T0.[Code] = 1

 SELECT @AgeColumn6 = isnull(substring(cast(T0.[U_Col_5] as varchar),charindex('-',T0.[U_Col_5])+1,len(T0.[U_Col_5])-charindex('-',T0.[U_Col_5])),0)
 FROM [dbo].[@AE_DEPN_MATRIX]  T0 WHERE T0.[Code] = 1

 SELECT @AgeColumn7 = isnull(substring(cast(T0.[U_Col_6] as varchar),charindex('-',T0.[U_Col_6])+1,len(T0.[U_Col_6])-charindex('-',T0.[U_Col_6])),0)
 FROM [dbo].[@AE_DEPN_MATRIX]  T0 WHERE T0.[Code] = 1

SET @AgeBy=0
SET @COUNTColumns =0

set @ColumnHeading1 = '1 To ' + cast(@AgeColumn1 as varchar(20))
set @ColumnHeading2 = cast((@AgeColumn1+1) as varchar(20)) + ' - ' + cast((@AgeColumn2) as varchar(20))
set @ColumnHeading3 = cast((@AgeColumn2+1) as varchar(20)) + ' - ' + cast((@AgeColumn3) as varchar(20))
set @ColumnHeading4 = cast((@AgeColumn3+1) as varchar(20)) + ' - ' + cast((@AgeColumn4) as varchar(20))
set @ColumnHeading5 = cast((@AgeColumn4+1) as varchar(20)) + ' - ' + cast((@AgeColumn5) as varchar(20)) 
set @ColumnHeading6 = cast((@AgeColumn5+1) as varchar(20)) + ' - ' + cast((@AgeColumn6) as varchar(20))

if isnull(@AgeColumn7,0) = 0 
begin
set @ColumnHeading7 = ' > ' + cast(@AgeColumn6 + 1 as varchar(20))
set @AgeColumn7 = @AgeColumn6 + 1
end
else
begin
set @ColumnHeading7 = ' > ' + cast(@AgeColumn7 as varchar(20))
end
 
/* Retrieve Company Info */        
SELECT Top 1 @LocCurr = MainCurncy
			,@CoRegNo = TaxIdNum
			,@GSTRegNo = TaxIdNum2 
			,@CompName = ISNULL(OADM.PrintHeadr,OADM.CompnyName)
FROM OADM

/* Get Balance Stock Value form Warehouse Journal */


SELECT T0.[ItemCode], T0.[Warehouse],T2.[ItmsGrpNam], datediff(dd,T0.[DocDate], @DateFrom) as 'AgeBy' ,
(sum(Inqty) - sum(OutQty )) as [Balance Stock], (sum(Inqty) - sum(OutQty )) * T0.[CalcPrice]  as [Balance Value] ,

case when datediff(dd,T0.[DocDate], @DateFrom) >=0 and datediff(dd,T0.[DocDate], @DateFrom) <= @AgeColumn1 then isnull((sum(Inqty) - sum(OutQty )) * T0.[CalcPrice],0) end [ColumnHeading1],
case when datediff(dd,T0.[DocDate], @DateFrom) >=0 and datediff(dd,T0.[DocDate], @DateFrom) <= @AgeColumn1 then 
isnull((SELECT T0.[U_Col_0] FROM [dbo].[@AE_DEPN_MATRIX]  T0 WHERE T0.[Name] = T2.[ItmsGrpNam]),0)
end [ColumnHeading1 Depreciation],

case when datediff(dd,T0.[DocDate], @DateFrom) >= @AgeColumn1 + 1 and datediff(dd,T0.[DocDate], @DateFrom) <= @AgeColumn2  then isnull((sum(Inqty) - sum(OutQty )) * T0.[CalcPrice],0) end [ColumnHeading2],
case when datediff(dd,T0.[DocDate], @DateFrom) >= @AgeColumn1 + 1 and datediff(dd,T0.[DocDate], @DateFrom) <= @AgeColumn2  then 
isnull((SELECT T0.[U_Col_1] FROM [dbo].[@AE_DEPN_MATRIX]  T0 WHERE T0.[Name] = T2.[ItmsGrpNam]),0)
end [ColumnHeading2 Depreciation],

case when datediff(dd,T0.[DocDate], @DateFrom) >= @AgeColumn2 + 1 and datediff(dd,T0.[DocDate], @DateFrom) <= @AgeColumn3 then isnull((sum(Inqty) - sum(OutQty )) * T0.[CalcPrice],0) end [ColumnHeading3],
case when datediff(dd,T0.[DocDate], @DateFrom) >= @AgeColumn2 + 1 and datediff(dd,T0.[DocDate], @DateFrom) <= @AgeColumn3 then 
isnull((SELECT T0.[U_Col_2] FROM [dbo].[@AE_DEPN_MATRIX]  T0 WHERE T0.[Name] = T2.[ItmsGrpNam]),0)
end [ColumnHeading3 Depreciation],

case when datediff(dd,T0.[DocDate], @DateFrom) >= @AgeColumn3 + 1 and datediff(dd,T0.[DocDate], @DateFrom) <= @AgeColumn4  then isnull((sum(Inqty) - sum(OutQty )) * T0.[CalcPrice],0) end [ColumnHeading4],
case when datediff(dd,T0.[DocDate], @DateFrom) >= @AgeColumn3 + 1 and datediff(dd,T0.[DocDate], @DateFrom) <= @AgeColumn4  then 
isnull((SELECT T0.[U_Col_3] FROM [dbo].[@AE_DEPN_MATRIX]  T0 WHERE T0.[Name] = T2.[ItmsGrpNam]),0)
end [ColumnHeading4 Depreciation],

case when datediff(dd,T0.[DocDate], @DateFrom) >= @AgeColumn4 + 1 and datediff(dd,T0.[DocDate], @DateFrom) <= @AgeColumn5 then isnull((sum(Inqty) - sum(OutQty )) * T0.[CalcPrice],0) end [ColumnHeading5],
case when datediff(dd,T0.[DocDate], @DateFrom) >= @AgeColumn4 + 1 and datediff(dd,T0.[DocDate], @DateFrom) <= @AgeColumn5 then 
isnull((SELECT T0.[U_Col_4] FROM [dbo].[@AE_DEPN_MATRIX]  T0 WHERE T0.[Name] = T2.[ItmsGrpNam]),0)
end [ColumnHeading5 Depreciation],

case when datediff(dd,T0.[DocDate], @DateFrom) >= @AgeColumn5 + 1 and datediff(dd,T0.[DocDate], @DateFrom) <= @AgeColumn6 then isnull((sum(Inqty) - sum(OutQty )) * T0.[CalcPrice],0) end [ColumnHeading6],
case when datediff(dd,T0.[DocDate], @DateFrom) >= @AgeColumn5 + 1  and datediff(dd,T0.[DocDate], @DateFrom) <= @AgeColumn6 then 
isnull((SELECT T0.[U_Col_5] FROM [dbo].[@AE_DEPN_MATRIX]  T0 WHERE T0.[Name] = T2.[ItmsGrpNam]),0)
end [ColumnHeading6 Depreciation] ,

case when datediff(dd,T0.[DocDate], @DateFrom) >= @AgeColumn7 then isnull((sum(Inqty) - sum(OutQty )) * T0.[CalcPrice],0) end [ColumnHeading7],
case when datediff(dd,T0.[DocDate], @DateFrom) >= @AgeColumn7  then 
isnull((SELECT T0.[U_Col_5] FROM [dbo].[@AE_DEPN_MATRIX]  T0 WHERE T0.[Name] = T2.[ItmsGrpNam]),0)
end [ColumnHeading7 Depreciation] 
INTO #STOCKDEPRECIATION
--,
--case when datediff(dd,T0.[DocDate], '20110801') >=0 and datediff(dd,T0.[DocDate], '20110801') <= 30 then (sum(Inqty) - sum(OutQty )) * T0.[CalcPrice] end [ColumnHeading1]
FROM [dbo].[OINM]  T0 INNER JOIN [dbo].[OITM]  T1 ON T0.[ItemCode] = T1.[ItemCode] INNER JOIN OITB T2 ON T1.[ItmsGrpCod] = T2.[ItmsGrpCod]
group by T2.[ItmsGrpNam],T0.[ItemCode],T0.[Warehouse],  datediff(dd,T0.[DocDate], @DateFrom),T0.[CalcPrice]
--having datediff(dd,T0.[DocDate], @DateFrom) > 0
order by datediff(dd,T0.[DocDate], @DateFrom)

--select * from #STOCKDEPRECIATION

truncate table StockDep

insert into StockDep 
SELECT @CompName, [ItmsGrpNam],  sum(isnull([ColumnHeading1],0)) [ColumnHeading1]
,sum(isnull([ColumnHeading2],0)) [ColumnHeading2],
sum(isnull([ColumnHeading3],0)) [ColumnHeading3],
sum(isnull([ColumnHeading4],0)) [ColumnHeading4],
sum(isnull([ColumnHeading5],0)) [ColumnHeading5],
sum(isnull([ColumnHeading6],0)) [ColumnHeading6],
sum(isnull([ColumnHeading7],0)) [ColumnHeading7],
sum(isnull([ColumnHeading1],0) * (isnull([ColumnHeading1 Depreciation],0) / 100.0))[ColumnHeading1 Dep], 
sum(isnull([ColumnHeading2],0) * (isnull([ColumnHeading2 Depreciation],0) / 100.0))[ColumnHeading2 Dep],
sum(isnull( [ColumnHeading3],0) * (isnull([ColumnHeading3 Depreciation],0) / 100.0))[ColumnHeading3 Dep],
sum(isnull( [ColumnHeading4],0) * (isnull([ColumnHeading4 Depreciation],0) / 100.0))[ColumnHeading4 Dep],
sum(isnull([ColumnHeading5],0) * (isnull([ColumnHeading5 Depreciation],0) / 100.0))[ColumnHeading5 Dep],
sum((isnull([ColumnHeading6],0) * (isnull([ColumnHeading6 Depreciation],0) / 100.0))) [ColumnHeading6 Dep],
sum((isnull([ColumnHeading7],0) * (isnull([ColumnHeading7 Depreciation],0) / 100.0))) [ColumnHeading7 Dep],
sum(isnull([ColumnHeading1],0) * (isnull([ColumnHeading1 Depreciation],0) / 100.0)) + 
sum(isnull([ColumnHeading2],0) * (isnull([ColumnHeading2 Depreciation],0) / 100.0)) +
sum(isnull( [ColumnHeading3],0) * (isnull([ColumnHeading3 Depreciation],0) / 100.0)) +
sum(isnull( [ColumnHeading4],0) * (isnull([ColumnHeading4 Depreciation],0) / 100.0)) +
sum(isnull([ColumnHeading5],0) * (isnull([ColumnHeading5 Depreciation],0) / 100.0)) +
sum(isnull([ColumnHeading6],0) * (isnull([ColumnHeading6 Depreciation],0) / 100.0))+
sum(isnull([ColumnHeading7],0) * (isnull([ColumnHeading7 Depreciation],0) / 100.0)) [Total Dep] ,

cast(@ColumnHeading1 as varchar(20)) + ' Days' as Header1,
cast(@ColumnHeading2 as varchar(20)) + ' Days' as Header2,
cast(@ColumnHeading3 as varchar(20)) + ' Days' as Header3,
cast(@ColumnHeading4 as varchar(20)) + ' Days' as Header4,
cast(@ColumnHeading5 as varchar(20)) + ' Days' as Header5,
cast(@ColumnHeading6 as varchar(20)) + ' Days' as Header6,
cast(@ColumnHeading7 as varchar(20)) + ' Days' as Header7

from #STOCKDEPRECIATION 
group by  [ItmsGrpNam]
--, [ColumnHeading1 Depreciation],[ColumnHeading1], [ColumnHeading2 Depreciation], 
--[ColumnHeading3 Depreciation]
--, [ColumnHeading4 Depreciation],[ColumnHeading5 Depreciation],
-- [ColumnHeading6 Depreciation],[ColumnHeading2],[ColumnHeading3],[ColumnHeading4],[ColumnHeading5],[ColumnHeading6]
 order by [ItmsGrpNam]

insert into [@AE_STKDEP_REG] (Code, Name, U_AE_Company, U_AE_ItemGroup, U_Column1, U_Column2, U_Column3, U_Column4, U_Column5, U_Column6, U_Column7,
U_Depreciation1, U_Depreciation2, U_Depreciation3, U_Depreciation4, U_Depreciation5, U_Depreciation6, U_Depreciation7, U_TotalDepreciation, U_Header1,
U_Header2, U_Header3, U_Header4, U_Header5, U_Header6, U_Header7, U_AE_Stat, U_AE_Month, U_AE_Year)
 select upper(left(DateName( month , @DateFrom),3)) + cast(year(@DateFrom) as varchar) + cast(Sno as varchar) [Code], 
 upper(left(DateName( month , @DateFrom),3)) + cast(year(@DateFrom) as varchar) + cast(Sno as varchar) [Name],
 Company [U_AE_Company], ItemGroup [U_AE_ItemGroup], Column1 [U_Column1], Column2 [U_Column2], Column3 [U_Column3], Column4 [U_Column4], Column5 [U_Column5], Column6 [U_Column6],
 Column7 [U_Column7],
 Depreciation1 [U_Depreciation1], Depreciation2 [U_Depreciation2], Depreciation3 [U_Depreciation3], Depreciation4 [U_Depreciation4], Depreciation5 [U_Depreciation5],
 Depreciation6 [U_Depreciation6], Depreciation7 [U_Depreciation7], TotalDepreciation [U_TotalDepreciation], Header1 [U_Header1], Header2 [U_Header2],
 Header3 [U_Header3], Header4 [U_Header4], Header5 [U_Header5], Header6 [U_Header6], Header7 [U_Header7],'O' [U_AE_Stat],  month(@DateFrom) [U_AE_Month],
 year(@DateFrom) [U_AE_Year] 
 from StockDep where TotalDepreciation >0
 --upper(left(DateName( month , @DateFrom),3))
 select * from [@AE_STKDEP_REG] where U_TotalDepreciation > 0
	
drop table #STOCKDEPRECIATION
drop table StockDep

END



