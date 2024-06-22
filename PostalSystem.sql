create database PostalSystem
go

use PostalSystem
go

create table Clients
(
	Id int not null primary key identity,
	Login nvarchar(30) not null unique check(Login != N''),
	Password nvarchar(30) not null check(Password != N''),

	constraint CHK_LoginAndPasswordNotEqual check (Login != Password)
)

create table ParcelTypes
(
	Id int not null primary key identity,
	Type nvarchar(20) not null unique check(Type != N''),
	CostMultiplier int not null check(CostMultiplier > 1)
)

create table Buildings
(
	Id int not null primary key identity,
	X int not null,
	Y int not null
)

create table Parcels
(
	Id int not null primary key identity,
	SenderId int not null,
	ReceiverId int not null,
	FromBuildingId int not null,
	ToBuildingId int not null,
	TypeId int not null,
	Description nvarchar(100) not null check(Description != N''),
	Distance int not null default 0,
	Cost int not null default 0,

	constraint CHK_SenderAndReceiverIdsNotEqual check (SenderId != ReceiverId),
	constraint CHK_BuildingsNotEqual check (FromBuildingId != ToBuildingId),
	constraint FK_SenderId foreign key (SenderId) references Clients(Id),
	constraint FK_ReceiverId foreign key (ReceiverId) references Clients(Id),
	constraint FK_FromBuildingId foreign key (FromBuildingId) references Buildings(Id),
	constraint FK_ToBuildingId foreign key (ToBuildingId) references Buildings(Id),
	constraint FK_TypeId foreign key (TypeId) references ParcelTypes(Id)
)
go

create trigger CalculateDistance on Parcels after insert as
begin
    update Parcels
    set Distance = round(sqrt(power(b_to.x - b_from.x, 2) + power(b_to.y - b_from.y, 2)), 0),
        Cost = round(sqrt(power(b_to.x - b_from.x, 2) + power(b_to.y - b_from.y, 2)), 0) * pt.CostMultiplier
    from Parcels as p
    inner join inserted as i on p.id = i.id
    inner join Buildings as b_to on p.ToBuildingId = b_to.Id
    inner join Buildings as b_from on p.FromBuildingId = b_from.Id
    inner join ParcelTypes as pt on p.TypeId = pt.Id;
end