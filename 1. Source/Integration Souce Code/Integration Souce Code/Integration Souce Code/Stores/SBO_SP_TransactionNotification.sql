/****** Object:  StoredProcedure [dbo].[SBO_SP_TransactionNotification]    Script Date: 11/05/2013 11:18:54 ******/
ALTER proc [dbo].[SBO_SP_TransactionNotification] 

@object_type nvarchar(20), 				-- SBO Object Type
@transaction_type nchar(1),			-- [A]dd, [U]pdate, [D]elete, [C]ancel, C[L]ose
@num_of_cols_in_key int,
@list_of_key_cols_tab_del nvarchar(255),
@list_of_cols_val_tab_del nvarchar(255)

AS

begin

-- Return values
declare @error  int				-- Result (0 for no error)
declare @error_message nvarchar (200) 		-- Error string to be displayed
select @error = 0
select @error_message = N'Ok'

--------------------------------------------------------------------------------------------------------------------------------
declare @count int
declare @startDate date
declare @adminFee numeric(19,6)
declare @term1 int
declare @term2 int
declare @term3 int
declare @dailyInterest1 numeric(19,6)
declare @dailyInterest2 numeric(19,6)
declare @dailyInterest3 numeric(19,6)
declare @cardcode nvarchar(15)
declare @projectcode nvarchar(20)

If @object_type = N'46' and @num_of_cols_in_key = 1
Begin
	If @transaction_type = N'A' 
	begin
		set @startDate = (select IsNull(U_StartDate, DocDate) from OVPM where DocEntry = @list_of_cols_val_tab_del)
		set @adminFee = (select U_AdminF from OVPM where DocEntry = @list_of_cols_val_tab_del)
		set @term1 = (select U_Term from OVPM where DocEntry = @list_of_cols_val_tab_del)
		set @term2 = (select U_Term2 from OVPM where DocEntry = @list_of_cols_val_tab_del)
		set @term3 = (select U_Term3 from OVPM where DocEntry = @list_of_cols_val_tab_del)
		set @dailyInterest1 = (select U_DailyDisc from OVPM where DocEntry = @list_of_cols_val_tab_del)
		set @dailyInterest2 = (select U_DailyDisc2 from OVPM where DocEntry = @list_of_cols_val_tab_del)
		set @dailyInterest3 = (select U_DailyDisc3 from OVPM where DocEntry = @list_of_cols_val_tab_del)
		set @cardcode = (select CardCode from OVPM where DocEntry = @list_of_cols_val_tab_del)
		set @projectcode = (select PrjCode from OVPM where DocEntry = @list_of_cols_val_tab_del)
		-- Term 1
		set @count = 0
		while @count < @term1	
		begin
			Insert into InterestOVPM select @list_of_cols_val_tab_del, 1 , @count + 1,
											DATEADD(day, @count, @startDate),@dailyInterest1,
											0,'',0, @cardcode, @projectcode
											
			set @count = @count + 1
		end
		
		-- Term 2
		set @count = 0
		while @count < @term2	
		begin
			Insert into InterestOVPM select @list_of_cols_val_tab_del, 2 , @count + @term1 + 1,
											DATEADD(day, @count + @term1, @startDate),@dailyInterest2,
											0,'',0, @cardcode, @projectcode
											
			set @count = @count + 1
		end
		
		-- Term 3
		set @count = 0
		while @count < @term3
		begin
			Insert into InterestOVPM select @list_of_cols_val_tab_del, 3 , @count + @term1 + @term2 + 1,
											DATEADD(day, @count + @term1 + @term3, @startDate),@dailyInterest3,
											0,'',0, @cardcode, @projectcode
											
			set @count = @count + 1
		end
		--------------------------

		Update OVPM 
		Set U_Outstanding = U_Principal
		Where DocEntry = @list_of_cols_val_tab_del
	end
	else if @transaction_type = N'C' 
	begin
		Update InterestOVPM
		Set Cancelled = 1
		Where DocEntry = @list_of_cols_val_tab_del
		
		Insert into CancelOVPM select @list_of_cols_val_tab_del ,0, @cardcode, @projectcode
	end
End

--------------------------------------------------------------------------------------------------------------------------------

-- Select the return values
select @error, @error_message

end