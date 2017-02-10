Declare @BPcode as nvarchar(20)
Declare @PrjCode as nvarchar(50)
Declare @DocEntry as int 
Declare @Day as int
Declare @Term as int
Declare @Term1 As int  
Declare @Term2 As int  
Declare @Term3 As int  
Declare @DailyDisc As numeric(19,6)  
Declare @DailyDisc2 As numeric(19,6)  
Declare @DailyDisc3 As numeric(19,6)  
Declare @OutStanding numeric(19,6)
  
  
set @BPcode = '{0}'
set @PrjCode = '{1}'
set @DocEntry = {2}
set @Term = {3}
set @Day = {4}

set @OutStanding = (Select SUM(Debit - Credit) From JDT1 where ShortName = @BPcode and Project = @PrjCode)  
set @Term1 = (select IsNUll(U_Term,0) from OVPM where DocEntry = @DocEntry)
set @Term2 = (select IsNUll(U_Term2,0) from OVPM where DocEntry = @DocEntry)
set @Term3 = (select IsNUll(U_Term3,0) from OVPM where DocEntry = @DocEntry)

set @DailyDisc = (select IsNUll(U_DiscountFees,0) from OVPM where DocEntry = @DocEntry)
set @DailyDisc2 = (select IsNUll(U_DiscountFees2,0) from OVPM where DocEntry = @DocEntry)
set @DailyDisc3 = (select IsNUll(U_DiscountFee3,0) from OVPM where DocEntry = @DocEntry)

IF @Term = 1
Begin
	if @Day = @Term1
	begin
		UPDATE InterestOVPM 
        Set Value =  Round(@Outstanding * (@DailyDisc2 / 100 / 30),2)
        Where DocEntry = @DocEntry and Term = 2 and CardCode = @BPcode and ProjectCode = @PrjCode
        
        UPDATE OVPM 
		Set U_NewDailyDisc = Round(@Outstanding * (@DailyDisc2 / 100 / 30),2) ,
			U_DailyDisc2 = Round(@Outstanding * (@DailyDisc2 / 100 / 30),2)
		where DocEntry = @DocEntry
        
        UPDATE InterestOVPM 
        Set Value =  Round(@Outstanding * (@DailyDisc3 / 100 / 30),2)
        Where DocEntry = @DocEntry and Term = 3 and CardCode = @BPcode and ProjectCode = @PrjCode
	end
End
Else If @Term = 2
Begin
	if @Term2 = @Day - @Term1 
	begin
		UPDATE InterestOVPM 
        Set Value =  Round(@Outstanding * (@DailyDisc3 / 100 / 30),2)
        Where DocEntry = @DocEntry and Term = 3 and CardCode = @BPcode and ProjectCode = @PrjCode
        
        UPDATE OVPM 
		Set U_NewDailyDisc = Round(@Outstanding * (@DailyDisc3 / 100 / 30),2) ,
			U_DailyDisc3 = Round(@Outstanding * (@DailyDisc2 / 100 / 30),2)
		where DocEntry = @DocEntry
	end
End