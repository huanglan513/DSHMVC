BEGIN TRANSACTION
GO
Insert dbo.[Function] (FunctionID,FunctionName,ParentID)
values (25,'������',0);
Insert dbo.[Function] (FunctionID,FunctionName,ParentID,Url)
values (26,'�������',25,'FinancialPayment/NotApplyPaymentList');
Insert dbo.[Function] (FunctionID,FunctionName,ParentID,Url)
values (27,'�������',25,'FinancialPayment/ApprovalPaymentList');
Insert dbo.[Function] (FunctionID,FunctionName,ParentID,Url)
values (28,'����',25,'FinancialPayment/PaymentList');

Insert dbo.Privilege (PrivilegeID,RoleID,FunctionID)
values(25,1,25);
Insert dbo.Privilege (PrivilegeID,RoleID,FunctionID)
values(26,1,26);
Insert dbo.Privilege (PrivilegeID,RoleID,FunctionID)
values(27,1,27);
Insert dbo.Privilege (PrivilegeID,RoleID,FunctionID)
values(28,1,28);
Commit;
