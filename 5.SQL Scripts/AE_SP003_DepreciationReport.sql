USE [STOCKDEP]
GO
/****** Object:  StoredProcedure [dbo].[AE_SP003_Depriciation]    Script Date: 4/28/2015 10:03:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--AE_SP003_DepreciationReport 'Beverages','Dairy Product','April','2015'

alter procedure [dbo].[AE_SP003_DepreciationReport]

@ItemGroupF nvarchar(100),
@ItemGroupT nvarchar(100),
@Month nvarchar(10),
@Year  nvarchar(10)
as
begin 

Declare @MonthNum as varchar(5)
set @MonthNum = Month(@month + ' 1 2015')

select

--------------------Company Info---------------------------------  

 OADM.CompnyName, isnull(OADM.Street,'') as 'Street' , isnull(OADM.StreetNo,'') as 'StreetNo' , isnull(OADM.Block,'') as 'Block' , isnull(OADM.Building,'') as 'Building' , 
isnull(OADM.City,'') as 'City' , isnull(OADM.Country,'') as 'Country' ,OADM.LogoImage ,isnull(OADM.ZipCode,'') as 'ZipCode', isnull(OADM.Name,'') as 'Name' , isnull(OADM.Phone1,'') as 'Phone1'
, isnull(OADM.E_Mail ,'') as 'E_Mail' , isnull(OADM.Fax , '') as 'Fax' ,
 ----OADM.Phone1,OADM.Phone2,OADM.GlblLocNum,OADM.Fax, OADM.FreeZoneNo, OADM.TaxIdNum,OADM.RevOffice,  
 ----OADM.CompnyAddr ,OADM.Block, OADM.Street, OADM.StreetNo , OADM.City ,OADM.Country, OADM.ZipCode,OADM.LogoImage,OADM.E_Mail,OADM.IntrntAdrs,  
 

T0.U_AE_ItemGroup,
T0.U_Column1,
T0.U_Column2,
T0.U_Column3,
T0.U_Column4,
T0.U_Column5,
T0.U_Column6,
T0.U_Column7,
T0.U_Depreciation1,
T0.U_Depreciation2,
T0.U_Depreciation3,
T0.U_Depreciation4,
T0.U_Depreciation5,
T0.U_Depreciation6,
T0.U_Depreciation7,
T0.U_AE_Month,
T0.U_AE_Year
from  [@AE_STKDEP_REG] T0

Left outer join   
(  
 select top(1) isnull(T0.PrintHeadr,T0.CompnyName) CompnyName,T0.Phone1,T0.E_Mail ,T1.GlblLocNum, T0.Fax, T0.FreeZoneNo, T0.TaxIdNum, 
   T0.CompnyAddr,T1.Street, T1.StreetNo , T1.Block, T1.Building, T1.ZipCode , T1.City, T1.Country , T3.LogoImage,T1.IntrntAdrs,T0.RevOffice, T2.Name  
 from OADM T0 with(nolock)   
  join ADM1 T1 with(nolock) on 1=1  
   join OADP T3 with(nolock) on 1=1  
    join OCST T2 with(nolock) on T2.Country  =T1.Country  
	) OADM on 1=1 
	

where T0.U_AE_ItemGroup between @ItemGroupF and @ItemGroupT  and T0.U_AE_Month=@MonthNum and T0.U_AE_Year=@Year 


end

